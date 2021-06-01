using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class Generation : MonoBehaviour
{
    public int height = 512;
    public int width = 512;

    public float smooth = 1;
    [Range(0, 1)]
    [SerializeField] float modifier = 1;

    [SerializeField] float mul1 = 1;
    [SerializeField] float mul2 = 1;
    [SerializeField] float mul3 = 1;

    public int seed;

    public Tilemap map;
    public Tile dirt;
    public Tile dirt2;
    public Tile stone;
    public Tile grass;

    public static Vector3 playerSpawn; // Variable von Player

    // Start is called before the first frame update
    void Awake()
    {
        seed = Random.Range(-1000000, 1000000);
        GenerateWorld();
        playerSpawn = SpawnPoint(new Vector3Int(0,0,0)); 
        Debug.Log(playerSpawn);
    }

    void GenerateWorld()
    {
        int[] perlinHeight;

        for (int x = 0; x < width; x++)
        {
            perlinHeight = AddPerlinNoiseArray(AddPerlinNoiseArray(CreatePerlinNoiseArray(mul1), CreatePerlinNoiseArray(mul2)), CreatePerlinNoiseArray(mul3));
            //perlinHeight = CreatePerlinNoiseArray();

            for (int y = 0; y < perlinHeight[x]; y++)
            {
                int perlinRot = Mathf.RoundToInt(Mathf.PerlinNoise(x * modifier + seed, y * modifier + seed));

                if (perlinHeight[x] - y > 10)
                {
                    map.SetTile(new Vector3Int(x - width / 2, y - height, 0), stone);
                }
                else
                {
                    if (perlinRot == 1)
                    {
                        map.SetTile(new Vector3Int(x - width / 2, y - height, 0), dirt2);
                    }
                    else
                    {
                        map.SetTile(new Vector3Int(x - width / 2, y - height, 0), dirt);
                    }
                }
            }

            map.SetTile(new Vector3Int(x - width / 2, perlinHeight[x] - height, 0), grass);
        }
    }

    int[] CreatePerlinNoiseArray(float mod = 1)
    {
        int[] arr = new int[width];

        for (int x = 0; x < width; x++)
        {
            //if(x < 20)
            //{
            //    Debug.Log($"{Mathf.PerlinNoise(x / (smooth * mod), seed) * height} ");
            //}
            
            arr[x] = Mathf.RoundToInt(Mathf.PerlinNoise(x / (smooth * mod), seed) * height);
            arr[x] += height / 2;
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
    public Vector3 SpawnPoint(Vector3Int x)
    {
        if(map.GetTile(x)==null)
        {
            while(map.GetTile(x)==null) // move down
            {
                Debug.Log(x);
                x.y--;
            }
        } else 
        {
            while(map.GetTile(x)!=null) // move up
            {
                Debug.Log(x);
                x.y++;
            }
        }
        return map.CellToWorld(x);;
    }

}
