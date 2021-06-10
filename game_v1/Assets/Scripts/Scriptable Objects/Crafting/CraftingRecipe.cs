using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RecipeComponent
{
    public ItemObject item;
    [Range(1, 50)]
    public int amountNeeded;
}
[CreateAssetMenu(fileName = "New Crafting Recipe", menuName = "Crafting/Crafting Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public List<RecipeComponent> recipeComponents;
    public ItemObject craftableItem;
}
