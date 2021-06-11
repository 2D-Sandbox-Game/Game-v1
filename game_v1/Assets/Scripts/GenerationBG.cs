using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerationBG : MonoBehaviour
{
    int[,] perlinArr;
    int[] perlinHeight;
    Tilemap tilemapBG;

    public Tile tileBG;

    void Start()
    {
        perlinArr = Generation.perlinArr;
        perlinHeight = Generation.perlinHeight;

        tilemapBG = GetComponent<Tilemap>();
        GenerateBackground(perlinArr, perlinHeight, tileBG);
    }

    void GenerateBackground(int[,] perlinArr, int[] perlinHeight, Tile tiles)
    {
        for (int x = 0; x < perlinArr.GetLength(0); x++)
        {
            for (int y = 0; y <= perlinHeight[x]; y++)
            {
                tilemapBG.SetTile(new Vector3Int(x, y, 0), tileBG);
            }
        }
    }
}
