using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MineSapling : MonoBehaviour
{
    // Public variables
    public GameObject Player;
    public Tilemap Tilemap;
    public GameObject BlockBreaking;
    public GameObject Pickaxe;
    public ItemDatabaseObject Database;
    public GameObject DroppedAcorns;
    public float MiningSpeed = 3;
    public float Reach = 5;
    float MiningDuration;

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

        _breakingAnim.speed = MiningSpeed;
        // Mining duration is the inverse of mining speed
        MiningDuration = 1 / MiningSpeed;

        _mapArr = Generation.s_perlinArr;
        _trees = GenerateTrees.s_trees;
    }

    // Update is called once per frame
    void Update()
    {
        //World Space
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Converts worlds space click point to tile map click point.
        _mousePosTranslated = Tilemap.WorldToCell(_mousePos);

        // Mouse position changed from the previous selection
        if (_mousePosTranslated != _posSelectedTile)
        {
            _timeSinceMiningStart = 0;
            _breakingAnim.Play("Idle");
            _pickaxeAnim.Play("Idle");
            _posSelectedTile = _mousePosTranslated;
        }

        // Conditions for mining a sapling are met
        if (Input.GetKey(KeyCode.Mouse0) && SaplingExists(_posSelectedTile) && WithinBounds(_posSelectedTile, Reach))
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

                DeleteSapling(_posSelectedTile);
            }
        }
    }

    /// <summary>
    /// Creates a game object containing the item object "acorn".
    /// </summary>
    void GenerateItem()
    {
        DroppedAcorns.GetComponent<GroundItem>().item = Database.items[8];
        Instantiate(DroppedAcorns, _posSelectedTile + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
    }

    /// <summary>
    /// Checks if the mouse position is within the player's reach.
    /// </summary>
    /// <param name="clickPos"></param>
    /// <param name="reach"></param>
    /// <returns></returns>
    bool WithinBounds(Vector3Int clickPos, float reach)
    {
        Vector3 playerPos = Player.transform.position;
        return (playerPos - clickPos).magnitude <= reach;
    }

    /// <summary>
    /// Check if sapling exists at position.
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
    /// Deletes the sapling from the map and destroys its game object.
    /// </summary>
    /// <param name="pos"></param>
    void DeleteSapling(Vector3Int pos)
    {
        // Deletes the sapling from the 2D world map array.
        _mapArr[pos.x, pos.y] = Generation.BlockType.None;
        _mapArr[pos.x, pos.y + 1] = Generation.BlockType.None;

        foreach (GameObject tree in _trees)
        {
            if (tree.transform.position == pos)
            {
                GenerateItem();
                _trees.Remove(tree);
                Destroy(tree);
                return;
            }
        }
    }
}
