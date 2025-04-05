using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NaughtyAttributes;
using Extentions.Addressable;
using System.Threading.Tasks;


public enum Dish
{
    // All the different dishes players can create and costumers can order
    // add more at the BOTTOM when needed
    FishBurgerWithOnio,
    FishBurger,
    Fries,
    FriedFish,
    Burnt
}

[Serializable]
public class Recipe
{
    public Dish nameOfDish;
    public List<FoodType> ingredients;
    public CookerType canBeCookedIn;
    public GameObject resultPrefab; // The prefab to spawn
}

public class S_RecipeDatabase : MonoBehaviour
{
    public static S_RecipeDatabase Instance;
    [SerializeField, Expandable] SO_RecipeBook book;

    private async void Start()
    {
        Instance = this;
        book = await Addressable.LoadAsset<SO_RecipeBook>(Addressable.names[2]);
    }
#if UNITY_EDITOR
    private async void OnValidate()
    {
        book = await Addressable.LoadAsset<SO_RecipeBook>(Addressable.names[2]);
    }
#endif
    public static Recipe FindMatchingRecipe(List<FoodType> playerIngredients, CookerType playerCooker)
    {
        if(Instance.book == null)
        {
            Debug.Log("No recipebook in database");
        }
        foreach (var recipe in Instance.book.recipes)
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