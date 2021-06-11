using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerSpawn : MonoBehaviour
{
    public static Vector3 playerSpawn;
    [SerializeField] Tilemap map;
    Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Transform>();
        playerSpawn = map.CellToWorld(new Vector3Int(Generation.width / 2, Generation.perlinHeight[Generation.width / 2] + 2, 0));
        player.transform.position = playerSpawn;

    }


    // Update is called once per frame
    void Update()
    {
        
    }

}
