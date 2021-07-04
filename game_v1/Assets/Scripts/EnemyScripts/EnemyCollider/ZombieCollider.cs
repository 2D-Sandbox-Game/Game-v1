using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieCollider : MonoBehaviour
{
    bool doAttack = true;
    public int damage = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //if (collision.gameObject.tag == "Enemy")
        //{
        //    Physics2D.IgnoreCollision(transform.parent.gameObject.GetComponent<Collider2D>(), collision, true);
        //}

        if (collision.gameObject.tag == "Player" && doAttack)
        {

            collision.gameObject.SendMessage("DamageHealth", transform.parent.gameObject, SendMessageOptions.DontRequireReceiver);

            doAttack = false;
            StartCoroutine(WaitForSecondAttack());
        }
        //Debug.Log("teeeest");
    }

    private IEnumerator WaitForSecondAttack()
    {
        yield return new WaitForSeconds(1f);
        doAttack = true;
    }
}
