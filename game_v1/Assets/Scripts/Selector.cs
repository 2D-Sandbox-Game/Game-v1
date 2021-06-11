using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Selector : MonoBehaviour
{
    public Tilemap tilemap;

    TileBase selectedTile = null;
    Vector3 mousePos = Vector3.zero;
    Vector3Int mousePosTranslated = Vector3Int.zero;
    Vector3Int posSelectedTile = Vector3Int.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //World Space
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Mouse Position On tile Map
        //Converts worlds space click point to tile map click point.
        mousePosTranslated = tilemap.WorldToCell(mousePos);
        selectedTile = tilemap.GetTile(mousePosTranslated);

        if (mousePosTranslated != posSelectedTile)
        {
            GetComponent<SpriteRenderer>().color = Color.clear;
            posSelectedTile = mousePosTranslated;

            if (selectedTile != null || IsNextToExistingTile(posSelectedTile))
            {
                transform.position = new Vector3(0.5f + posSelectedTile.x, 0.5f + posSelectedTile.y);
                GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }

    bool IsNextToExistingTile(Vector3Int tilePos)
    {
        Vector3Int[] directions = { new Vector3Int(-1, 0, 0), new Vector3Int(0, 1, 0), new Vector3Int(1, 0, 0), new Vector3Int(0, -1, 0) };

        foreach (var dir in directions)
        {

            if (tilemap.GetTile(tilePos + dir) != null)
            {
                return true;
            }
        }

        return false;
    }
}
