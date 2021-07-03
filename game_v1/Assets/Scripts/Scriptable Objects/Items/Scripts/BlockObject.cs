using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Block Object", menuName = "Inventory System/Items/Block")]
public class BlockObject : ItemObject
{
    // Subtype of itemobject that contains all the blocks which make up the ingame world
    public void Awake()
    {
        // Corrected Type gets set on start
        type = ItemType.Block;
    }
}
