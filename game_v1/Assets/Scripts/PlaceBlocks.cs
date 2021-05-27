using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class PlaceBlocks : MonoBehaviour
{
    public InventoryObject inventory;
    InventorySlot selectedInventorySlot;

    public Tilemap tilemap;

    TileBase selectedTile = null;
    Vector3 mousePos = Vector3.zero;
    Vector3Int mousePosTranslated = Vector3Int.zero;

    // Start is called before the first frame update
    void Start()
    {

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

        //Debug.Log("Inventory List:");
        //for (int i = 0; i < inventory.Container.Items.Length; i++)
        //{
        //    Debug.Log($"Slot {i}: ID: {inventory.Container.Items[i].id}");
        //}

        //Debug.Log($"Slot {inventory.selectedSlot}: ID: {inventory.Container.Items[inventory.selectedSlot].item.id}");

        if (selectedInventorySlot.id != -1 && inventory.database.Items.Where(item => item.id == selectedInventorySlot.item.id).ToList()[0].type == ItemType.Block && selectedTile == null && Input.GetKey(KeyCode.Mouse0) && IsNextToExistingTile(mousePosTranslated))
        {
            PlaceBlock();
        }
    }

    void PlaceBlock()
    {
        Tile tile = ScriptableObject.CreateInstance<Tile>();

        // Gets the sprite of the selected item from the database 
        tile.sprite = inventory.database.Items.Where(item => item.id == selectedInventorySlot.item.id).ToList()[0].uiDisplay;
        tile.name = tile.sprite.name;

        tilemap.SetTile(mousePosTranslated, tile);
        selectedInventorySlot.amount--;

        if (selectedInventorySlot.amount == 0)
        {
            // Deletes the item
            selectedInventorySlot.id = -1;
            selectedInventorySlot.item.id = 0;
            selectedInventorySlot.item.name = "";
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
