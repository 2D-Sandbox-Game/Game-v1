using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingInventory : MonoBehaviour
{
    public RecipeDatabase database;
    public InventoryObject playerInventory;
    public InventoryObject craftingInventory;
    Dictionary<int, CraftingRecipe> itemsToRecipe;
    // Start is called before the first frame update
    void Start()
    {
        itemsToRecipe = new Dictionary<int, CraftingRecipe>();
        foreach (CraftingRecipe recipe in database.recipeDatabase)
        {
            itemsToRecipe.Add(new Item(recipe.craftableItem).id, recipe);
        }
    }

    private void OnEnable()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        foreach (CraftingRecipe recipe in database.recipeDatabase)
        {
            if (CanCraft(recipe) && !AlreadyDisplayed(recipe.craftableItem))
            {
               craftingInventory.AddItem(new Item(recipe.craftableItem), 1);
                Debug.Log("Item added");
            }
        }
    }
    private bool AlreadyDisplayed(ItemObject craftableItem)
    {
        for (int i = 0; i < craftingInventory.Container.Items.Length; i++)
        {
            if (craftingInventory.Container.Items[i].id == craftableItem.id)
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
            if (!ComponentPresent(component))
            {
                return false;
            }
        }
        return true;
    }
    private bool ComponentPresent(RecipeComponent component)
    {
        for (int i = 0; i < playerInventory.Container.Items.Length; i++)
        {
            if (playerInventory.Container.Items[i].id == component.item.id)
            {
                if (playerInventory.Container.Items[i].amount >= component.amountNeeded)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public CraftingRecipe ItemsToRecipe(int id)
    {
        return itemsToRecipe[id];
    }
    public void Craft(int id)
    {
        CraftingRecipe recipe = itemsToRecipe[id];
        if (recipe != null)
        {
            foreach (RecipeComponent component in recipe.recipeComponents)
            {
                for (int i = 0; i < playerInventory.Container.Items.Length; i++)
                {
                    if (playerInventory.Container.Items[i].id == component.item.id)
                    {
                        playerInventory.Container.Items[i].SubAmount(component.amountNeeded);                        
                        break;
                    }
                }
            }
            foreach (InventorySlot slot in craftingInventory.Container.Items)
            {
                slot.SubAmount(slot.amount);
            }
            playerInventory.AddItem(new Item(recipe.craftableItem), 1);
        }
    }
    private void OnApplicationQuit() // clears the inventory after the game is quit
    {
        craftingInventory.Container.Items = new InventorySlot[36];
    }
}
