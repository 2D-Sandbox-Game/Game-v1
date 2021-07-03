using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class CutTrees : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject blockBreaking;
    //public ItemDatabaseObject database;
    //public GameObject createableItem;
    public ItemDatabaseObject database;
    public GameObject droppedWood;
    public GameObject droppedAcorns;

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

    Generation.BlockType[,] mapArr;
    List<GameObject> trees;

    // Start is called before the first frame update
    void Start()
    {
        breakingAnim = blockBreaking.GetComponent<Animator>();
        axeAnim = gameObject.GetComponent<Animator>();

        mapArr = Generation.perlinArr;
        trees = GenerateTrees.trees;
    }

    // Update is called once per frame
    void Update()
    {
        breakingAnim.speed = miningSpeed;
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
            axeAnim.Play("Idle");
            posSelectedTile = mousePosTranslated;
        }

        if (Input.GetKey(KeyCode.Mouse0) && TreeExists(posSelectedTile) && WithinBounds(posSelectedTile, reach))
        {
            blockBreaking.transform.position = new Vector3(0.5f + posSelectedTile.x, 0.5f + posSelectedTile.y);
            breakingAnim.Play("BlockBreaking");
            axeAnim.Play("AxeSwinging");

            timeSinceMiningStart += Time.deltaTime;

            if (timeSinceMiningStart > miningDuration)
            {
                breakingAnim.Play("Idle");
                axeAnim.Play("Idle");
                timeSinceMiningStart = 0;

                DeleteTree(posSelectedTile);
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0) || !TreeExists(posSelectedTile))
        {
            breakingAnim.Play("Idle");
            axeAnim.Play("Idle");
            timeSinceMiningStart = 0;
        }
    }

    void GenerateItem(GameObject tree)
    {
        int amountWood = tree.GetComponentsInChildren<Transform>().Length * 4;

        droppedWood.GetComponent<GroundItem>().item = database.items[9];
        for (int i = 0; i < amountWood; i++)
        {
            Instantiate(droppedWood, posSelectedTile + new Vector3(0.4f, 0.5f, 0), Quaternion.identity);
        }

        droppedAcorns.GetComponent<GroundItem>().item = database.items[8];
        for (int i = 0; i < Random.Range(1, 3); i++)
        {
            Instantiate(droppedAcorns, posSelectedTile + new Vector3(0.6f, 0.5f, 0), Quaternion.identity);
        }
    }

    bool TreeExists(Vector3Int pos)
    {
        foreach (GameObject tree in trees)
        {
            if (tree.name.Contains("Tree") && tree.transform.position == pos)
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
                GenerateItem(tree);
                RemoveTreeFromMap(tree, pos, mapArr);
                trees.Remove(tree);
                Destroy(tree);
                return;
            }
        }
    }

    void RemoveTreeFromMap(GameObject tree, Vector3Int pos, Generation.BlockType[,] mapArr)
    {
        int trunkLength = tree.GetComponentsInChildren<Transform>().Length - 3;
        //Debug.Log(trunkLength);

        for (int i = 0; i <= trunkLength; i++)
        {
            mapArr[pos.x, pos.y + i] = Generation.BlockType.None;
        }

        for (int i = pos.x - 1; i < pos.x + 2; i++)
        {
            for (int j = pos.y + trunkLength + 1; j < pos.y + trunkLength + 4; j++)
            {
                if (i >= 0 && i < mapArr.GetLength(0) && j >= 0 && j < mapArr.GetLength(1))
                {
                    mapArr[i, j] = Generation.BlockType.None;
                }
            }
        }
    }

    bool WithinBounds(Vector3Int clickPos, float reach)
    {
        Vector3 playerPos = gameObject.transform.parent.parent.transform.position;

        return (playerPos - clickPos).magnitude <= reach;
    }
}
