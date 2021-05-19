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

            if (selectedTile != null)
            {
                transform.position = new Vector3(0.5f + posSelectedTile.x, 0.5f + posSelectedTile.y);
                GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }
}
