using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Consumable,
    Equipment,
    Block,
    Default
}
public abstract class ItemObject : ScriptableObject
{
    public int id;
    public Sprite uiDisplay; // holds display for the item
    public ItemType type;
}
[System.Serializable]
public class Item
{
    public string name;
    public int id;
    public ItemType type;
    public Item(ItemObject itemObject)
    {
        name = itemObject.name;
        id = itemObject.id;
        type = itemObject.type;
    }
}