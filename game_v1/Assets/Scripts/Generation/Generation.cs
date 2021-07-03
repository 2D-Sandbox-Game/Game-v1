using UnityEngine;
using System.Linq;

public class Generation : MonoBehaviour
{
    // Static variables
    public static int s_height = 450;
    public static int s_width = 800;
    // Holds a 2D-array representing the world map
    public static BlockType[,] s_perlinArr;
    // Holds an array representing the highest points along the X-Axis
    public static int[] s_perlinHeight;

    // Public variables (Terrain parameters)
    public float Smoothness = 90;
    [Range(0, 1)]
    public float CaveMod = 0.02f;
    public float CopperMod = 20;
    public float IronMod = 30;
    public float GoldMod = 40;
    public int DirtLayerSize = 2;
    public int Seed;
    public enum BlockType { None, Dirt, Stone, Grass, Copper, Iron, Gold, Wood, Tree, Sapling, Cave};

    // Delegates
    public delegate void Del(int[] arr);
    public Del GenerationFinished;

    // Awake is called before the Start() Method
    void Awake()
    {
        // Seed Generation
        Seed = UnityEngine.Random.Range(-1000000, 1000000);
        // World Generation
        s_perlinArr = GenerateWorld(s_width, s_height, Seed, CaveMod, ref s_perlinHeight);
    }

    // Start is called before the first frame update
    private void Start()
    {
        // Adds the Player's Spawn() Method to the GenerationFinished Delegate
        GenerationFinished += new Del(GameObject.Find("Player").GetComponent<PlayerSpawn>().Spawn);
    }

    /// <summary>
    /// Returns a 2D array representing the generated world map.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="seed"></param>
    /// <param name="caveMod"></param>
    /// <param name="perlinHeight"></param>
    /// <returns></returns>
    BlockType[,] GenerateWorld(int width, int height, int seed, float caveMod, ref int[] perlinHeight)
    {
        BlockType[,] perlinArr = new BlockType[width, height];

        int repetitions = 3;
        float[] mod = new float[repetitions];

        // Sets each field of the float array mod with a random number between 0.8f and 1f. 
        mod = mod.Select(x => UnityEngine.Random.Range(0.8f, 1f)).ToArray();

        for (int x = 0; x < width; x++)
        {
            // Holds an array representing the highest points along the X-Axis
            perlinHeight = RecursivePerlinNoiseCombination(0, mod);

            for (int y = 0; y < perlinHeight[x]; y++)
            {
                // Perlin noise arrays for different generation structures
                int perlinCave = Mathf.RoundToInt(Mathf.PerlinNoise(x * caveMod + seed, y * caveMod + seed));
                float perlinCopper = Mathf.PerlinNoise((x/ CopperMod) + seed, (y / CopperMod) + seed);
                float perlinIron = Mathf.PerlinNoise((x / IronMod) + seed, (y / IronMod) + seed);
                float perlinGold = Mathf.PerlinNoise((x / GoldMod) + seed, (y / GoldMod) + seed);

                // Cave generation below specified dirt layer size
                if (perlinCave == 1 && perlinHeight[x] - y > DirtLayerSize)
                {
                    perlinArr[x, y] = BlockType.Cave;
                }
                else
                {
                    // Ore generation 40 blocks below surface
                    if (perlinHeight[x] - y > 40)
                    {
                        // Probability of each ore being created
                        // 25 %
                        if (perlinCopper > 0.75)
                        {
                            perlinArr[x, y] = BlockType.Copper;
                        }
                        // 20 %
                        else if (perlinIron > 0.80)
                        {
                            perlinArr[x, y] = BlockType.Iron;
                        }
                        // 15 %
                        else if (perlinGold > 0.85)
                        {
                            perlinArr[x, y] = BlockType.Gold;
                        }
                        else
                        {
                            perlinArr[x, y] = BlockType.Stone;
                        }
                    }
                    else
                    {
                        perlinArr[x, y] = BlockType.Dirt;

                    }
                }
            }

            // Generates grass layer on the surface
            if (perlinArr[x, perlinHeight[x] - 1] == BlockType.Dirt)
            {
                perlinArr[x, perlinHeight[x]] = BlockType.Grass;
            }
        }

        return perlinArr;
    }

    /// <summary>
    /// Creates a perlin noise array based on the terrain parameters.
    /// </summary>
    /// <param name="mod"></param>
    /// <returns></returns>
    int[] CreatePerlinNoiseArray(float mod)
    {
        int[] arr = new int[s_width];

        for (int x = 0; x < s_width; x++)
        {
            arr[x] = Mathf.RoundToInt(Mathf.PerlinNoise(x / (Smoothness * mod), Seed) * 40);
            arr[x] += (int)(s_height * 0.75);
        }

        return arr;
    }

    /// <summary>
    /// Adds two perlin noise arrays together.
    /// </summary>
    /// <param name="arr1"></param>
    /// <param name="arr2"></param>
    /// <returns></returns>
    int[] AddPerlinNoiseArray(int[] arr1, int[] arr2)
    {
        int[] temp = new int[s_width];

        for (int x = 0; x < s_width; x++)
        {
            temp[x] = (arr1[x] + arr2[x]) / 2;
        }

        return temp;
    }

    /// <summary>
    /// Creates and combines a certain number of perlin noise arrays.
    /// </summary>
    /// <param name="repetitions"></param>
    /// <param name="mod"></param>
    /// <returns></returns>
    int[] RecursivePerlinNoiseCombination(int repetitions, float[] mod)
    {
        if (repetitions == 0)
        {
            return CreatePerlinNoiseArray(mod[repetitions]);
        }

        return AddPerlinNoiseArray(RecursivePerlinNoiseCombination(repetitions - 1, mod), CreatePerlinNoiseArray(mod[repetitions]));
    }

}
