using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Fusion;
using Extentions.Addressable;
using TMPro;
using Unity.VisualScripting;

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
    
    [SerializeField, Networked] private CookerState state { get; set; } = CookerState.Available;

    private List<FoodType> foodCooking = new ();
    private List<S_Food> foodScripts = new ();

    
    [Space]
    [Header("Timer related")]
    [SerializeField] private float underCookedTime = 10;
    [SerializeField] private float perfectlyCookedTime = 20;
    [SerializeField] private float overCookedTime = 30;
    TMP_Text timerText;

    
    
    private bool isAbleToStartCooking = true;
    bool isLocal => Object && Object.HasStateAuthority;
    [SerializeField, Networked] private float timer { get; set; }
    private async void Start()
    {
        timerText = GetComponentInChildren<TMP_Text>();

        // Subscribe to foodSockets Events
        foreach (var foodSocket in foodSockets)
        {
            foodSocket.selectEntered.AddListener(AddFood);
            foodSocket.selectExited.AddListener(RemoveFood);
        }

        if (cookerType == CookerType.Oven)
        {
            dishSocket.selectExited.AddListener(StopOven);
        }
    }

    void Update()
    {
        switch (state)
        {
            case CookerState.Available:
                timerText.text = "Available";
                break;
            case CookerState.Cooking:
                if(isLocal)
                    timer += Time.deltaTime;

                timerText.text = "Timer: " + timer.ToString("..");
                //TODO: update the timer UI
                break;
            case CookerState.Finished:
                timerText.text = "Finished!";
                break;
        }
        
    }


    private void AddFood(SelectEnterEventArgs args)
    {
        // Add food from food list
        if (args.interactableObject.transform.TryGetComponent(out S_Food food)) {
            foodCooking.Add(food.GetFoodType());
            foodScripts.Add(food);
            // When the first ingredient is added start the cooker if possible
            if (foodCooking.Count == 1)
            {
                InteractWithCooker();
            }
            else if (foodCooking.Count > 1)
            {
                AddTime();
            }
        }
    }

    private void RemoveFood(SelectExitEventArgs args)
    {
        // Remove food from food list
        if (args.interactableObject.transform.TryGetComponent(out S_Food food)){
            foodCooking.Remove(food.GetFoodType());
            foodScripts.Remove(food);
        }
    }

    public void InteractWithCooker()
    {
        print("Interacting with Cooker " + name);
        // Activate Cooker and start timer
        if (state == CookerState.Available && isAbleToStartCooking)
        {
            timer = 0.0f;

            // Turn off colliders so they can't be picked up while cooking
            foreach (var foodScript in foodScripts)
            {
                foodScript.ToggleColliders();
            }
            //TODO: animation that closes the cooker or shows cooker cooking
            RPC_SetCookerState(CookerState.Cooking);
            
        }
        // Stop Cooker and empty food items inside
        else if (state == CookerState.Cooking)
        {
            Dish dish = GetDish();
            Debug.Log(dish);
            if (!dish.resultPrefab.TryGetComponent(out NetworkObject dishToSpawn))
            {
                Debug.LogError("Couldnt get a networkobject");
                return;
            }

            DishStatus dishStatus = DishStatus.UnCooked;

            // Undercooked
            if (timer < underCookedTime)
            {
                dishStatus = DishStatus.UnderCooked;
            }
            // Perfect
            else if (timer < perfectlyCookedTime)
            {
                dishStatus = DishStatus.Cooked;

            }
            // Overcooked
            else if (timer < overCookedTime)
            {
                dishStatus = DishStatus.OverCooked;

            }
            // Burnt
            else
            {
                dishStatus = DishStatus.Burnt;
            }

            CleanCooker();
            var spawnedDish =  Runner.Spawn(dishToSpawn, dishSocket.transform.position, dishSocket.transform.rotation);
            if (spawnedDish.TryGetComponent(out S_DishStatus dishStatusScript))
            {
                dishStatusScript.ChangeStatus(dishStatus);
            }
            // TODO: Animation that opens the cooker or show it stops cooking

            RPC_SetCookerState(CookerState.Available);
        }
    }

    private Dish GetDish() //Looks through the RecipeBook to see if any dish is created from those ingredients
    {
        var dishInfo = S_RecipeDatabase.FindMatchingRecipe(foodCooking, cookerType);
        
        if (dishInfo != null)
        {
            return dishInfo;
        }
        
        return null;
    }

    
    //we can do object pooling later as it requires RPC calling and stuff to make happen
    private void CleanCooker()  // Moves items in cooker under the stage. possible to use object pooling.
    {
        S_Food[] foodList = foodScripts.ToArray();
        foreach (var foodScript in foodList)
        {
            foodScript.Toggle();
            foodScript.transform.position = new Vector3(0,-10,0);
            Runner.Despawn(foodScript.GetComponent<NetworkObject>());
        }

        foodScripts.Clear();
    }

    #region OvenSpecifics

    // When food are added to the Oven increase the timer
    private void AddTime()
    {
        
    }

    private void StopOven(SelectExitEventArgs args)
    {
        if (state == CookerState.Cooking)
            InteractWithCooker();
    }
    #endregion
    
    #region FryerSpecifics

    // When the basket is placed in the fryer, check if there is food to cook
    // Connect this to the editor
    public void CheckIfFryerShouldStart(S_Cooker fryer)
    {
        print("Interacting with Socket " + name);

        isAbleToStartCooking = true;
        
        if (foodCooking.Count > 0)
        {
            InteractWithCooker();
        }
    }
    public void CantCook()
    {
        isAbleToStartCooking = false;
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
    }
}
