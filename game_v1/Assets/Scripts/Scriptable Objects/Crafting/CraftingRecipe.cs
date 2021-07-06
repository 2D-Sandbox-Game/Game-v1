using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RecipeComponent
{
    // Hold the single components that make up a recipe
    public ItemObject item;
    [Range(1, 50)]
    public int amountRequired;
}
[CreateAssetMenu(fileName = "New Crafting Recipe", menuName = "Crafting/Crafting Recipe")]
public class CraftingRecipe : ScriptableObject
{
    // Hold the items that are needed to craft the craftableItem
    public List<RecipeComponent> recipeComponents;
    public ItemObject craftableItem;
}
