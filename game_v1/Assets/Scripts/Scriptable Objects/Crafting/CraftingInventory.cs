using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingInventory : MonoBehaviour
{
    public RecipeDatabase database;
    public InventoryObject playerInventory;

    // A unique inventory that is holding the items which can be crafted based upon the player inventory
    // Is used to display the items a player can craft
    public InventoryObject craftingInventory;
    public Dictionary<int, CraftingRecipe> itemsToRecipe;

    // Start is called before the first frame update
    void Start()
    {
        // Links all craftable items to their recipe in a dictionary
        itemsToRecipe = new Dictionary<int, CraftingRecipe>();
        foreach (CraftingRecipe recipe in database.recipeDatabase)
        {
            itemsToRecipe.Add(new Item(recipe.craftableItem).id, recipe);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (CraftingRecipe recipe in database.recipeDatabase)
        {
            if (CanCraft(recipe) && !IsAlreadyAdded(recipe.craftableItem))
            {
                // Adds craftable item to the craftinginventory
                // If it can be crafted does not yet exist
                craftingInventory.AddItem(new Item(recipe.craftableItem), 1);
            }
        }
    }
    private bool IsAlreadyAdded(ItemObject craftableItem)
    {
        for (int i = 0; i < craftingInventory.container.items.Length; i++)
        {
            // Checks if an item is already in the inventory
            if (craftingInventory.container.items[i].id == craftableItem.id)
            {
                return true;
            }
        }
        return false;
    }
    public bool CanCraft(CraftingRecipe recipe)
    {
        foreach (RecipeComponent component in recipe.recipeComponents)
        {
            // If all components in a recipe are in the player inventory it can be crafted
            if (!IsComponentPresent(component))
            {
                // If only one component is missing it can't be crafted
                return false;
            }
        }
        return true;
    }
    private bool IsComponentPresent(RecipeComponent component)
    {
        for (int i = 0; i < playerInventory.container.items.Length; i++)
        {
            // Checks if an Item is in the player inventory
            if (playerInventory.container.items[i].id == component.item.id)
            {
                // Checks if the required amount is in the inventory
                if (playerInventory.container.items[i].amount >= component.amountRequired)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public void CraftItem(int id)
    {
        // Get the recipe for the given id
        CraftingRecipe recipe = itemsToRecipe[id];
        // If a recipe exists
        if (recipe != null)
        {
            foreach (RecipeComponent component in recipe.recipeComponents)
            {
                for (int i = 0; i < playerInventory.container.items.Length; i++)
                {
                    if (playerInventory.container.items[i].id == component.item.id)
                    {
                        // Removes all recipecomponents from the player inventory
                        playerInventory.container.items[i].SubAmountFromSlot(component.amountRequired);
                        break;
                    }
                }
            }
            foreach (InventorySlot slot in craftingInventory.container.items)
            {
                // Removes the craftable item from the craftinginventory
                slot.SubAmountFromSlot(slot.amount);
            }
            // Adds the craftable item to the player inventory
            playerInventory.AddItem(new Item(recipe.craftableItem), 1);
        }
    }
    private void OnApplicationQuit()
    {
        // clears the inventory after the game is quit
        craftingInventory.container.items = new InventorySlot[36];
    }
}
