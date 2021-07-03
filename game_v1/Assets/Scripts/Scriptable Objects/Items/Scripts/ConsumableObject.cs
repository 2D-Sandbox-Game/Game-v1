using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable Object", menuName = "Inventory System/Items/Consumable")]
public class ConsumableObject : ItemObject
{
    // Subtype of itemobject that the player can consume
    public void Awake()
    {
        // Corrected Type gets set on start
        type = ItemType.Consumable;
    }
}
