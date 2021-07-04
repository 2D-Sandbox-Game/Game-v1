using System.Collections;
using UnityEngine;

public class ZombieCollider : MonoBehaviour
{
    // Public variables
    public int damage = 1;

    // Private variables
    bool doAttack = true;

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Collision enemy and player 
        if (collision.gameObject.tag == "Player" && doAttack)
        {
            // Executes HealthDamage() with enemy object as parameter
            collision.gameObject.SendMessage("HealthDamage", transform.parent.gameObject, SendMessageOptions.DontRequireReceiver);

            doAttack = false;
            StartCoroutine(WaitForSecondAttack());
        }
    }

    // 1 second delay
    private IEnumerator WaitForSecondAttack()
    {
        yield return new WaitForSeconds(1f);
        doAttack = true;
    }
}
