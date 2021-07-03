using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Default Object", menuName = "Inventory System/Items/Default")]
public class DefaultObject : ItemObject
{
    // Subtype of itemobject that consists nonspecific items like items which are only used to craft other items
    public void Awake()
    {
        // Corrected Type gets set on start
        type = ItemType.Default;
    }
}
