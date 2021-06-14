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

    public float smooth = 90;
    //public float oreFrequency = 40;
    [Range(0, 1)]
    public float caveMod = 0.02f;

    public float copperMod = 20;
    public float ironMod = 30;
    public float goldMod = 40;

    public int seed;

    public enum BlockType { None, Dirt, Stone, Grass, Copper, Iron, Gold, Cave };

    public static int[,] perlinArr;
    public static int[] perlinHeight;


    private void Awake()
    {
        seed = UnityEngine.Random.Range(-1000000, 1000000);
        perlinArr = GenerateWorld(width, height, seed, caveMod, ref perlinHeight);
    }

    int[,] GenerateWorld(int width, int height, int seed, float caveMod, ref int[] perlinHeight)
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
                //int perlinCopper = Mathf.RoundToInt(Mathf.PerlinNoise(x * copperMod * copperMod * copperMod + seed, y * copperMod * copperMod * copperMod + seed));
                //int  = Mathf.RoundToInt(Mathf.PerlinNoise(x * ironMod + seed, y * ironMod + seed));
                // perlinGold = Mathf.RoundToInt(Mathf.PerlinNoise(x * goldMod + seed, y * goldMod + seed));
                float perlinCopper = Mathf.PerlinNoise((x/ copperMod) + seed, (y / copperMod) + seed);
                float perlinIron = Mathf.PerlinNoise((x / ironMod) + seed, (y / ironMod) + seed);
                float perlinGold = Mathf.PerlinNoise((x / goldMod) + seed, (y / goldMod) + seed);

                if (perlinCave == 1 && perlinHeight[x] - y > 20)
                {
                    perlinArr[x, y] = (int)BlockType.Cave;
                }
                else
                {
                    if (perlinHeight[x] - y > 40)
                    {
                        if (perlinCopper > 0.75)
                        {
                            perlinArr[x, y] = (int)BlockType.Copper;
                        }
                        else if (perlinIron > 0.80)
                        {
                            perlinArr[x, y] = (int)BlockType.Iron;
                        }
                        else if (perlinGold > 0.85)
                        {
                            perlinArr[x, y] = (int)BlockType.Gold;
                        }
                        else
                        {
                            perlinArr[x, y] = (int)BlockType.Stone;
                        }
                    }
                    else
                    {
                        perlinArr[x, y] = (int)BlockType.Dirt;

                    }
                }
            }

            if (perlinArr[x, perlinHeight[x] - 1] == 1)
            {
                perlinArr[x, perlinHeight[x]] = (int)BlockType.Grass;
            }
        }

        return perlinArr;
    }

    int[] CreatePerlinNoiseArray(float mod)
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

    int[] AddPerlinNoiseArray(int[] arr1, int[] arr2)
    {
        int[] temp = new int[width];

        for (int x = 0; x < width; x++)
        {
            temp[x] = (arr1[x] + arr2[x]) / 2;
        }

        return temp;
    }

    int[] RecursivePerlinNoiseCombination(int repetitions, float[] mod)
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
