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
    float miningDuration;

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
        breakingAnim.speed = miningSpeed;
        miningDuration = 1 / miningSpeed;

        mapArr = Generation.perlinArr;
        trees = GenerateTrees.trees;
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
            timeSinceMiningStart = 0;
            breakingAnim.Play("Idle");
            pickaxeAnim.Play("Idle");
            posSelectedTile = mousePosTranslated;
        }


        if (Input.GetKey(KeyCode.Mouse0) && BlockExists(posSelectedTile, mapArr) && !TreeOnBlock(posSelectedTile, trees) && WithinBounds(posSelectedTile, reach))
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

        if (Input.GetKeyUp(KeyCode.Mouse0) || !BlockExists(posSelectedTile, mapArr))
        {
            breakingAnim.Play("Idle");
            pickaxeAnim.Play("Idle");
            timeSinceMiningStart = 0;
        }


        //if (mx != prevMx && mx != 0)
        //{
        //    if (mx == -1)
        //    {
        //        pickaxe.GetComponent<SpriteRenderer>().flipX = true;
        //        pickaxe.transform.position += new Vector3(-1, 0);
        //    }
        //    else
        //    {
        //        pickaxe.GetComponent<SpriteRenderer>().flipX = false;
        //        pickaxe.transform.position += new Vector3(1, 0);
        //    }

        //    Debug.Log($"mx: {mx}");

        //    prevMx = mx;
        //}
    }
    void GenerateItem(Generation.BlockType blockType)
    {
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
        return blockTypeAtPos > Generation.BlockType.None && blockTypeAtPos < Generation.BlockType.Tree;
    }
}

