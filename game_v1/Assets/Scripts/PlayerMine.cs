using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMine : MonoBehaviour
{
    public Tilemap tilemap;
    public Tile inputTile;
    public Animator animator;
    public GameObject blockBreaking;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        /*  Vector3 mousePos;
          TileData tileData;
          Vector3Int mousePosInt;
          GameObject block;
          Color col = Color.blue;
          MeshRenderer renderer;
          if (Input.GetKey(KeyCode.T))
          {
              mousePos = Input.mousePosition;
              mousePosInt = new Vector3Int((int)mousePos.x, (int)mousePos.y, (int)mousePos.z);
             // TileBase tile = tilemap.GetTile(mousePosInt);
              tilemap.DeleteCells(mousePosInt)
              tilemap.SetTile(mousePosInt, inputTile);
              //renderer = block.GetComponent<MeshRenderer>();
              //renderer.material.color = col;
              Debug.Log("Test");
          }

          */

        Vector3 mousePos = Vector3.zero;
        Vector3Int mousePosTranslated = Vector3Int.zero;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("Click");

            //World Space
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //Mouse Position On tile Map
            //Converts worlds space click point to tile map click point.
            mousePosTranslated = tilemap.WorldToCell(mousePos);

            //PAINTS BOXES
            //activeTileMap.BoxFill(mousePosTranslated, gameManager.cobbleTile, mousePosTranslated.x,mousePosTranslated.y,mousePosTranslated.x +1,mousePosTranslated.y +1);

            //GETS TILE NAME AND PRINTS IT
            //                Debug.Log( activeTileMap.GetTile(mousePosTranslated));
            //                Debug.Log( gameManager.objectsMap.GetTile(mousePosTranslated));
            //                Debug.Log( gameManager.bordersMap.GetTile(mousePosTranslated));


            //now we can change tiles! :slight_smile:

            TileBase clickedTile = null;
            ITilemap itilemap = null;
            TileData tileData = new TileData();


            clickedTile = tilemap.GetTile(mousePosTranslated);

            //CHANGES TILES THAT ARE CLICKED ON
            if (clickedTile == null)
                tilemap.SetTile(mousePosTranslated, inputTile);
            else
            {
                animator.SetBool("Mining", true);

                //Start the coroutine we define below named ExampleCoroutine.
                StartCoroutine(ExampleCoroutine());
                blockBreaking.transform.position = new Vector3(0.5f + mousePosTranslated.x, 0.5f + mousePosTranslated.y, 10);


            }
            // clickedTile.GetTileData(mousePosTranslated, itilemap, ref tileData);

        }
        IEnumerator ExampleCoroutine()
        {
            //Print the time of when the function is first called.
            Debug.Log("Started Coroutine at timestamp : " + Time.time);

            //yield on a new YieldInstruction that waits for 5 seconds.
            yield return new WaitForSeconds(2);

            tilemap.SetTile(mousePosTranslated, null);
            animator.SetBool("Mining", false);

            //After we have waited 5 seconds print the time again.
            Debug.Log("Finished Coroutine at timestamp : " + Time.time);
        }
    }
}
