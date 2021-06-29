using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Reflection;
using UnityEngine.Experimental.Rendering.Universal;

public class GenerationFG : Generation
{
    //int[,] perlinArr;
    Tilemap tilemapFG;
    Tile[] tiles = new Tile[6];

    public Tile dirt;
    public Tile stone;
    public Tile grass;
    public Tile copper;
    public Tile iron;
    public Tile gold;

    void Start()
    {
        tiles[0] = dirt;
        tiles[1] = stone;
        tiles[2] = grass;
        tiles[3] = copper;
        tiles[4] = iron;
        tiles[5] = gold;


        //perlinArr = Generation.perlinArr;
        tilemapFG = GetComponent<Tilemap>();
        GenerateForeground(perlinArr, tiles);

        //playerSpawn = SpawnPoint(new Vector3Int(0, 0, 0));
    }

    void GenerateForeground(BlockType[,] perlinArr, Tile[] tiles)
    {
        for (int x = 0; x < perlinArr.GetLength(0); x++)
        {
            for (int y = 0; y < perlinArr.GetLength(1); y++)
            {
                if(perlinArr[x, y] > BlockType.None && perlinArr[x, y] < BlockType.Wood)
                {
                    tilemapFG.SetTile(new Vector3Int(x, y, 0), tiles[(int)perlinArr[x, y] - 1]);
                }
            }
        }
    }

    //public Vector3 SpawnPoint(Vector3Int x)
    //{
    //    if (tilemapFG.GetTile(x) == null)
    //    {
    //        while (tilemapFG.GetTile(x) == null) // move down
    //        {
    //            Debug.Log(x);
    //            x.y--;
    //        }
    //    }
    //    else
    //    {
    //        while (tilemapFG.GetTile(x) != null) // move up
    //        {
    //            Debug.Log(x);
    //            x.y++;
    //        }
    //    }
    //    return tilemapFG.CellToWorld(x); ;
    //}
}
