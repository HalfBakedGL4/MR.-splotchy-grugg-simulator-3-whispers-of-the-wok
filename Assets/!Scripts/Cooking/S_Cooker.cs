using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public enum CookerType
{
    Oven,
    Fryer,
}
public class S_Cooker : MonoBehaviour
{
    private enum CookerState
    {
        Available,
        Cooking,
        Finished,
    }
    

    [SerializeField] private CookerType cookerType;
    [SerializeField] private GameObject burntSlop;
    
    [Header("Cooking Timers")]
    [SerializeField] private float goodTime;
    [SerializeField] private float badTime;
    [SerializeField] private float worstTime;
    
    [Header("Sockets")]
    [SerializeField] private S_SocketTagInteractor[] foodSocket;
    [SerializeField] private S_SocketTagInteractor dishSocket;

    
    private CookerState state = CookerState.Available;

    private S_RecipeDatabase _RecipeDatabase;

    private void Awake()
    {
        // Find the RecipeDatabase in the scene
        _RecipeDatabase = FindObjectOfType<S_RecipeDatabase>();

        if (_RecipeDatabase == null)
        {
            Debug.LogError("RecipeBook not found in the scene!");
        }
    }

    private List<FoodType> foodCooking = new ();
    private List<S_Food> foodScripts = new ();

    private float timer;
    
    void Update()
    {
        switch (state)
        {
            case CookerState.Available:
                break; 
            case CookerState.Cooking:
                timer += Time.deltaTime;
                // update the timer UI
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
    
    public void InteractWithCooker()
    {
        print("Interacting with Cooker");
        // Activate Cooker and start timer
        if (state == CookerState.Available)
        {
            timer = 0.0f;

            // Turn off colliders so they can't be picked up while cooking
            foreach (var foodScript in foodScripts)
            {
                foodScript.TurnOffColliders();
            }
            state = CookerState.Cooking;
        }
        // Stop Cooker and empty food items inside
        else if (state == CookerState.Cooking)
        {
            GameObject dishToSpawn;

            // Undercooked
            if (timer < goodTime)
            {
                dishToSpawn = GetDish();
                // Negative points because undercooked
            }
            // Perfect
            else if (timer < badTime)
            {
                dishToSpawn = GetDish();
            }
            // Overcooked
            else if (timer < worstTime)
            {
                dishToSpawn = GetDish();
                // Negative points because overcooked
            }
            // Burnt
            else
            {
                // Cannot be served
                dishToSpawn = burntSlop;
            }

            CleanCooker();
            Instantiate(dishToSpawn, dishSocket.transform.position, dishSocket.transform.rotation);
            state = CookerState.Available;
        }
    }

    private GameObject GetDish()
    {
        var dishInfo = _RecipeDatabase.FindMatchingRecipe(foodCooking, cookerType);
        
        if (dishInfo != null)
        {
            print(dishInfo.name);
            return dishInfo.resultPrefab;
        }
        
        return burntSlop;
    }

    private void CleanCooker()
    {
        S_Food[] foodList = foodScripts.ToArray();
        foreach (var foodScript in foodList)
        {
            foodScript.TurnOffGrab();
            foodScript.TurnOnColliders();
            foodScript.transform.position = new Vector3(0,-10,0);
            foodScript.TurnOnGrab();
        }
    }
}
