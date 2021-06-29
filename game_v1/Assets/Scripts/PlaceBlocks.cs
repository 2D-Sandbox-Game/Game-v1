using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using System;

public class PlaceBlocks : MonoBehaviour
{
    public InventoryObject inventory;
    InventorySlot selectedInventorySlot;

    public Tilemap tilemap;
    public float reach = 5;

    //TileBase selectedTile = null;
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
        //selectedTile = tilemap.GetTile(mousePosTranslated);

        // Sets the currently selected Inventory 
        selectedInventorySlot = inventory.Container.Items[inventory.selectedSlot];

        //Debug.Log("Inventory List:");
        //for (int i = 0; i < inventory.Container.Items.Length; i++)
        //{
        //    Debug.Log($"Slot {i}: ID: {inventory.Container.Items[i].id}");
        //}

        //Debug.Log($"Slot {inventory.selectedSlot}: ID: {inventory.Container.Items[inventory.selectedSlot].item.id}");

        if (Input.GetKey(KeyCode.Mouse0) && PlaceableObjectInSlot(selectedInventorySlot) && WithinBounds(mousePosTranslated, reach) && !BlockExists(mousePosTranslated, mapArr) && IsNextToExistingTile(mousePosTranslated) && !IsInPlayerPosition(mousePosTranslated))
        {
            PlaceBlock(selectedInventorySlot, tilemap, mousePosTranslated, mapArr);
        }
    }

    void PlaceBlock(InventorySlot selectedSlot, Tilemap tilemap, Vector3Int pos, Generation.BlockType[,] mapArr)
    {
        Tile tile = ScriptableObject.CreateInstance<Tile>();

        // Gets the sprite of the selected item from the database 
        tile.sprite = inventory.database.Items.Where(item => item.id == selectedSlot.item.id).ToList()[0].uiDisplay;
        tile.name = tile.sprite.name;

        tilemap.SetTile(pos, tile);
        selectedSlot.amount--;

        mapArr[pos.x, pos.y] = (Generation.BlockType)selectedSlot.item.id;

        if (selectedSlot.amount == 0)
        {
            // Deletes the item
            selectedSlot.id = -1;
            selectedSlot.item.id = 0;
            selectedSlot.item.name = "";
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

    bool IsInPlayerPosition(Vector3Int placePos)
    {
        Vector3Int posPlayer = tilemap.WorldToCell(gameObject.transform.position);

        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                //Debug.Log($"place: {placePos} player: {posPlayer + new Vector3Int(x, y, 0)}");

                if (placePos == posPlayer + new Vector3Int(x, y, 0))
                {
                    return true;
                }
            }
        }

        return false;
    }

    bool PlaceableObjectInSlot(InventorySlot selectedSlot)
    {
        return selectedSlot.id != -1 && selectedSlot.item.id > (int)Generation.BlockType.None && selectedSlot.item.id < (int)Generation.BlockType.Tree;
    }

    bool BlockExists(Vector3Int pos, Generation.BlockType[,] mapArr)
    {
        Generation.BlockType blockTypeAtPos = mapArr[pos.x, pos.y];

        return blockTypeAtPos > Generation.BlockType.None && blockTypeAtPos < Generation.BlockType.Cave;
    }

    bool WithinBounds(Vector3Int clickPos, float reach)
    {
        Vector3 playerPos = gameObject.transform.position;

        return (playerPos - clickPos).magnitude <= reach;
    }
}
