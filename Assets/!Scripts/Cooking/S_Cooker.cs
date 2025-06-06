using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Fusion;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.Events;

public enum CookerType
{
    Oven,
    Fryer,
}
public class S_Cooker : NetworkBehaviour, IToggle
{
    private enum CookerState
    {
        Available,
        Cooking,
        Finished,
    }
    

    [SerializeField] private CookerType cookerType;
    [Networked, SerializeField] private CookerState state { get; set; } = CookerState.Available;

    [Header("Sockets")]
    [SerializeField] private S_SocketTagInteractor[] foodSockets;
    [SerializeField] private S_SocketTagInteractor dishSocket;
    
    [Header("Interactable to turn On/Off")]
    [SerializeField] private XRBaseInteractable[] interactable;

    [Networked, SerializeField, Capacity(4)] private NetworkLinkedList<FoodType> _foodCooking => default;
    [Networked, SerializeField, Capacity(4)] private NetworkLinkedList<S_Food> _foodScripts => default;

    private S_DishStatus _currentDishStatus;
    private GameObject _spawnedDish;

    [Space]
    
    [Header("Timer until they are no longer that state")]
    [SerializeField] private float underCookedTime = 10;
    [SerializeField] private float perfectlyCookedTime = 20;
    [SerializeField] private float overCookedTime = 30;
    
    [SerializeField] private S_CookTimer cookTimer;

    private float _savedUnderCookedTime;
    private float _savedPerfectlyCookedTime;
    private float _savedOverCookedTime;
    
    [Networked] private bool isAbleToStartCooking { get; set; } = true;
    bool isLocal => Object && Object.HasStateAuthority;
    [Networked, SerializeField] private float timer { get; set; }
    
    [Networked, SerializeField] private bool isTurnedOn { get; set; }

    public UnityEvent StartCooking;
    public UnityEvent StoppedCooking;
    public override void Spawned()
    {
        base.Spawned();
        
        ConnectToApplicationManager();

        // Subscribe to foodSockets Events
        foreach (var foodSocket in foodSockets)
        {
            foodSocket.selectEntered.AddListener(AddFood);
            foodSocket.selectExited.AddListener(RemoveFood);
        }
        
        dishSocket.selectExited.AddListener(RemoveSocket);

        if (cookerType == CookerType.Oven)
        {
            dishSocket.selectExited.AddListener(StopOven);
        }
        
        // Just storing the values to increase for Oven
        _savedUnderCookedTime = underCookedTime;
        _savedPerfectlyCookedTime = perfectlyCookedTime;
        _savedOverCookedTime = overCookedTime;
        
        cookTimer.SetAllTimers(underCookedTime, perfectlyCookedTime, overCookedTime);
        
        // Turn off Dish Socket, should only be used when dish is spawned
        dishSocket.socketActive = false;
        
    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
        if (!isTurnedOn) {return;}

        cookTimer.UpdateTimer(timer);

        switch (state)
        {
            case CookerState.Available:
                break;
            case CookerState.Cooking:
                if (isLocal)
                {
                    timer += Time.fixedDeltaTime;
                    
                    if (cookerType == CookerType.Oven)
                    {
                        SetDishStatus();
                    }
                }
                break;
            case CookerState.Finished:
                break;
        }
    }
    
    private void AddFood(SelectEnterEventArgs args)
    {
        // Add food from food list
        if (args.interactableObject.transform.TryGetComponent(out S_Food food)) {
            RPC_AddFood(food);
        }
    }
    private void RemoveFood(SelectExitEventArgs args)
    {
        // Remove food from food list
        if (args.interactableObject.transform.TryGetComponent(out S_Food food))
        {
            RPC_RemoveFood(food);
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    void RPC_AddFood(S_Food food)
    {
        Debug.Log("[Cooker] add " + food + " to " + name);

        _foodCooking.Add(food.GetFoodType());
        _foodScripts.Add(food);
        // When the first ingredient is added start the cooker if possible
        if (_foodCooking.Count == 1)
        {
            RPC_InteractWithCooker();
        }
        else if (_foodCooking.Count > 1)
        {
            AddTime();
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    void RPC_RemoveFood(S_Food food)
    {
        Debug.Log("[Cooker] remove " + food + " from " + name);

        _foodCooking.Remove(food.GetFoodType());
        _foodScripts.Remove(food);
    }

    private void RemoveSocket(SelectExitEventArgs arg0)
    {
        dishSocket.socketActive = false;
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_InteractWithCooker()
    {
        if (!isTurnedOn) {return;}
        print("[Cooker] Interacting with " + name);

        // Activate Cooker and start timer
        if (state == CookerState.Available && isAbleToStartCooking)
        {
            print("[Cooker] Activate " + name);

            // Turn off colliders so they can't be picked up while cooking
            foreach (var foodScript in _foodScripts)
            {
                foodScript.ToggleColliders();
            }

            SetCookerState(CookerState.Cooking);
        }
        // Stop Cooker and empty food items inside
        else if (state == CookerState.Cooking)
        {
            _currentDishStatus = SpawnDish();

            SetCookerState(CookerState.Available);
        }
    }

    // Spawns the dish in the dish sockets and returns the dish if it needs to change
    private S_DishStatus SpawnDish()
    {
        print("[Cooker] Spawn Dish");

        Dish dish = GetDish();
        Debug.Log(dish);
        if (!dish.resultPrefab.TryGetComponent(out NetworkObject dishToSpawn))
        {
            Debug.LogError("Couldnt get a networkobject");
            return null;
        }

        DishStatus dishStatus = CheckDishStatus();

        RPC_CleanCooker();
        
        dishSocket.socketActive = true;

        var dishInstance =  Runner.Spawn(dishToSpawn, dishSocket.transform.position, dishSocket.transform.rotation);
        if (dishInstance.TryGetComponent(out S_DishStatus dishStatusScript))
        {
            dishStatusScript.ChangeStatus(dishStatus);
            return dishStatusScript;
        }

        return null;
    }

    // Check and return what cooking state the Dish is in. E.g. UnderCooked 
    private DishStatus CheckDishStatus()
    {
        DishStatus dishStatus;
        if (timer < underCookedTime)                // Undercooked
        {
            dishStatus = DishStatus.UnderCooked;
        }
        else if (timer < perfectlyCookedTime)       // Perfect
        {
            dishStatus = DishStatus.Cooked;
        }
        else if (timer < overCookedTime)            // Overcooked
        {
            dishStatus = DishStatus.OverCooked;
        }
        else                                        // Burnt
        {
            dishStatus = DishStatus.Burnt;
        }

        return dishStatus;
    }

    private Dish GetDish() //Looks through the RecipeBook to see if any dish is created from those ingredients
    {
        var listFoodCooking = _foodCooking.ToList();
        var dishInfo = S_RecipeDatabase.FindMatchingRecipe(listFoodCooking, cookerType);
        
        return dishInfo;
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_CleanCooker()  // Moves items in cooker under the stage. possible to use object pooling.
    {
        Debug.Log("[Cooker] Clean");

        S_Food[] foodList = _foodScripts.ToArray();
        foreach (var foodScript in foodList)
        {
            foodScript.Toggle();
            foodScript.transform.position = new Vector3(0,-10,0);
            S_GameManager.TryDespawnFood(foodScript);
        }

        _foodScripts.Clear();
    }

    void SetCookerState(CookerState state)
    {
        Debug.Log("[Cooker] " + state);
        this.state = state;

        RPC_OnUpdateCookerState(state);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    void RPC_OnUpdateCookerState(CookerState state)
    {
        switch (this.state)
        {
            case CookerState.Available:
                {
                    timer = 0.0f;
                    cookTimer.TimerToggle(true);
                    StoppedCooking?.Invoke();
                    break;
                }
            case CookerState.Cooking:
                {
                    cookTimer.TimerToggle(false);
                    StartCooking?.Invoke();
                    break;
                }
        }
    }

    #region OvenSpecifics

    // When food are added to the Oven increase the timer
    private void AddTime()
    {
        underCookedTime += 5;
        perfectlyCookedTime += 5;
        overCookedTime += 5;
        cookTimer.SetAllTimers(underCookedTime, perfectlyCookedTime, overCookedTime);
    }

    // Dish 
    private void SetDishStatus()
    {
        if (_currentDishStatus)
        {
            var newStatus = CheckDishStatus();
            var currentStatus = _currentDishStatus.GetDishStatus().dishStatus;
            if (newStatus == DishStatus.Burnt)
            {
                Runner.Despawn(_spawnedDish.GetComponent<NetworkObject>());
                SpawnDish();
            }
            else if (newStatus != currentStatus)
                _currentDishStatus.ChangeStatus(newStatus);
            
        }
        else if (timer > underCookedTime)
        {
            _currentDishStatus = SpawnDish();
            _spawnedDish = _currentDishStatus.gameObject;
            // Disable the food socket so that the oven doesn't break
            foreach (var socket in foodSockets)
            {
                socket.socketActive = false;
            }
            dishSocket.socketActive = true;
        }
    }

    // When Dish is removed make the oven available and ready to cook next dish
    private void StopOven(SelectExitEventArgs args)
    {
        if (state == CookerState.Cooking)
        {
            _currentDishStatus = null;

            underCookedTime = _savedUnderCookedTime;
            perfectlyCookedTime = _savedPerfectlyCookedTime;
            overCookedTime = _savedOverCookedTime;
            
            cookTimer.TimerToggle(false);
            
            // Enable the food socket so that the oven can be used again
            foreach (var socket in foodSockets)
            {
                socket.socketActive = true;
            }
            
            dishSocket.socketActive = false;
            
            SetCookerState(CookerState.Available);
        }
    }
    #endregion
    
    #region FryerSpecifics

    // When the basket is placed in the fryer, check if there is food to cook
    // Connect this to the editor
    public void CheckIfFryerShouldStart()
    {
        RPC_SetCookerAbleToCook(true);
        
        if (_foodCooking.Count > 0)
        {
            RPC_InteractWithCooker();
        }
    }
    public void CantCook()
    {
        RPC_SetCookerAbleToCook(false);
    }
    
    // If the basket has something in it and is let go the items inside will be disabled so that there won't be any collider issues
    public void DisableFoodInBasket(SelectExitEventArgs args = null)
    {
        RPC_SetFoodColliderToggle(false);
    }

    // When picked up items then are enabled again to be able to pick them up
    public void EnableFoodInBasket(SelectEnterEventArgs args = null)
    {
        RPC_SetFoodColliderToggle(true);
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    void RPC_SetFoodColliderToggle(bool toggle)
    {
        if (_foodScripts.Count == 1)
        {  
            _foodScripts[0].ToggleColliders(toggle);
        }
    }
    
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    void RPC_SetCookerAbleToCook(bool isActive)
    {
        isAbleToStartCooking = isActive;
    }

    #endregion
    
    public void SetApplicationActive(bool toggle)
    {
        isTurnedOn = toggle;
        
        
        print(name + " is turned on: " + toggle);

        RPC_ToggleMovement(toggle);

    }

    private XRGrabInteractable _grabInteractable;
    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_ToggleMovement(bool toggle)
    {
        foreach (var socket in foodSockets)
        {
            socket.socketActive = toggle;
        }
        dishSocket.socketActive = toggle;
        foreach (var interact in interactable)
        {
            interact.enabled = toggle;
        }
        
        if (_grabInteractable == null)
        {
            _grabInteractable = GetComponent<XRGrabInteractable>();
        }
        
        // Is opposite of toggle because it needs to be on when everything is off
        if (cookerType == CookerType.Fryer)
        {
            return;
        }
        _grabInteractable.enabled = !toggle;
    }
    public void ConnectToApplicationManager()
    {
        if (S_ApplicationManager.Instance != null)
        {
            S_ApplicationManager.Instance.RegisterToggle(this);
        }
    }

    private void OnDestroy()
    {
        foreach (var foodSocket in foodSockets)
        {
            foodSocket.selectEntered.RemoveListener(AddFood);
            foodSocket.selectExited.RemoveListener(RemoveFood);
        }
        dishSocket.selectExited.RemoveListener(RemoveSocket);
        
        if (cookerType == CookerType.Oven)
        {
            dishSocket.selectExited.RemoveListener(StopOven);
        }
    }
}
