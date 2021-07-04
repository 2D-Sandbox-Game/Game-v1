using UnityEngine;

public class EnemyCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Collision enemy with ground
        if (collision.gameObject.tag == "Ground")
        {
            transform.parent.gameObject.GetComponent<Direction>().pathBlocked = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.parent.gameObject.GetComponent<Direction>().pathBlocked = false;
    }
}
