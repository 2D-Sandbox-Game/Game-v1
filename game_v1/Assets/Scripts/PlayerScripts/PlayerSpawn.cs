using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerSpawn : MonoBehaviour
{
    public static Vector3 playerSpawn;
    [SerializeField] Tilemap map;
    Transform player;
    bool playerSpawned = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Transform>();
        playerSpawn = map.CellToWorld(new Vector3Int(Generation.s_width / 2, Generation.s_perlinHeight[Generation.s_width / 2] + 2, 0));
    }




    // Update is called once per frame
    void Update()
    {
        if (!playerSpawned)
        {
            player.position = playerSpawn;
            playerSpawned = true;
        }
    }

    public void Spawn(int[] perlinHeight)
    {
        player = GetComponent<Transform>();
        playerSpawn = map.CellToWorld(new Vector3Int(Generation.s_width / 2, perlinHeight[Generation.s_width / 2] + 2, 0));
        player.transform.position = playerSpawn;
        player.GetComponent<SpriteRenderer>().color = Color.blue;
        Debug.Log("Miaaaaaau");
    }

}
