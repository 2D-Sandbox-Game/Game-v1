using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Contains all implemented item types
public enum ItemType
{
    Consumable,
    Equipment,
    Block,
    Default
}

// Items that exist in the game as an object
public abstract class ItemObject : ScriptableObject
{
    public int id;
    public Sprite uiDisplay; // holds display for the item
    public ItemType type;
    public float attribute;
}

[System.Serializable]
// Items that exist in the inventory
public class Item
{
    public string name;
    public int id;
    public ItemType type;
    public float attribute;
    public Sprite uiDisplay;
    public Item(ItemObject itemObject)
    {
        // transfers all the data from an itemobject to an item
        name = itemObject.name;
        id = itemObject.id;
        type = itemObject.type;
        attribute = itemObject.attribute;
        uiDisplay = itemObject.uiDisplay;
    }
}