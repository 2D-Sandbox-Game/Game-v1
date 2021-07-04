using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAutoJump : MonoBehaviour
{
    public float yOffset = 1.8f;
    public float xOffset = 0.6f;
    GameObject _player;
    Generation.BlockType[,] _mapArr;

    // Start is called before the first frame update
    void Start()
    {
        _player = gameObject.transform.parent.gameObject;
        _mapArr = Generation.perlinArr;
    }
    // Function moves the player up the steps
    private void OnCollisionStay2D(Collision2D collision)
    {
        float dir = _player.GetComponent<PlayerMovement>().mx;

        if (collision.gameObject.tag == "Ground" && Mathf.Abs(collision.contacts[0].normal.x) > 0.5f && !IsBlockedByWall(_player.transform.position, dir))
        {
            _player.transform.position = new Vector3(transform.position.x + dir * xOffset, transform.position.y + yOffset, transform.position.z);
        }
    }

    bool IsBlockedByWall(Vector3 pos, float dir)
    {
        Vector3Int vecDir;
        if (dir < 0)
        {
            vecDir = new Vector3Int(-1, 0, 0);
        }
        else
        {
            vecDir = new Vector3Int(1, 0, 0);
        }
        Vector3Int posTranslated = new Vector3Int((int)pos.x, (int)pos.y, (int)pos.z);
        return BlockExists(posTranslated + vecDir, _mapArr);
    }

    bool BlockExists(Vector3Int pos, Generation.BlockType[,] mapArr)
    {
        Generation.BlockType blockTypeAtPos = mapArr[pos.x, pos.y];
        return blockTypeAtPos > Generation.BlockType.None && blockTypeAtPos < Generation.BlockType.Tree;
    }
}
