using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using System.Linq;

public class Generation : MonoBehaviour
{
    public static int height = 512;
    public static int width = 512;

    public static float smooth = 90;
    [Range(0, 1)]
    public static float caveMod = 0.02f;

    public static int seed;

    public enum BlockType { None, Dirt, Stone, Grass, Cave };

    [Range(0, 1)]
    public float alpha;

    public static int[,] perlinArr;
    public static int[] perlinHeight;


    private void Awake()
    {
        seed = UnityEngine.Random.Range(-1000000, 1000000);
        perlinArr = GenerateWorld(width, height, seed, caveMod, ref perlinHeight);
    }

    static int[,] GenerateWorld(int width, int height, int seed, float caveMod, ref int[] perlinHeight)
    {
        int[,] perlinArr = new int[width, height];

        int repetitions = 3;
        float[] mod = new float[repetitions];
        mod = mod.Select(x => UnityEngine.Random.Range(0.8f, 1f)).ToArray();

        //Array.ForEach(mod, x => Debug.Log($"Nr: {x}"));

        for (int x = 0; x < width; x++)
        {
            perlinHeight = RecursivePerlinNoiseCombination(0, mod);

            for (int y = 0; y < perlinHeight[x]; y++)
            {
                int perlinCave = Mathf.RoundToInt(Mathf.PerlinNoise(x * caveMod + seed, y * caveMod + seed));

                if (perlinCave == 1 && perlinHeight[x] - y > 20)
                {
                    perlinArr[x, y] = (int)BlockType.Cave;
                    //map.SetTile(new Vector3Int(x - width / 2, y - height, 0), stone);
                    //map.SetTileFlags(new Vector3Int(x - width / 2, y - height, 0), TileFlags.None);
                    //map.SetColor(new Vector3Int(x - width / 2, y - height, 0), Color.HSVToRGB(0, 0, 0.35f));
                }
                else
                {
                    if (perlinHeight[x] - y > 40)
                    {
                        perlinArr[x, y] = (int)BlockType.Stone;
                        //map.SetTile(new Vector3Int(x - width / 2, y - height, 0), stone);
                        //map.SetTileFlags(new Vector3Int(x - width / 2, y - height, 0), TileFlags.None);
                        //map.SetColor(new Vector3Int(x - width / 2, y - height, 0), Color.HSVToRGB(0, 0, 0.35f));
                    }
                    else
                    {
                        perlinArr[x, y] = (int)BlockType.Dirt;
                        //map.SetTile(new Vector3Int(x - width / 2, y - height, 0), dirt);
                        //map.SetTileFlags(new Vector3Int(x - width / 2, y - height, 0), TileFlags.None);
                        //map.SetColor(new Vector3Int(x - width / 2, y - height, 0), Color.HSVToRGB(0,0, 0.35f));
                    }
                }
            }

            //TileBase highestTile = map.GetTile(new Vector3Int(x, perlinHeight[x], 0));

            if (perlinArr[x, perlinHeight[x] - 1] == 1)
            {
                perlinArr[x, perlinHeight[x]] = (int)BlockType.Grass;
                //map.SetTile(new Vector3Int(x - width / 2, perlinHeight[x] - height, 0), grass);
                //map.SetTileFlags(new Vector3Int(x - width / 2, perlinHeight[x] - height, 0), TileFlags.None);
                //map.SetColor(new Vector3Int(x - width / 2, perlinHeight[x] - height, 0), Color.HSVToRGB(0, 0, 0.35f));

            }
        }

        return perlinArr;
    }

    static int[] CreatePerlinNoiseArray(float mod)
    {
        int[] arr = new int[width];

        for (int x = 0; x < width; x++)
        {
            //if(x < 20)
            //{
            //    Debug.Log($"{Mathf.PerlinNoise(x / (smooth * mod), seed) * height} ");
            //}
            arr[x] = Mathf.RoundToInt(Mathf.PerlinNoise(x / (smooth * mod), seed) * 40);
            arr[x] += (int)(height * 0.75);
        }

        return arr;
    }

    static int[] AddPerlinNoiseArray(int[] arr1, int[] arr2)
    {
        int[] temp = new int[width];

        for (int x = 0; x < width; x++)
        {
            temp[x] = (arr1[x] + arr2[x]) / 2;
        }

        return temp;
    }

    static int[] RecursivePerlinNoiseCombination(int repetitions, float[] mod)
    {
        if (repetitions == 0)
        {

            return CreatePerlinNoiseArray(mod[repetitions]);
        }

        return AddPerlinNoiseArray(RecursivePerlinNoiseCombination(repetitions - 1, mod), CreatePerlinNoiseArray(mod[repetitions]));
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.J))
        //{
        //    GenerateWorld();
        //}
        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    map.ClearAllTiles();
        //}
    }
}
