using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMine : MonoBehaviour
{
    // Public variables
    public Tilemap Tilemap;
    public GameObject BlockBreaking;
    public GameObject Pickaxe;
    public ItemDatabaseObject Database;
    public GameObject CreateableItem;
    public float MiningSpeed = 3;
    public float Reach = 5;
    public float MiningDuration;

    // Private variables
    Animator _pickaxeAnim;
    Animator _breakingAnim;
    Vector3 _mousePos = Vector3.zero;
    Vector3Int _mousePosTranslated = Vector3Int.zero;
    Vector3Int _posSelectedTile = Vector3Int.zero;
    float _timeSinceMiningStart;
    Generation.BlockType[,] _mapArr;
    List<GameObject> _trees;

    // Start is called before the first frame update
    void Start()
    {
        _breakingAnim = BlockBreaking.GetComponent<Animator>();
        _pickaxeAnim = Pickaxe.GetComponent<Animator>();

        _mapArr = Generation.s_perlinArr;
        _trees = GenerateTrees.s_trees;
    }

    // Update is called once per frame
    void Update()
    {
        _breakingAnim.speed = MiningSpeed;
        MiningDuration = 1 / MiningSpeed;

        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Converts worlds space click point to tile map click point
        _mousePosTranslated = Tilemap.WorldToCell(_mousePos);

        // Mouse position changed from the previous selection
        if (_mousePosTranslated != _posSelectedTile)
        {
            _timeSinceMiningStart = 0;
            _breakingAnim.Play("Idle");
            _pickaxeAnim.Play("Idle");
            _posSelectedTile = _mousePosTranslated;
        }

        // Conditions are met for mining a block
        if (Input.GetKey(KeyCode.Mouse0) && IsInMap(_posSelectedTile) && WithinBounds(_posSelectedTile, Reach) && BlockExists(_posSelectedTile, _mapArr) && !TreeOnBlock(_posSelectedTile, _trees))
        {
            // Moves the gameobject containing the blockbreaking animation to the selected position
            BlockBreaking.transform.position = new Vector3(0.5f + _posSelectedTile.x, 0.5f + _posSelectedTile.y);
            // Plays the animations
            _breakingAnim.Play("BlockBreaking");
            _pickaxeAnim.Play("Swinging");

            _timeSinceMiningStart += Time.deltaTime;

            // Mining completed
            if (_timeSinceMiningStart > MiningDuration)
            {
                // Stops animation
                _breakingAnim.Play("Idle");
                _pickaxeAnim.Play("Idle");
                _timeSinceMiningStart = 0;

                // Romoves block from tilemap
                Tilemap.SetTile(_posSelectedTile, null);
                GenerateItem(_mapArr[_posSelectedTile.x, _posSelectedTile.y]);
                // Deletes block from the 2D world map array.
                _mapArr[_posSelectedTile.x, _posSelectedTile.y] = Generation.BlockType.None;
            }
        }

        // Mining was interrupted
        if (Input.GetKeyUp(KeyCode.Mouse0) || !IsInMap(_posSelectedTile) || (!BlockExists(_posSelectedTile, _mapArr) && !SaplingExists(_posSelectedTile)))
        {
            _breakingAnim.Play("Idle");
            _pickaxeAnim.Play("Idle");
            _timeSinceMiningStart = 0;
        }
    }

    /// <summary>
    /// Creates a game object containing the item object of the block.
    /// </summary>
    /// <param name="blockType"></param>
    void GenerateItem(Generation.BlockType blockType)
    {
        Debug.Log(blockType);

        if (blockType == Generation.BlockType.Grass)
        {
            blockType = Generation.BlockType.Dirt;
        }

        CreateableItem.GetComponent<GroundItem>().item = Database.Items[(int)blockType];
        Instantiate(CreateableItem, _posSelectedTile + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
    }

    /// <summary>
    /// Checks if there is a tree object on top of the block.
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="trees"></param>
    /// <returns></returns>
    bool TreeOnBlock(Vector3Int pos, List<GameObject> trees)
    {
        foreach (GameObject tree in trees)
        {
            if (tree.transform.position == pos + new Vector3(0, 1, 0))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Checks if the mouse position is within the player's reach.
    /// </summary>
    /// <param name="clickPos"></param>
    /// <param name="reach"></param>
    /// <returns></returns>
    bool WithinBounds(Vector3Int clickPos, float reach)
    {
        Vector3 playerPos = gameObject.transform.position;
        return (playerPos - clickPos).magnitude <= reach;
    }

    /// <summary>
    /// Checks if block exists at position.
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="mapArr"></param>
    /// <returns></returns>
    bool BlockExists(Vector3Int pos, Generation.BlockType[,] mapArr)
    {
        Generation.BlockType blockTypeAtPos = mapArr[pos.x, pos.y];
        return blockTypeAtPos > Generation.BlockType.None && blockTypeAtPos < Generation.BlockType.Tree;
    }

    /// <summary>
    /// Checks if sapling exists at position.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    bool SaplingExists(Vector3Int pos)
    {
        foreach (GameObject tree in _trees)
        {
            if (tree.name.Contains("Sapling") && tree.transform.position == pos)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Checks if the position is inside the world map.
    /// </summary>
    /// <param name="clickPos"></param>
    /// <returns></returns>
    bool IsInMap(Vector3Int clickPos)
    {
        return clickPos.x >= 0 && clickPos.x < Generation.s_width && clickPos.y >= 0 && clickPos.y < Generation.s_height;
    }
}