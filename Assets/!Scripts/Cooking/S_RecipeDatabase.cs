using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using NaughtyAttributes;
using Extentions.Addressable;
using System.Threading.Tasks;

public class S_RecipeDatabase : MonoBehaviour
{
    public static S_RecipeDatabase Instance;
    [SerializeField, Expandable] SO_RecipeBook book;

    private async void Start()
    {
        Instance = this;
        book = await Addressable.LoadAsset<SO_RecipeBook>(AddressableAsset.RecipeBook);
    }
#if UNITY_EDITOR
    private async void OnValidate()
    {
        book = await Addressable.LoadAsset<SO_RecipeBook>(AddressableAsset.RecipeBook);
    }
#endif
    public static Dish FindMatchingRecipe(List<FoodType> playerIngredients, CookerType playerCooker)
    {
        if(Instance.book == null)
        {
            Debug.Log("No recipebook in database");
        }
        foreach (var recipe in Instance.book.recipes)
        {
            // First check if correct cooking station   and   then check if the recipe is correct
            if (recipe.canBeCookedIn == playerCooker && 
                recipe.ingredients.OrderBy(i => i).SequenceEqual(playerIngredients.OrderBy(i => i)))
            {
                return recipe;
            }
        }
        
        var burntDish = Instance.book.recipes.FirstOrDefault(e => e.typeOfDish == DishType.Burnt);
        return burntDish; // No match
    }
}