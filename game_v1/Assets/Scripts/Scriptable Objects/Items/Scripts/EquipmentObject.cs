using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentObject : ItemObject
{
    private void Awake()
    {
        type = ItemType.Equipment;
    }
}
