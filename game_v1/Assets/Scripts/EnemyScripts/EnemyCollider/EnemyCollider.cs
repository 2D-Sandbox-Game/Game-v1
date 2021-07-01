using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollider : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            transform.parent.gameObject.GetComponent<Direction>().pathBlocked = true;
        }

        //Debug.Log("teeeest");
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.parent.gameObject.GetComponent<Direction>().pathBlocked = false;

        //Debug.Log("teeeest");
    }

}
