using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_RecipeBook", menuName = "Food/SO_RecipeBook")]
public class SO_RecipeBook : ScriptableObject
{
    public List<Recipe> recipes = new List<Recipe>();
}

public enum Dish
{
    // All the different dishes players can create and costumers can order
    // add more at the BOTTOM when needed
    None = 0,
    FriedOnion = 1,
    FishBurger = 2,
    FriedFish = 4,
    Burnt = 5
}

[Serializable]
public class Recipe
{
    public Dish nameOfDish;
    public List<FoodType> ingredients;
    public CookerType canBeCookedIn;
    public GameObject resultPrefab; // The prefab to spawn
}
