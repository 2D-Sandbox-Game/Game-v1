using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public ItemDatabaseObject database;
    public Inventory Container;
    public void AddItem(Item item, int amount)
    {
        for (int i = 0; i < Container.Items.Count; i++)
        {
            if (Container.Items[i].item.id == item.id) // checks if item is already in the inventory
            {
                Container.Items[i].AddAmount(amount); // adds amount to existing item
                return;
            }
        }
        Container.Items.Add(new InventorySlot(item.id,item, amount));
    }
}

[System.Serializable]
public class Inventory
{
    public List<InventorySlot> Items = new List<InventorySlot>();
}
[System.Serializable]
public class InventorySlot // combines a given item in a slot with an amount
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
    public void AddAmount(int value)
    {
        amount += value;
    }
    public void SubAmount(int value)
    {
        if (amount > value)
        {
            amount -= value;
        }
        else
        {
            Debug.Log("Not enough items");
        }
    }
}
