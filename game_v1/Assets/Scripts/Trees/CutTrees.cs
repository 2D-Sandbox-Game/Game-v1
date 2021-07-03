using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CutTrees : MonoBehaviour
{
    // Public variables
    public Tilemap Tilemap;
    public GameObject BlockBreaking;
    public ItemDatabaseObject Database;
    public GameObject DroppedWood;
    public GameObject DroppedAcorns;
    public float MiningSpeed = 3;
    public float Reach = 5;

    // Private variables
    Animator _axeAnim;
    Animator _breakingAnim;
    float _miningDuration;
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
        _axeAnim = gameObject.GetComponent<Animator>();

        _mapArr = Generation.s_perlinArr;
        _trees = GenerateTrees.trees;
    }

    // Update is called once per frame
    void Update()
    {
        _breakingAnim.speed = MiningSpeed;
        // Mining duration is the inverse of mining speed
        _miningDuration = 1 / MiningSpeed;

        // World Space
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Converts worlds space click point to tile map click point.
        _mousePosTranslated = Tilemap.WorldToCell(_mousePos);

        // Mouse position changed from the previous selection
        if (_mousePosTranslated != _posSelectedTile)
        {
            _timeSinceMiningStart = 0;
            _breakingAnim.Play("Idle");
            _axeAnim.Play("Idle");
            _posSelectedTile = _mousePosTranslated;
        }

        // Conditions for cutting the tree are met
        if (Input.GetKey(KeyCode.Mouse0) && TreeExists(_posSelectedTile) && WithinBounds(_posSelectedTile, Reach))
        {
            // Moves the gameobject containing the blockbreaking animation to the selected position
            BlockBreaking.transform.position = new Vector3(0.5f + _posSelectedTile.x, 0.5f + _posSelectedTile.y);
            // Plays the animations
            _breakingAnim.Play("BlockBreaking");
            _axeAnim.Play("AxeSwinging");

            _timeSinceMiningStart += Time.deltaTime;

            // Cutting completed 
            if (_timeSinceMiningStart > _miningDuration)
            {
                // Stops animation
                _breakingAnim.Play("Idle");
                _axeAnim.Play("Idle");

                _timeSinceMiningStart = 0;
                DeleteTree(_posSelectedTile);
            }
        }

        // Mouse press was interrupted
        if (Input.GetKeyUp(KeyCode.Mouse0) || !TreeExists(_posSelectedTile))
        {
            // Stops animation
            _breakingAnim.Play("Idle");
            _axeAnim.Play("Idle");

            _timeSinceMiningStart = 0;
        }
    }

    /// <summary>
    /// Creates game objects containing the item objects "wood" and "acorn".
    /// </summary>
    /// <param name="tree"></param>
    void GenerateItem(GameObject tree)
    {
        // Sets the amount of wood according to the tree size
        int amountWood = tree.GetComponentsInChildren<Transform>().Length * 4;
        // Sets the item object to "wood"
        DroppedWood.GetComponent<GroundItem>().item = Database.Items[9];

        for (int i = 0; i < amountWood; i++)
        {
            // Creates a gameobject containing the item object "wood"
            Instantiate(DroppedWood, _posSelectedTile + new Vector3(0.4f, 0.5f, 0), Quaternion.identity);
        }

        // Sets the item object to "wood"
        DroppedAcorns.GetComponent<GroundItem>().item = Database.Items[8];

        for (int i = 0; i < Random.Range(1, 3); i++)
        {
            // Creates a gameobject containing the item object "acorn"
            Instantiate(DroppedAcorns, _posSelectedTile + new Vector3(0.6f, 0.5f, 0), Quaternion.identity);
        }
    }

    /// <summary>
    /// Checks if a tree already exists at the position.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    bool TreeExists(Vector3Int pos)
    {
        foreach (GameObject tree in _trees)
        {
            if (tree.name.Contains("Tree") && tree.transform.position == pos)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Deletes the tree game object and creates a game object containing item objects.
    /// </summary>
    /// <param name="pos"></param>
    void DeleteTree(Vector3Int pos)
    {
        foreach (GameObject tree in _trees)
        {
            if (tree.transform.position == pos)
            {
                GenerateItem(tree);
                RemoveTreeFromMap(tree, pos, _mapArr);

                // Removes tree from tree array
                _trees.Remove(tree);
                // Destroys the game object
                Destroy(tree);
                return;
            }
        }
    }

    /// <summary>
    /// Deletes the tree from the 2D world map array.
    /// </summary>
    /// <param name="tree"></param>
    /// <param name="pos"></param>
    /// <param name="mapArr"></param>
    void RemoveTreeFromMap(GameObject tree, Vector3Int pos, Generation.BlockType[,] mapArr)
    {
        int trunkLength = tree.GetComponentsInChildren<Transform>().Length - 3;

        // Removes tree trunk from the map
        for (int i = 0; i <= trunkLength; i++)
        {
            mapArr[pos.x, pos.y + i] = Generation.BlockType.None;
        }

        // Removes tree crown from the map
        for (int i = pos.x - 1; i < pos.x + 2; i++)
        {
            for (int j = pos.y + trunkLength + 1; j < pos.y + trunkLength + 4; j++)
            {
                if (i >= 0 && i < mapArr.GetLength(0) && j >= 0 && j < mapArr.GetLength(1))
                {
                    mapArr[i, j] = Generation.BlockType.None;
                }
            }
        }
    }

    /// <summary>
    /// Checks if the mouse position is within the player's reach.
    /// </summary>
    /// <param name="clickPos"></param>
    /// <param name="reach"></param>
    /// <returns></returns>
    bool WithinBounds(Vector3Int clickPos, float reach)
    {
        Vector3 playerPos = gameObject.transform.parent.parent.transform.position;
        return (playerPos - clickPos).magnitude <= reach;
    }
}
