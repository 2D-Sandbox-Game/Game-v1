using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerSpawn : MonoBehaviour
{
    public static Vector3 s_playerSpawn;
    [SerializeField] Tilemap _map;
    Transform _player;
    bool _playerSpawned = false;

    // Start is called before the first frame update
    // Calculation of spawn point of player 
    void Start()
    {
        _player = GetComponent<Transform>();
        s_playerSpawn = _map.CellToWorld(new Vector3Int(Generation.s_width / 2, Generation.s_perlinHeight[Generation.s_width / 2] + 2, 0));
    }
    // Update is called once per frame
    // Player spawn if dead
    void Update()
    {
        if (!_playerSpawned)
        {
            _player.position = s_playerSpawn;
            _playerSpawned = true;
        }
    }
    // Function updates the spawn point
    public void SpawnPlayer(int[] perlinHeight)
    {
        _player = GetComponent<Transform>();
        s_playerSpawn = _map.CellToWorld(new Vector3Int(Generation.s_width / 2, perlinHeight[Generation.s_width / 2] + 2, 0));
        _player.transform.position = s_playerSpawn;
        _player.GetComponent<SpriteRenderer>().color = Color.blue;
    }

}
