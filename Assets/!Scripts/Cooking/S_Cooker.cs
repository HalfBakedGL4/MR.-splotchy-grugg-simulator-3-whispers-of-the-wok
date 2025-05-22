using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Fusion;

public enum CookerType
{
    Oven,
    Fryer,
}
public class S_Cooker : NetworkBehaviour
{
    private enum CookerState
    {
        Available,
        Cooking,
        Finished,
    }
    

    [SerializeField] private CookerType cookerType;
    
    [Header("Sockets")]
    [SerializeField] private S_SocketTagInteractor[] foodSockets;
    [SerializeField] private S_SocketTagInteractor dishSocket;
    
    [Networked] private CookerState state { get; set; } = CookerState.Available;

    private NetworkLinkedList<FoodType> _foodCooking = new ();
    private NetworkLinkedList<S_Food> _foodScripts = new ();

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
    [Networked] private float timer { get; set; }
    private void Start()
    {

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
        
        // Turn off Dishsocket, should only be used when dish is spawned
        dishSocket.socketActive = false;
    }

    void Update()
    {
        switch (state)
        {
            case CookerState.Available:
                break;
            case CookerState.Cooking:
                if (isLocal)
                {
                    timer += Time.deltaTime;
                    
                    if (cookerType == CookerType.Oven)
                    {
                        SetDishStatus();
                    }
                }
                break;
            case CookerState.Finished:
                break;
        }
        cookTimer.UpdateTimer(timer);

    }


    private void AddFood(SelectEnterEventArgs args)
    {
        if (!isLocal) { return; }
        // Add food from food list
        if (args.interactableObject.transform.TryGetComponent(out S_Food food)) {
            _foodCooking.Add(food.GetFoodType());
            _foodScripts.Add(food);
            // When the first ingredient is added start the cooker if possible
            if (_foodCooking.Count == 1)
            {
                InteractWithCooker();
            }
            else if (_foodCooking.Count > 1)
            {
                AddTime();
            }
        }
    }

    private void RemoveFood(SelectExitEventArgs args)
    {
        if (!isLocal) { return; }
        // Remove food from food list
        if (args.interactableObject.transform.TryGetComponent(out S_Food food)){
            _foodCooking.Remove(food.GetFoodType());
            _foodScripts.Remove(food);
        }
    }
    
    private void RemoveSocket(SelectExitEventArgs arg0)
    {
        dishSocket.socketActive = false;
    }

    public void InteractWithCooker()
    {
        print("Interacting with Cooker " + name);
        // Activate Cooker and start timer
        if (state == CookerState.Available && isAbleToStartCooking)
        {
            timer = 0.0f;
            cookTimer.TimerToggle(true);
            // Turn off colliders so they can't be picked up while cooking
            foreach (var foodScript in _foodScripts)
            {
                foodScript.ToggleColliders();
            }
            RPC_SetCookerState(CookerState.Cooking);
            
        }
        // Stop Cooker and empty food items inside
        else if (state == CookerState.Cooking)
        {
            _currentDishStatus = SpawnDish();
            cookTimer.TimerToggle(false);

            RPC_SetCookerState(CookerState.Available);
        }
    }

    // Spawns the dish in the dish sockets and returns the dish if it needs to change
    private S_DishStatus SpawnDish()
    {
        Dish dish = GetDish();
        Debug.Log(dish);
        if (!dish.resultPrefab.TryGetComponent(out NetworkObject dishToSpawn))
        {
            Debug.LogError("Couldnt get a networkobject");
            return null;
        }

        DishStatus dishStatus = CheckDishStatus();

        CleanCooker();
        
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

    
    private void CleanCooker()  // Moves items in cooker under the stage. possible to use object pooling.
    {
        S_Food[] foodList = _foodScripts.ToArray();
        foreach (var foodScript in foodList)
        {
            foodScript.Toggle();
            foodScript.transform.position = new Vector3(0,-10,0);
            S_GameManager.TryDespawnFood(foodScript);
        }

        _foodScripts.Clear();
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
            
            RPC_SetCookerState(CookerState.Available);
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
            InteractWithCooker();
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

    [Rpc(sources: RpcSources.All, targets: RpcTargets.StateAuthority)]
    void RPC_SetCookerState(CookerState state)
    {
        this.state = state;
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
