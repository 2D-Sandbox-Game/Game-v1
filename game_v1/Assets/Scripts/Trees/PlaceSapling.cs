using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class PlaceSapling : MonoBehaviour
{
    public InventoryObject inventory;
    InventorySlot selectedInventorySlot;

    public Tilemap tilemap;
    public Sprite saplingSprite;

    TileBase selectedTile = null;
    Vector3 mousePos = Vector3.zero;
    Vector3Int mousePosTranslated = Vector3Int.zero;

    Generation.BlockType[,] mapArr;

    // Start is called before the first frame update
    void Start()
    {
        mapArr = Generation.perlinArr;
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Mouse Position On tile Map
        //Converts worlds space click point to tile map click point.
        mousePosTranslated = tilemap.WorldToCell(mousePos);
        selectedTile = tilemap.GetTile(mousePosTranslated);

        // Sets the currently selected Inventory 
        selectedInventorySlot = inventory.Container.Items[inventory.selectedSlot];


        if (Input.GetKey(KeyCode.Mouse0) && selectedInventorySlot.id != -1 && selectedInventorySlot.item.id == 8 && !BlockExists(mousePosTranslated, mapArr) && !BlockExists(mousePosTranslated + new Vector3Int(0, 1, 0), mapArr) && PlacedOnDirt(mousePosTranslated, mapArr))
        {
            PlaceSaplingObject();
        }
    }

    void PlaceSaplingObject()
    {
        GameObject sapling = new GameObject(); ;
        // Gets the sprite of the selected item from the database 
        sapling.AddComponent<SpriteRenderer>().sprite = saplingSprite;
        sapling.name = "Sapling";

        sapling.transform.parent = gameObject.transform;
        sapling.transform.position = mousePosTranslated;
        selectedInventorySlot.amount--;

        mapArr[mousePosTranslated.x, mousePosTranslated.y] = Generation.BlockType.Sapling;

        if (selectedInventorySlot.amount == 0)
        {
            // Deletes the item
            selectedInventorySlot.id = -1;
            selectedInventorySlot.item.id = 0;
            selectedInventorySlot.item.name = "";
        }
    }

    bool PlacedOnDirt(Vector3Int placePos, Generation.BlockType[,] mapArr)
    {
        return mapArr[placePos.x, placePos.y - 1] == Generation.BlockType.Dirt || mapArr[placePos.x, placePos.y - 1] == Generation.BlockType.Grass;
    }

    bool BlockExists(Vector3Int pos, Generation.BlockType[,] mapArr)
    {
        Generation.BlockType blockTypeAtPos = mapArr[pos.x, pos.y];

        return blockTypeAtPos > Generation.BlockType.None && blockTypeAtPos < Generation.BlockType.Cave;
    }
}
