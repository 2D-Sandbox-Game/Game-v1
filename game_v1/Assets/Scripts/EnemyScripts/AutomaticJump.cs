using UnityEngine;

public class AutomaticJump : MonoBehaviour
{
    // Public variables
    public float YOffset = 2f;
    public float XOffset = 0.6f;

    // Private variables
    GameObject _enemy;
    Generation.BlockType[,] _mapArr;

    // Start is called before the first frame update
    void Start()
    {
        _enemy = gameObject.transform.parent.gameObject;
        _mapArr = Generation.s_perlinArr;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        int dir = _enemy.GetComponent<Direction>().dir;

        // Collision enemy and enemy
        if (collision.gameObject.tag == "Enemy")
        {
            // Ignore collision
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>(), true);
        }

        // Conditions for auto jump are met
        if (collision.gameObject.tag == "Ground" && Mathf.Abs(collision.contacts[0].normal.x) > 0.5f && !IsBlockedByWall(_enemy.transform.position, dir))
        {
            // Performs auto jump
            _enemy.transform.position = new Vector3(transform.position.x + dir * XOffset, transform.position.y + YOffset, transform.position.z);
        }
    }

    /// <summary>
    /// Checks if the game object's path is blocked by a wall (min. height = 2 blocks).
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="dir"></param>
    /// <returns></returns>
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
}
