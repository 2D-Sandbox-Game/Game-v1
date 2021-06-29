using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using System;

public class PlaceSapling : MonoBehaviour
{
    public GameObject player;
    public InventoryObject inventory;
    public Tilemap tilemap;
    public Sprite[] saplingSprites;
    public float reach = 5;

    InventorySlot selectedInventorySlot;

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

        if (Input.GetKey(KeyCode.Mouse0) && selectedInventorySlot.id != -1 && selectedInventorySlot.item.id == 8 && WithinBounds(mousePosTranslated, reach) && !BlockExists(mousePosTranslated, mapArr) && !BlockExists(mousePosTranslated + new Vector3Int(0, 1, 0), mapArr) && PlacedOnDirt(mousePosTranslated, mapArr))
        {
            PlaceSaplingObject();
        }
    }

    void PlaceSaplingObject()
    {
        GameObject sapling = new GameObject();
        // Gets the sprite of the selected item from the database 
        sapling.AddComponent<SpriteRenderer>().sprite = saplingSprites[0];
        sapling.name = $"Sapling, State:0, Day:{DayAndNightCycle.days}";

        sapling.transform.parent = gameObject.transform;
        sapling.transform.position = mousePosTranslated;

        GenerateTrees.trees.Add(sapling);

        mapArr[mousePosTranslated.x, mousePosTranslated.y] = Generation.BlockType.Sapling;
        mapArr[mousePosTranslated.x, mousePosTranslated.y + 1] = Generation.BlockType.Sapling;

        selectedInventorySlot.amount--;

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

    public void GrowSapling()
    {
        GameObject[] trees = new GameObject[GenerateTrees.trees.Count];
        Array.Copy(GenerateTrees.trees.ToArray(), trees, GenerateTrees.trees.Count);

        foreach (GameObject obj in trees)
        {
            //if (obj.name.Contains("Sapling"))
            //{
            //    Debug.Log(obj.name[obj.name.Length - 1] - '0');
            //}

            if (obj.name.Contains("Sapling") && obj.name[obj.name.Length - 1] - '0' < DayAndNightCycle.days)
            {
                //Debug.Log("Scuur");

                int stateIdx = obj.name.IndexOf("State:") + 6; 
                int state = obj.name[stateIdx] - '0';

                if (++state < saplingSprites.Length)
                {
                    obj.GetComponent<SpriteRenderer>().sprite = saplingSprites[state];

                    char[] name = obj.name.ToCharArray();
                    name[stateIdx] = (char)(state + '0');
                    obj.name = new string(name);
                }
                else
                {
                    Vector3 pos = obj.transform.position;

                    if (gameObject.GetComponent<GenerateTrees>().TreeCanBePlaced((int)pos.x, Generation.perlinHeight))
                    {
                        //Remove Sapling
                        mapArr[(int)pos.x, (int)pos.y] = Generation.BlockType.None;
                        mapArr[(int)pos.x, (int)pos.y + 1] = Generation.BlockType.None;
                        GenerateTrees.trees.Remove(obj);
                        Destroy(obj);

                        PlaceTree(pos);
                    }
                }
            }
        }
    }

    void PlaceTree(Vector3 pos)
    {
        GameObject tree = new GameObject();
        tree.transform.position = pos;
        tree.tag = "Tree";
        tree.name = "Tree";
        tree.transform.parent = gameObject.transform;

        gameObject.GetComponent<GenerateTrees>().CreateTreeObject(UnityEngine.Random.Range(3, 13), ref tree);

        GenerateTrees.trees.Add(tree);
    }

    bool WithinBounds(Vector3Int clickPos, float reach)
    {
        Vector3 playerPos = player.transform.position;

        return (playerPos - clickPos).magnitude <= reach;
    }
}
