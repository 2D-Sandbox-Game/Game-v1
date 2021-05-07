using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class PlyerMine2 : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject blockBreaking;
    public GameObject pickaxe;
    public GameObject selector;

    Animator breakingAnim;
    public float miningSpeed = 3;
    float miningDuration;

    Animator pickaxeAnim;

    TileBase selectedTile = null;
    Vector3 mousePos = Vector3.zero;
    Vector3Int mousePosTranslated = Vector3Int.zero;
    Vector3Int posSelectedTile = Vector3Int.zero;
    float timeSinceMiningStart;

    float mx;
    float prevMx = 1;

    // Start is called before the first frame update
    void Start()
    {
        breakingAnim = blockBreaking.GetComponent<Animator>();
        pickaxeAnim = pickaxe.GetComponent<Animator>();
        breakingAnim.speed = miningSpeed;
        miningDuration = 1 / miningSpeed;
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

            selector.GetComponent<SpriteRenderer>().color = Color.clear;
            posSelectedTile = mousePosTranslated;

            if (selectedTile != null)
            {
                selector.transform.position = new Vector3(0.5f + posSelectedTile.x, 0.5f + posSelectedTile.y);
                selector.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }

        if (selectedTile != null)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                blockBreaking.transform.position = new Vector3(0.5f + posSelectedTile.x, 0.5f + posSelectedTile.y);
                breakingAnim.Play("BlockBreaking");
                pickaxeAnim.Play("Swinging");

                timeSinceMiningStart += Time.deltaTime;

                if (timeSinceMiningStart > miningDuration)
                {
                    breakingAnim.Play("Idle");
                    pickaxeAnim.Play("Idle");
                    tilemap.SetTile(posSelectedTile, null);
                    timeSinceMiningStart = 0;
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0) || selectedTile == null)
        {
            breakingAnim.Play("Idle");
            pickaxeAnim.Play("Idle");
            timeSinceMiningStart = 0;
        }

        mx = Input.GetAxisRaw("Horizontal");

        if (mx != prevMx && mx != 0)
        {
            if (mx == -1)
            {
                pickaxe.GetComponent<SpriteRenderer>().flipX = true;
                pickaxe.transform.position += new Vector3(-1, 0);
            }
            else
            {
                pickaxe.GetComponent<SpriteRenderer>().flipX = false;
                pickaxe.transform.position += new Vector3(1, 0);
            }

            Debug.Log($"mx: {mx}");

            prevMx = mx;
        }
    }
}

