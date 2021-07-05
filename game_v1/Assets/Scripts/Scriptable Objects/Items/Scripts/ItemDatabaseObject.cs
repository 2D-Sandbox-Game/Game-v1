using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Items/Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    // Array which is filled in the editor with the different itemsobjects that have been created
    public ItemObject[] items;
    // Allows to derive the properties from an item by the unique id
    public Dictionary<int, ItemObject> idToItem = new Dictionary<int, ItemObject>();


    // Make sure the dictionary is filled with ids and itemobjects before the game is fully started
    public void OnAfterDeserialize()
    { 
        //idToItem = new Dictionary<int, ItemObject>();

        //for (int i = 0; i < items.Length; i++)
        //{
        //    items[i].id = i;
        //    idToItem.Add(i, items[i]);
        //}
    }
    public void OnBeforeSerialize()
    {
        idToItem = new Dictionary<int, ItemObject>();

        for (int i = 0; i < items.Length; i++)
        {
            items[i].id = i;
            idToItem.Add(i, items[i]);
        }
    }
}
