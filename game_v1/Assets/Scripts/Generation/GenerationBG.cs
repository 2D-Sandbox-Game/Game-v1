using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerationBG : Generation
{
    // Public variables
    public Tile TileBG;

    // Private variables
    Tilemap _tilemapBG;

    // Start is called before the first frame update
    void Start()
    {
        _tilemapBG = GetComponent<Tilemap>();
        GenerateBackground(s_perlinArr, s_perlinHeight, TileBG);
    }

    /// <summary>
    /// Sets the background component (cave walls) in the tilemap according to the 2D perlin array.
    /// </summary>
    /// <param name="perlinArr"></param>
    /// <param name="perlinHeight"></param>
    /// <param name="tileBG"></param>
    void GenerateBackground(BlockType[,] perlinArr, int[] perlinHeight, Tile tileBG)
    {
        for (int x = 0; x < perlinArr.GetLength(0); x++)
        {
            for (int y = 0; y <= perlinHeight[x]; y++)
            {
                _tilemapBG.SetTile(new Vector3Int(x, y, 0), tileBG);
            }
        }
    }
}
