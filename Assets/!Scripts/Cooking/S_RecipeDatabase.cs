using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public enum Dish
{
    // All the different dishes players can create and costumers can order
    // add more at the BOTTOM when needed
    FishBurgerWithOnion,
    FishBurger,
    Fries,
    FriedFish,
    Burnt
}

[System.Serializable]
public class Recipe
{
    public Dish nameOfDish;
    public List<FoodType> ingredients;
    public CookerType canBeCookedIn;
    public GameObject resultPrefab; // The prefab to spawn
}

public class S_RecipeDatabase : MonoBehaviour
{
    [SerializeField] private List<Recipe> recipes = new List<Recipe>();
    
    public Recipe FindMatchingRecipe(List<FoodType> playerIngredients, CookerType playerCooker)
    {
        foreach (var recipe in recipes)
        {
            // First check if correct cooking station   and   then check if the recipe is correct
            if (recipe.canBeCookedIn == playerCooker && recipe.ingredients.OrderBy(i => i).SequenceEqual(playerIngredients.OrderBy(i => i)))
            {
                return recipe;
            }
        }
        return null; // No match
    }
}