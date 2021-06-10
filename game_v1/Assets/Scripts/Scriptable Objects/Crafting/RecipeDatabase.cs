using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Crafting Recipe", menuName = "Crafting/Recipe Database")]

public class RecipeDatabase : ScriptableObject
{
    public List<CraftingRecipe> recipeDatabase;
}
