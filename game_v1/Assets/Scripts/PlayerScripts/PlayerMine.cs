using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class PlayerMine : MonoBehaviour
{
    public Tilemap tilemap;
    public GameObject blockBreaking;
    public GameObject pickaxe;
    public ItemDatabaseObject database;
    public GameObject createableItem;
    public float miningSpeed = 3;
    public float reach = 5;
    public float miningDuration;

    Animator _pickaxeAnim;
    Animator _breakingAnim;
    TileBase _selectedTile = null;
    Vector3 _mousePos = Vector3.zero;
    Vector3Int _mousePosTranslated = Vector3Int.zero;
    Vector3Int _posSelectedTile = Vector3Int.zero;
    float _timeSinceMiningStart;
    Generation.BlockType[,] _mapArr;
    List<GameObject> _trees;

    // Start is called before the first frame update
    void Start()
    {
        _breakingAnim = blockBreaking.GetComponent<Animator>();
        _pickaxeAnim = pickaxe.GetComponent<Animator>();
        _mapArr = Generation.perlinArr;
        _trees = GenerateTrees.trees;
    }
    // Update is called once per frame
    void Update()
    {
        _breakingAnim.speed = miningSpeed;
        miningDuration = 1 / miningSpeed;
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _mousePosTranslated = tilemap.WorldToCell(_mousePos);
        _selectedTile = tilemap.GetTile(_mousePosTranslated);
        if (_mousePosTranslated != _posSelectedTile)
        {
            _timeSinceMiningStart = 0;
            _breakingAnim.Play("Idle");
            _pickaxeAnim.Play("Idle");
            _posSelectedTile = _mousePosTranslated;
        }
        if (Input.GetKey(KeyCode.Mouse0) && IsInMap(_posSelectedTile) && WithinBounds(_posSelectedTile, reach) && BlockExists(_posSelectedTile, _mapArr) && !TreeOnBlock(_posSelectedTile, _trees))
        {
            blockBreaking.transform.position = new Vector3(0.5f + _posSelectedTile.x, 0.5f + _posSelectedTile.y);
            _breakingAnim.Play("BlockBreaking");
            _pickaxeAnim.Play("Swinging");
            _timeSinceMiningStart += Time.deltaTime;
            if (_timeSinceMiningStart > miningDuration)
            {
                _breakingAnim.Play("Idle");
                _pickaxeAnim.Play("Idle");
                _timeSinceMiningStart = 0;
                tilemap.SetTile(_posSelectedTile, null);
                GenerateItem(_mapArr[_posSelectedTile.x, _posSelectedTile.y]);
                _mapArr[_posSelectedTile.x, _posSelectedTile.y] = Generation.BlockType.None;
            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse0) || !IsInMap(_posSelectedTile) || (!BlockExists(_posSelectedTile, _mapArr) && !SaplingExists(_posSelectedTile)))
        {
            _breakingAnim.Play("Idle");
            _pickaxeAnim.Play("Idle");
            _timeSinceMiningStart = 0;
        }
    }
    void GenerateItem(Generation.BlockType blockType)
    {
        if (blockType == Generation.BlockType.Grass)
        {
            blockType = Generation.BlockType.Dirt;
        }
        createableItem.GetComponent<GroundItem>().item = database.Items[(int)blockType];
        Instantiate(createableItem, _posSelectedTile + new Vector3(0.5f, 0.5f, 0), Quaternion.identity);
    }
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
    bool WithinBounds(Vector3Int clickPos, float reach)
    {
        Vector3 playerPos = gameObject.transform.position;
        return (playerPos - clickPos).magnitude <= reach;
    }

    bool BlockExists(Vector3Int pos, Generation.BlockType[,] mapArr)
    {
        Generation.BlockType blockTypeAtPos = mapArr[pos.x, pos.y];
        return blockTypeAtPos > Generation.BlockType.None && blockTypeAtPos < Generation.BlockType.Tree;
    }
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
    bool IsInMap(Vector3Int clickPos)
    {
        return clickPos.x >= 0 && clickPos.x < Generation.width && clickPos.y >= 0 && clickPos.y < Generation.height;
    }
}

