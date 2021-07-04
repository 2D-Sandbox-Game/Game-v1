using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class PlaceBlocks : MonoBehaviour
{
    public InventoryObject Inventory;
    public Tilemap Tilemap;
    public float Reach = 5;

    InventorySlot _selectedInventorySlot;
    Vector3 _mousePos = Vector3.zero;
    Vector3Int _mousePosTranslated = Vector3Int.zero;
    Generation.BlockType[,] _mapArr;

    // Start is called before the first frame update
    void Start()
    {
        _mapArr = Generation.s_perlinArr;
    }

    // Update is called once per frame
    void Update()
    {
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Converts worlds space click point to tile map click point.
        _mousePosTranslated = Tilemap.WorldToCell(_mousePos);

        // Sets the currently selected Inventory 
        _selectedInventorySlot = Inventory.Container.Items[Inventory.selectedSlot];

        // Conditions for placing a block are met
        if (Input.GetKey(KeyCode.Mouse0) && 
            PlaceableObjectInSlot(_selectedInventorySlot) && 
            WithinBounds(_mousePosTranslated, Reach) && 
            !BlockExists(_mousePosTranslated, _mapArr) && 
            IsNextToExistingTile(_mousePosTranslated) && 
            !IsInPlayerPosition(_mousePosTranslated))
        {
            PlaceBlock(_selectedInventorySlot, Tilemap, _mousePosTranslated, _mapArr);
        }
    }

    /// <summary>
    /// Sets the block in the tilemap and deletes the item in the inventory.
    /// </summary>
    /// <param name="selectedSlot"></param>
    /// <param name="tilemap"></param>
    /// <param name="pos"></param>
    /// <param name="mapArr"></param>
    void PlaceBlock(InventorySlot selectedSlot, Tilemap tilemap, Vector3Int pos, Generation.BlockType[,] mapArr)
    {
        Tile tile = ScriptableObject.CreateInstance<Tile>();

        // Gets the sprite of the selected item from the database 
        tile.sprite = Inventory.database.Items.Where(item => item.id == selectedSlot.item.id).ToList()[0].uiDisplay;
        tile.name = tile.sprite.name;

        tilemap.SetTile(pos, tile);
        selectedSlot.amount--;

        // Adds block to the 2D world map array
        mapArr[pos.x, pos.y] = (Generation.BlockType)selectedSlot.item.id;

        if (selectedSlot.amount == 0)
        {
            // Deletes the item
            selectedSlot.id = -1;
            selectedSlot.item.id = 0;
            selectedSlot.item.name = "";
        }
    }

    /// <summary>
    /// Checks if position is next to existing tile in tilemap.
    /// </summary>
    /// <param name="tilePos"></param>
    /// <returns></returns>
    bool IsNextToExistingTile(Vector3Int tilePos)
    {
        Vector3Int[] directions = { new Vector3Int(-1, 0, 0), new Vector3Int(0, 1, 0), new Vector3Int(1, 0, 0), new Vector3Int(0, -1, 0) };

        foreach (var dir in directions)
        {
            if (Tilemap.GetTile(tilePos + dir) != null)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Checks if position is within the position of the player game object.
    /// </summary>
    /// <param name="placePos"></param>
    /// <returns></returns>
    bool IsInPlayerPosition(Vector3Int placePos)
    {
        Vector3Int posPlayer = Tilemap.WorldToCell(gameObject.transform.position);

        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                if (placePos == posPlayer + new Vector3Int(x, y, 0))
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Checks if the selected item in invetory is a placable block.
    /// </summary>
    /// <param name="selectedSlot"></param>
    /// <returns></returns>
    bool PlaceableObjectInSlot(InventorySlot selectedSlot)
    {
        return selectedSlot.id != -1 && selectedSlot.item.id > (int)Generation.BlockType.None && selectedSlot.item.id < (int)Generation.BlockType.Tree;
    }

    /// <summary>
    /// Checks if a block exists in the position.
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="mapArr"></param>
    /// <returns></returns>
    bool BlockExists(Vector3Int pos, Generation.BlockType[,] mapArr)
    {
        Generation.BlockType blockTypeAtPos = mapArr[pos.x, pos.y];

        return blockTypeAtPos > Generation.BlockType.None && blockTypeAtPos < Generation.BlockType.Cave;
    }

    /// <summary>
    /// Checks if the mouse position is within the player's reach.
    /// </summary>
    /// <param name="clickPos"></param>
    /// <param name="reach"></param>
    /// <returns></returns>
    bool WithinBounds(Vector3Int clickPos, float reach)
    {
        Vector3 playerPos = gameObject.transform.position;

        return (playerPos - clickPos).magnitude <= reach;
    }
}
