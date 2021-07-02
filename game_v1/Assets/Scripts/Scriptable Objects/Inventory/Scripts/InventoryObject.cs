using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public ItemDatabaseObject database;
    public Inventory Container;
    public int selectedSlot = 0;
    public void AddItem(Item item, int amount)
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if ((Container.Items[i].id == item.id) && (item.type != ItemType.Equipment) /*&& Container.Items[i].amount < 99*/) // checks if item is already in the inventory
            {
                Container.Items[i].AddAmount(amount); // adds amount to existing item
                return;
            }
        }
        SetEmptySlot(item, amount);
    }

    public InventorySlot SetEmptySlot(Item item, int amount)
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].id < 0)
            {
                Container.Items[i].UpdateSlot(item.id, item, amount);
                return Container.Items[i];
            }
        }
        return null;
        //inventory full adjust later
    }
    public void MoveItem(InventorySlot item1, InventorySlot item2)
    {
        InventorySlot temp = new InventorySlot(item2.id, item2.item, item2.amount);
        item2.UpdateSlot(item1.id, item1.item, item1.amount);
        item1.UpdateSlot(temp.id, temp.item, temp.amount);
    }

    public void RemoveItem(Item item)
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].item.id == item.id)
            {
                Container.Items[i].UpdateSlot(-1, null, 0);
                return;
            }
        }
    }
}
[System.Serializable]
public class Inventory
{
    public InventorySlot[] Items = new InventorySlot[36];
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
    public InventorySlot()
    {
        id = -1;
        item = null;
        amount = 0;
    }
    public void UpdateSlot(int id, Item item, int amount)
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
        else if (amount == value)
        {
            //UpdateSlot(-1, null, 0);
            id = -1;
            item.id = 0;
            item.name = "";
        }
        else
        {
            Debug.Log("Not enough items");
        }
    }
}
