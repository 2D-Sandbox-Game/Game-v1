using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CutTrees : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject blockBreaking;
    //public ItemDatabaseObject database;
    //public GameObject createableItem;

    Animator breakingAnim;
    public float miningSpeed = 3;
    public float reach = 5;
    float miningDuration;

    Animator axeAnim;

    TileBase selectedTile = null;
    Vector3 mousePos = Vector3.zero;
    Vector3Int mousePosTranslated = Vector3Int.zero;
    Vector3Int posSelectedTile = Vector3Int.zero;
    float timeSinceMiningStart;

    List<GameObject> trees;

    // Start is called before the first frame update
    void Start()
    {
        breakingAnim = blockBreaking.GetComponent<Animator>();
        axeAnim = gameObject.GetComponent<Animator>();
        breakingAnim.speed = miningSpeed;
        miningDuration = 1 / miningSpeed;

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
            posSelectedTile = mousePosTranslated;
        }

        if (TreeExists(posSelectedTile) && WithinBounds(posSelectedTile, reach))
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                blockBreaking.transform.position = new Vector3(0.5f + posSelectedTile.x, 0.5f + posSelectedTile.y);
                breakingAnim.Play("BlockBreaking");
                axeAnim.Play("AxeSwinging");

                timeSinceMiningStart += Time.deltaTime;

                if (timeSinceMiningStart > miningDuration)
                {
                    Debug.Log("aaa");
                    breakingAnim.Play("Idle");
                    axeAnim.Play("Idle");
                    DeleteTree(posSelectedTile);

                    foreach (GameObject tree in trees)
                    {
                        Debug.Log(tree.name);
                    }

                    timeSinceMiningStart = 0;
                }

            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0) || !TreeExists(posSelectedTile))
        {
            breakingAnim.Play("Idle");
            axeAnim.Play("Idle");
            timeSinceMiningStart = 0;
        }
        

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

    bool TreeExists(Vector3Int pos)
    {
        foreach (GameObject tree in trees)
        {
            if (tree.transform.position == pos)
            {
                return true;
            }
        }

        return false;
    }

    void DeleteTree(Vector3Int pos)
    {
        foreach (GameObject tree in trees)
        {
            if (tree.transform.position == pos)
            {
                trees.Remove(tree);

                //foreach (gameobject treepart in tree)
                //{
                //    generation.perlinarr[(int)treepart.transform.position.x, (int)treepart.transform.position.y] = 0;
                //}

                //for (int i = 0; i < length; i++)
                //{
                //    gameobject child = tree.transform.GetChild(;
                //}

                Destroy(tree);
                return;
            }
        }
    }

    bool WithinBounds(Vector3Int clickPos, float reach)
    {
        Vector3 playerPos = gameObject.transform.parent.parent.transform.position;

        return (playerPos - clickPos).magnitude <= reach;
    }
}
