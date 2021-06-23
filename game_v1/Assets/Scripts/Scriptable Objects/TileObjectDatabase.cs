using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New TileObject Database", menuName = "Tiles/TileObjectDataBase")]
public class TileObjectDatabase : ScriptableObject, ISerializationCallbackReceiver
{
    public TileObject[] tileObjects;
    public Dictionary<Tile, int> GetTileId;

    public void OnAfterDeserialize()
    {
        for (int i = 0; i < tileObjects.Length; i++)
        {
            GetTileId.Add(tileObjects[i].tile, tileObjects[i].itemId);
        }
    }

    public void OnBeforeSerialize()
    {
        GetTileId = new Dictionary<Tile, int>();
    }
}
