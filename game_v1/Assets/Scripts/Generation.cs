using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using System.Linq;

public class Generation : MonoBehaviour
{

    public int height = 512;
    public int width = 512;

    public float smooth = 1;
    [Range(0, 1)]
    [SerializeField] float modifier = 1;

    //[SerializeField] float mul1 = 1;
    //[SerializeField] float mul2 = 1;
    //[SerializeField] float mul3 = 1;

    public int seed;

    public Tilemap map;
    public Tile dirt;
    public Tile dirt2;
    public Tile stone;
    public Tile grass;

    // Start is called before the first frame update
    void Start()
    {
        seed = UnityEngine.Random.Range(-1000000, 1000000);
        GenerateWorld();
    }

    void GenerateWorld()
    {
        int[] perlinHeight;

        int repetitions = 3;
        float[] mod = new float[repetitions];
        mod = mod.Select(x => UnityEngine.Random.Range(0.8f, 1f)).ToArray();

        //Array.ForEach(mod, x => Debug.Log($"Nr: {x}"));

        for (int x = 0; x < width; x++)
        {
            perlinHeight = RecursivePerlinNoiseCombination(0, mod);

            for (int y = 0; y < perlinHeight[x]; y++)
            {
                int perlinCave = Mathf.RoundToInt(Mathf.PerlinNoise(x * modifier + seed, y * modifier + seed));


                if (perlinCave == 1 && perlinHeight[x] - y > 10)
                {
                    map.SetTile(new Vector3Int(x - width / 2, y - height, 0), null);
                }
                else
                {
                    if (perlinHeight[x] - y > 40)
                    {
                        map.SetTile(new Vector3Int(x - width / 2, y - height, 0), stone);
                    }
                    else
                    {
                        map.SetTile(new Vector3Int(x - width / 2, y - height, 0), dirt);
                    }
                }
            }

            TileBase highestTile = map.GetTile(new Vector3Int(x - width / 2, perlinHeight[x] - 1 - height, 0));

            if (highestTile != null && highestTile.name == "dirt")
            {
                map.SetTile(new Vector3Int(x - width / 2, perlinHeight[x] - height, 0), grass);
            }
        }
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
        if (Input.GetKeyDown(KeyCode.J))
        {
            GenerateWorld();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            map.ClearAllTiles();
        }
    }
}
