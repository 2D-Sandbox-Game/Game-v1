using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class PlaceSapling : MonoBehaviour
{
    // Public variables
    public GameObject Player;
    public InventoryObject Inventory;
    public Tilemap Tilemap;
    public Sprite[] SaplingSprites;
    public float Reach = 5;
    
    // Private variables
    InventorySlot _selectedInventorySlot;
    TileBase _selectedTile = null;
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
        //Mouse Position On tile Map
        //Converts worlds space click point to tile map click point
        _mousePosTranslated = Tilemap.WorldToCell(_mousePos);
        _selectedTile = Tilemap.GetTile(_mousePosTranslated);

        // Sets the currently selected Inventory 
        _selectedInventorySlot = Inventory.container.items[Inventory.selectedSlot];

        // Conditions for placing a sapling are met
        if (Input.GetKey(KeyCode.Mouse0) &&
            _selectedInventorySlot.id != -1 &&
            _selectedInventorySlot.item.id == 8 && 
            WithinBounds(_mousePosTranslated, Reach) && 
            // Sapling is two blocks high -> Checks if the space is empty
            !BlockExists(_mousePosTranslated, _mapArr) && 
            !BlockExists(_mousePosTranslated + new Vector3Int(0, 1, 0), _mapArr) && 
            PlacedOnDirt(_mousePosTranslated, _mapArr))
        {
            PlaceSaplingObject();
        }
    }

    /// <summary>
    /// Creates and places the sapling game object
    /// </summary>
    void PlaceSaplingObject()
    {
        GameObject sapling = new GameObject();
        // Gets the sprite of the selected item from the database 
        sapling.AddComponent<SpriteRenderer>().sprite = SaplingSprites[0];
        sapling.name = $"Sapling, State:0, Day:{DayAndNightCycle.s_days}";

        // Sets sapling as child
        sapling.transform.parent = gameObject.transform;
        sapling.transform.position = _mousePosTranslated;

        GenerateTrees.s_trees.Add(sapling);

        // Adds sapling to the 2D world map array
        _mapArr[_mousePosTranslated.x, _mousePosTranslated.y] = Generation.BlockType.Sapling;
        _mapArr[_mousePosTranslated.x, _mousePosTranslated.y + 1] = Generation.BlockType.Sapling;

        _selectedInventorySlot.amount--;

        if (_selectedInventorySlot.amount == 0)
        {
            // Deletes the item from inventory
            _selectedInventorySlot.id = -1;
            _selectedInventorySlot.item.id = 0;
            _selectedInventorySlot.item.name = "";
        }
    }

    /// <summary>
    /// Checks if the block under the placing position is of block type "dirt" or "grass".
    /// </summary>
    /// <param name="placePos"></param>
    /// <param name="mapArr"></param>
    /// <returns></returns>
    bool PlacedOnDirt(Vector3Int placePos, Generation.BlockType[,] mapArr)
    {
        return mapArr[placePos.x, placePos.y - 1] == Generation.BlockType.Dirt || mapArr[placePos.x, placePos.y - 1] == Generation.BlockType.Grass;
    }

    /// <summary>
    /// Checks if block exists at the position.
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
    /// Checks if a sapling has grown and changes its sprite accordingly.
    /// </summary>
    public void GrowSapling()
    {
        // Copies the tree array into a new one
        GameObject[] trees = new GameObject[GenerateTrees.s_trees.Count];
        Array.Copy(GenerateTrees.s_trees.ToArray(), trees, GenerateTrees.s_trees.Count);

        foreach (GameObject obj in trees)
        {
            // Sapling state is in the game object name
            if (obj.name.Contains("Sapling") && obj.name[obj.name.Length - 1] - '0' < DayAndNightCycle.s_days)
            {
                int stateIdx = obj.name.IndexOf("State:") + 6; 
                int state = obj.name[stateIdx] - '0';

                // Sapling is still in one of the 3 grow states
                if (++state < SaplingSprites.Length)
                {
                    // Change sprite
                    obj.GetComponent<SpriteRenderer>().sprite = SaplingSprites[state];

                    // Change state in game object name
                    char[] name = obj.name.ToCharArray();
                    name[stateIdx] = (char)(state + '0');
                    obj.name = new string(name);
                }
                // Sapling has finished growing -> try to replace with tree
                else
                {
                    Vector3 pos = obj.transform.position;

                    // Checks if tree can be placed
                    if (gameObject.GetComponent<GenerateTrees>().TreeCanBePlaced((int)pos.x, Generation.s_perlinHeight))
                    {
                        // Removes sapling from 2D world map array
                        _mapArr[(int)pos.x, (int)pos.y] = Generation.BlockType.None;
                        _mapArr[(int)pos.x, (int)pos.y + 1] = Generation.BlockType.None;
                        // Remove sapling from tree array
                        GenerateTrees.s_trees.Remove(obj);
                        Destroy(obj);
                        // Place tree in the sapling's position
                        PlaceTree(pos);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Creates a tree object and places it at the postion. 
    /// </summary>
    /// <param name="pos"></param>
    void PlaceTree(Vector3 pos)
    {
        GameObject tree = new GameObject();
        tree.transform.position = pos;
        tree.tag = "Tree";
        tree.name = "Tree";
        tree.transform.parent = gameObject.transform;

        // Adds tree part objects
        gameObject.GetComponent<GenerateTrees>().CreateTreeObject(UnityEngine.Random.Range(3, 13), ref tree);
        // Ads tree to tree array
        GenerateTrees.s_trees.Add(tree);
    }

    /// <summary>
    /// Checks if the mouse position is within the player's reach.
    /// </summary>
    /// <param name="clickPos"></param>
    /// <param name="reach"></param>
    /// <returns></returns>
    bool WithinBounds(Vector3Int clickPos, float reach)
    {
        Vector3 playerPos = Player.transform.position;
        return (playerPos - clickPos).magnitude <= reach;
    }
}
