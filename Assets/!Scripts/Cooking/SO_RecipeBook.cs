using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_RecipeBook", menuName = "Food/SO_RecipeBook")]
public class SO_RecipeBook : ScriptableObject
{
    public List<Dish> recipes = new List<Dish>();
}

public enum DishType
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
public class Dish
{
    public DishType typeOfDish;
    public List<FoodType> ingredients;
    public CookerType canBeCookedIn;
    [Required]
    public GameObject resultPrefab; // The prefab to spawn

    [Space]

    public float underCookedTime = 10;
    public float perfectlyCookedTime = 20;
    public float overCookedTime = 30;
}
