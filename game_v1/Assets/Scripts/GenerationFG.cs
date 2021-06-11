using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerationFG : MonoBehaviour
{
    int[,] perlinArr;
    Tilemap tilemapFG;
    Tile[] tiles = new Tile[3];

    public Tile dirt;
    public Tile stone;
    public Tile grass;

    void Start()
    {
        tiles[0] = dirt;
        tiles[1] = stone;
        tiles[2] = grass;

        perlinArr = Generation.perlinArr;
        tilemapFG = GetComponent<Tilemap>();
        GenerateForeground(perlinArr, tiles);
    }

    void GenerateForeground(int[,] perlinArr, Tile[] tiles)
    {
        for (int x = 0; x < perlinArr.GetLength(0); x++)
        {
            for (int y = 0; y < perlinArr.GetLength(1); y++)
            {
                if(perlinArr[x, y] >= 1 && perlinArr[x, y] <= 3)
                {
                    tilemapFG.SetTile(new Vector3Int(x, y, 0), tiles[perlinArr[x, y] - 1]);
                }
            }
        }
    }
}
