using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerationFG : Generation
{
    // Public variables
    public Tile Dirt;
    public Tile Stone;
    public Tile Grass;
    public Tile Copper;
    public Tile Iron;
    public Tile Gold;

    // Private variables
    Tilemap _tilemapFG;
    Tile[] _tiles = new Tile[6];

    // Start is called before the first frame update
    void Start()
    {
        _tiles[0] = Dirt;
        _tiles[1] = Stone;
        _tiles[2] = Grass;
        _tiles[3] = Copper;
        _tiles[4] = Iron;
        _tiles[5] = Gold;

        _tilemapFG = GetComponent<Tilemap>();
        GenerateForeground(s_perlinArr, _tiles);
    }

    /// <summary>
    /// Sets the foreground components in the tilemap according to the 2D perlin array.
    /// </summary>
    /// <param name="perlinArr"></param>
    /// <param name="tiles"></param>
    void GenerateForeground(BlockType[,] perlinArr, Tile[] tiles)
    {
        for (int x = 0; x < perlinArr.GetLength(0); x++)
        {
            for (int y = 0; y < perlinArr.GetLength(1); y++)
            {
                // Checks if the blocktype is a foreground component
                if(perlinArr[x, y] > BlockType.None && perlinArr[x, y] < BlockType.Wood)
                {
                    _tilemapFG.SetTile(new Vector3Int(x, y, 0), tiles[(int)perlinArr[x, y] - 1]);
                }
            }
        }
    }
}
