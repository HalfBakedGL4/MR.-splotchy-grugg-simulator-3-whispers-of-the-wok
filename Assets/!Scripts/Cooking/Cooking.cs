using System;
using System.Collections.Generic;
using UnityEngine;

public enum CookerType
{
    Oven,
    Fryer,
}
public class Cooking : MonoBehaviour
{
    private enum CookerState
    {
        Available,
        Cooking,
        Finished,
    }
    

    [SerializeField] private CookerType cookerType;
    [SerializeField] private Transform exitTransform;
    [SerializeField] private GameObject burntSlop;
    
    [Header("Cooking Timers")]
    [SerializeField] private float goodTime;
    [SerializeField] private float badTime;
    [SerializeField] private float worstTime;
    
    private CookerState state = CookerState.Available;

    private RecipeDatabase recipeDatabase;

    private void Awake()
    {
        // Find the RecipeDatabase in the scene
        recipeDatabase = FindObjectOfType<RecipeDatabase>();

        if (recipeDatabase == null)
        {
            Debug.LogError("RecipeBook not found in the scene!");
        }
    }

    private List<FoodType> foodCooking = new ();
    private float timer;
    void Update()
    {
        switch (state)
        {
            case CookerState.Available:
                print("Available");
                break; 
            case CookerState.Cooking:
                timer += Time.deltaTime;
                if (timer <= 1)
                {
                    InteractWithCooker();
                    state = CookerState.Finished;
                }
                break;
            case CookerState.Finished:
                
                    state = CookerState.Available;
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Adds food to the Cooker
        if (other.TryGetComponent(out Food foodScript) && state == CookerState.Available)
        {
            //Places food in cooker
            foodScript.TurnOffPhysics();
            other.transform.parent = this.transform;
            other.transform.localPosition = new Vector3(0, 0, 0);
            
            //Saving info about the food in cooker
            foodCooking.Add(foodScript.GetFoodType());
            //Changes state to cooking
            state = CookerState.Cooking;
            print("Cooking");
        }
    }

    public void InteractWithCooker()
    {
        // Activate Cooker and start timer
        if (state == CookerState.Available)
        {
            timer = 0.0f;

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

            Instantiate(dishToSpawn, exitTransform.position, exitTransform.rotation);

        }
    }

    private GameObject GetDish()
    {
        var dishInfo = recipeDatabase.FindMatchingRecipe(foodCooking, cookerType);
        
        if (dishInfo != null)
        {
            print(dishInfo.name);
            return dishInfo.resultPrefab;
        }
        
        return burntSlop;
    }
}
