using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class PlayerMine : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject blockBreaking;
    public GameObject pickaxe;

    public ItemDatabaseObject database;
    public GameObject createableItem;

    Animator breakingAnim;
    public float miningSpeed = 3;
    public float reach = 5;
    public float miningDuration;

    Animator pickaxeAnim;

    TileBase selectedTile = null;
    Vector3 mousePos = Vector3.zero;
    Vector3Int mousePosTranslated = Vector3Int.zero;
    Vector3Int posSelectedTile = Vector3Int.zero;
    float timeSinceMiningStart;

    Generation.BlockType[,] mapArr;
    List<GameObject> trees;

    // Start is called before the first frame update
    void Start()
    {
        breakingAnim = blockBreaking.GetComponent<Animator>();
        pickaxeAnim = pickaxe.GetComponent<Animator>();

        mapArr = Generation.perlinArr;
        trees = GenerateTrees.trees;
    }

    // Update is called once per frame
    void Update()
    {
        breakingAnim.speed = miningSpeed;
        //pickaxeAnim.speed = miningSpeed;
        miningDuration = 1 / miningSpeed;

        //World Space
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Mouse Position On tile Map
        //Converts worlds space click point to tile map click point.
        mousePosTranslated = tilemap.WorldToCell(mousePos);
        selectedTile = tilemap.GetTile(mousePosTranslated);

        if (mousePosTranslated != posSelectedTile)
        {
            timeSinceMiningStart = 0;
            breakingAnim.Play("Idle");
            pickaxeAnim.Play("Idle");
            posSelectedTile = mousePosTranslated;
        }

        if (Input.GetKey(KeyCode.Mouse0) && IsInMap(posSelectedTile) && WithinBounds(posSelectedTile, reach) && BlockExists(posSelectedTile, mapArr) && !TreeOnBlock(posSelectedTile, trees))
        {
            blockBreaking.transform.position = new Vector3(0.5f + posSelectedTile.x, 0.5f + posSelectedTile.y);
            breakingAnim.Play("BlockBreaking");
            pickaxeAnim.Play("Swinging");

            timeSinceMiningStart += Time.deltaTime;

            if (timeSinceMiningStart > miningDuration)
            {
                breakingAnim.Play("Idle");
                pickaxeAnim.Play("Idle");
                timeSinceMiningStart = 0;

                tilemap.SetTile(posSelectedTile, null);
                GenerateItem(mapArr[posSelectedTile.x, posSelectedTile.y]);
                mapArr[posSelectedTile.x, posSelectedTile.y] = Generation.BlockType.None;
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0) || !IsInMap(posSelectedTile) || (!BlockExists(posSelectedTile, mapArr) && !SaplingExists(posSelectedTile)))
        {
            breakingAnim.Play("Idle");
            pickaxeAnim.Play("Idle");
            timeSinceMiningStart = 0;
        }

    }
    void GenerateItem(Generation.BlockType blockType)
    {
        Debug.Log(blockType);

        if (blockType == Generation.BlockType.Grass)
        {
            blockType = Generation.BlockType.Dirt;
        }

        createableItem.GetComponent<GroundItem>().item = database.Items[(int)blockType];
        Instantiate(createableItem, posSelectedTile + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
    }

    //void GenerateItem(string name)
    //{
    //    //Debug.Log(name);

    //    for (int i = 0; i < database.Items.Length; i++)
    //    {
    //        if (name.Contains("dirt") || name.Contains("grass")) //just a temporary fix, need to find a way to convert the name to an ID with various names for one ID
    //        {
    //            name = "Dirt";
    //        }
    //        else if (name.Contains("stone"))
    //        {
    //            name = "Stone";
    //        }
    //        if (name == database.Items[i].name)
    //        {
    //            createableItem.GetComponent<GroundItem>().item = database.Items[i];
    //            Instantiate(createableItem, posSelectedTile + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
    //            //createableItem.GetComponent<Rigidbody2D>().AddForce(new Vector2(20, 0),ForceMode2D.Impulse); attempt to give the items an inital velocity when they spawn
    //        }
    //    }
    //}

    bool TreeOnBlock(Vector3Int pos, List<GameObject> trees)
    {
        foreach (GameObject tree in trees)
        {
            if (tree.transform.position == pos + new Vector3(0, 1, 0))
            {
                return true;
            }
        }

        return false;
    }

    bool WithinBounds(Vector3Int clickPos, float reach)
    {
        Vector3 playerPos = gameObject.transform.position;
        return (playerPos - clickPos).magnitude <= reach;
    }

    bool BlockExists(Vector3Int pos, Generation.BlockType[,] mapArr)
    {
        //Debug.Log(pos);
        Generation.BlockType blockTypeAtPos = mapArr[pos.x, pos.y];
        //Debug.Log(blockTypeAtPos);
        return blockTypeAtPos > Generation.BlockType.None && blockTypeAtPos < Generation.BlockType.Tree;
    }

    bool SaplingExists(Vector3Int pos)
    {
        foreach (GameObject tree in trees)
        {
            if (tree.name.Contains("Sapling") && tree.transform.position == pos)
            {
                return true;
            }
        }

        return false;
    }

    bool IsInMap(Vector3Int clickPos)
    {
        //Debug.Log(clickPos);
        //Debug.Log(clickPos.x >= 0 && clickPos.x < Generation.width && clickPos.y >= 0 && clickPos.y < Generation.height);
        return clickPos.x >= 0 && clickPos.x < Generation.width && clickPos.y >= 0 && clickPos.y < Generation.height;
    }
}

