using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Fusion;
using Extentions.Addressable;
using NaughtyAttributes;

public enum CookerType
{
    Oven,
    Fryer,
}
public class S_Cooker : NetworkBehaviour, IButtonObject
{
    private enum CookerState
    {
        Available,
        Cooking,
        Finished,
    }
    

    [SerializeField] private CookerType cookerType;
    private GameObject burntSlop;
    
    [Header("Cooking Timers")]
    [SerializeField] private float goodTime;
    [SerializeField] private float badTime;
    [SerializeField] private float worstTime;
    
    [Header("Sockets")]
    [SerializeField] private S_SocketTagInteractor[] foodSocket;
    [SerializeField] private S_SocketTagInteractor dishSocket;

    
    private CookerState state = CookerState.Available;

    private List<FoodType> foodCooking = new ();
    private List<S_Food> foodScripts = new ();

    private float timer;
    private async void Start()
    {
        burntSlop = await Addressable.LoadAsset<GameObject>(Addressable.names[3]);
    }

    void Update()
    {
        switch (state)
        {
            case CookerState.Available:
                break; 
            case CookerState.Cooking:
                timer += Time.deltaTime;
                //TODO: update the timer UI
                break;
            case CookerState.Finished:
                break;
        }
    }

    public void AddFood(SelectEnterEventArgs args)
    {
        // Add food from food list
        if (args.interactableObject.transform.TryGetComponent(out S_Food food)) {
            foodCooking.Add(food.GetFoodType());
            foodScripts.Add(food);
        }
    }

    public void RemoveFood(SelectExitEventArgs args)
    {
        // Remove food from food list
        if (args.interactableObject.transform.TryGetComponent(out S_Food food)){
            foodCooking.Remove(food.GetFoodType());
            foodScripts.Remove(food);
        }
    }

    public void OnButtonPressed()
    {
        print("Interacting with Cooker " + name);
        // Activate Cooker and start timer
        if (state == CookerState.Available)
        {
            timer = 0.0f;

            // Turn off colliders so they can't be picked up while cooking
            foreach (var foodScript in foodScripts)
            {
                foodScript.ToggleColliders();
            }
            //TODO: animation that closes the cooker or shows cooker cooking
            state = CookerState.Cooking;
        }
        // Stop Cooker and empty food items inside
        else if (state == CookerState.Cooking)
        {
            GameObject dish = GetDish();
            Debug.Log(dish);
            if (!dish.TryGetComponent(out NetworkObject dishToSpawn))
            {
                Debug.LogError("Couldnt get a networkobject");
                return;
            }

            DishStatus dishStatus = DishStatus.UnCooked;

            // Undercooked
            if (timer < goodTime)
            {
                dishStatus = DishStatus.UnderCooked;
            }
            // Perfect
            else if (timer < badTime)
            {
                dishStatus = DishStatus.Cooked;

            }
            // Overcooked
            else if (timer < worstTime)
            {
                dishStatus = DishStatus.OverCooked;

            }
            // Burnt
            else
            {
                // Cannot be served
                dishStatus = DishStatus.Burnt;
            }

            CleanCooker();
            var spawnedDish =  Runner.Spawn(dishToSpawn, dishSocket.transform.position, dishSocket.transform.rotation);
            if (spawnedDish.TryGetComponent(out S_DishStatus dishStatusScript))
            {
                dishStatusScript.ChangeStatus(dishStatus);
            }
            // TODO: Animation that opens the cooker or show it stops cooking
            
            state = CookerState.Available;
        }
    }

    private GameObject GetDish() //Looks through the RecipeBook to see if any dish is created from those ingredients
    {
        var dishInfo = S_RecipeDatabase.FindMatchingRecipe(foodCooking, cookerType);
        
        if (dishInfo != null)
        {
            return dishInfo.resultPrefab;
        }
        
        return burntSlop;
    }

    //we can do object pooling later as it requires RPC calling and stuff make happen
    private void CleanCooker()  // Moves items in cooker under the stage. possible to use object pooling.
    {
        S_Food[] foodList = foodScripts.ToArray();
        foreach (var foodScript in foodList)
        {
            Runner.Despawn(foodScript.GetComponent<NetworkObject>());

            //foodScript.Toggle();
            //foodScript.transform.position = new Vector3(0,-10,0);
        }

        foodScripts.Clear();
    }
}
