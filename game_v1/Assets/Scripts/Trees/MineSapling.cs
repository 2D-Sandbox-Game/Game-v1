using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MineSapling : MonoBehaviour
{
    public GameObject player;
    public Tilemap tilemap;
    public GameObject blockBreaking;
    public GameObject pickaxe;

    public ItemDatabaseObject database;
    public GameObject droppedAcorns;

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


        if (Input.GetKey(KeyCode.Mouse0) && SaplingExists(posSelectedTile) && WithinBounds(posSelectedTile, reach))
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

                mapArr[posSelectedTile.x, posSelectedTile.y] = Generation.BlockType.None;
                mapArr[posSelectedTile.x, posSelectedTile.y + 1] = Generation.BlockType.None;
                DeleteSapling(posSelectedTile);
            }
        }

        //if (Input.GetKeyUp(KeyCode.Mouse0) || !SaplingExists(posSelectedTile))
        //{
        //    breakingAnim.Play("Idle");
        //    pickaxeAnim.Play("Idle");
        //    timeSinceMiningStart = 0;
        //}
    }

    void GenerateItem()
    {
        droppedAcorns.GetComponent<GroundItem>().item = database.items[8];
        Instantiate(droppedAcorns, posSelectedTile + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
    }

    bool WithinBounds(Vector3Int clickPos, float reach)
    {
        Vector3 playerPos = player.transform.position;
        return (playerPos - clickPos).magnitude <= reach;
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

    void DeleteSapling(Vector3Int pos)
    {
        foreach (GameObject tree in trees)
        {
            if (tree.transform.position == pos)
            {
                GenerateItem();
                trees.Remove(tree);
                Destroy(tree);
                return;
            }
        }
    }
}
