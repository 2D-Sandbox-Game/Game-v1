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

    TileBase selectedTile = null;
    Vector3 mousePos = Vector3.zero;
    Vector3Int mousePosTranslated = Vector3Int.zero;

    int[,] mapArr;

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


        if (Input.GetKey(KeyCode.Mouse0) && 
            selectedInventorySlot.id != -1 &&
            selectedInventorySlot.item.name.ToLower().Contains("sapling") &&
            mapArr[mousePosTranslated.x, mousePosTranslated.y] == (int)Generation.BlockType.None &&
            PlacedOnDirt(mousePosTranslated, mapArr) &&
            mapArr[mousePosTranslated.x,mousePosTranslated.y + 1] == 0)
        {
            PlaceSaplingObject();
        }
    }

    void PlaceSaplingObject()
    {
        GameObject sapling = new GameObject(); ;
        // Gets the sprite of the selected item from the database 
        sapling.AddComponent<SpriteRenderer>().sprite = inventory.database.Items.Where(item => item.id == selectedInventorySlot.item.id).ToList()[0].uiDisplay;
        sapling.name = "Sapling";

        sapling.transform.parent = gameObject.transform;
        sapling.transform.position = mousePosTranslated;
        selectedInventorySlot.amount--;

        mapArr[mousePosTranslated.x, mousePosTranslated.y] = (int)Generation.BlockType.Sapling;

        if (selectedInventorySlot.amount == 0)
        {
            // Deletes the item
            selectedInventorySlot.id = -1;
            selectedInventorySlot.item.id = 0;
            selectedInventorySlot.item.name = "";
        }
    }

    bool PlacedOnDirt(Vector3Int placePos, int[,] mapArr)
    {
        return mapArr[placePos.x, placePos.y - 1] == (int)Generation.BlockType.Dirt || mapArr[placePos.x, placePos.y - 1] == (int)Generation.BlockType.Grass;
    }
}
