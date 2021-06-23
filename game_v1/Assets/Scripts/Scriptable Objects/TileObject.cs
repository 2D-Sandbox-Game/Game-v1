using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New TileObject", menuName = "Tiles/TileObject")]
public class TileObject : ScriptableObject
{
    public Tile tile;
    public int itemId;
}
