using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerSpawn : MonoBehaviour
{   
    [SerializeField] Tilemap map;
    public static Rigidbody2D player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        Debug.Log(Generation.playerSpawn.y++);
        player.transform.position = Generation.playerSpawn;

    }


    // Update is called once per frame
    void Update()
    {
        
    }

}
