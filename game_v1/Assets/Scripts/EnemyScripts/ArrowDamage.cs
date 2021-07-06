using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowDamage : MonoBehaviour
{
    public int damage = 1;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.SendMessage("HealthDamage", gameObject, SendMessageOptions.DontRequireReceiver);
        }
    }

}
