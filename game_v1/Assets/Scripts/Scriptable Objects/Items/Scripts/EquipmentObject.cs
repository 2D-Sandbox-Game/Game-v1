using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment Object", menuName = "Inventory System/Items/Equipment")]
public class EquipmentObject : ItemObject
{
    // Subtype of itemobject that consists the tools the player can use to interact with the world
    private void Awake()
    {
        // Corrected Type gets set on start
        type = ItemType.Equipment;
    }
}
