using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public ItemDatabaseObject database;
    public Inventory container;
    public int selectedSlot = 0;

    public void AddItem(Item item, int amount)
    {
        for (int i = 0; i < container.items.Length; i++)
        {
            // Checks if item is already in the inventory
            if ((container.items[i].id == item.id) && (item.type != ItemType.Equipment)) 
            {
                // Adds amount to existing item
                container.items[i].AddAmountToSlot(amount); 
                return;
            }
        }
        // If the item does not yet exist create a new inventoryslot
        CreateEmptySlot(item, amount);
    }

    public InventorySlot CreateEmptySlot(Item item, int amount)
    {
        for (int i = 0; i < container.items.Length; i++)
        {
            // Finds first empty slot in inventory
            if (container.items[i].id < 0)
            {
                // Adds the Item to the frist empty inventoryslot
                container.items[i].UpdateSlot(item.id, item, amount);
                return container.items[i];
            }
        }
        return null;
    }

    public void MoveItem(InventorySlot item1, InventorySlot item2)
    {
        // Swaps to inventoryslots
        InventorySlot temp = new InventorySlot(item2.id, item2.item, item2.amount);
        item2.UpdateSlot(item1.id, item1.item, item1.amount);
        item1.UpdateSlot(temp.id, temp.item, temp.amount);
    }

    public void RemoveItem(Item item)
    {
        for (int i = 0; i < container.items.Length; i++)
        {
            // Finds item in inventory resets the slot to be empty
            if (container.items[i].item.id == item.id)
            {
                container.items[i].UpdateSlot(-1, null, 0);
                return;
            }
        }
    }
}

[System.Serializable]
public class Inventory
{
    public InventorySlot[] items = new InventorySlot[36];
}
[System.Serializable]

// combines a given item in a slot with an amount
public class InventorySlot 
{
    public int id;
    public Item item;
    public int amount;
    public InventorySlot(int id, Item item, int amount)
    {
        this.id = id;
        this.item = item;
        this.amount = amount;
    }

    public InventorySlot()
    {
        // Default values for an empty slot
        id = -1;
        item = null;
        amount = 0;
    }

    public void UpdateSlot(int id, Item item, int amount)
    {
        // Updates a slot an sets all variables anew
        this.id = id;
        this.item = item;
        this.amount = amount;
    }

    public void AddAmountToSlot(int value)
    {
        // Adds a given value to slot
        amount += value;
    }

    public void SubAmountFromSlot(int value)
    {
        if (amount > value)
        {
            // Subtracts value from slot if it is less than the existing amount
            amount -= value;
        }
        else if (amount == value)
        {
            // If value and amount are the same the slot gets reset to default
            id = -1;
            item.id = 0;
            item.name = "";
        }
    }
}
