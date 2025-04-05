using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_RecipeBook", menuName = "Food/SO_RecipeBook")]
public class SO_RecipeBook : ScriptableObject
{
    public List<Recipe> recipes = new List<Recipe>();
}
