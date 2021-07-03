using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall_Damage : MonoBehaviour
{
    public static float s_velocity = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        transform.parent.gameObject.GetComponent<PlayerMovement>().enabled = false;
        s_velocity = transform.parent.gameObject.GetComponent<Rigidbody2D>().velocity.y;

        transform.parent.gameObject.GetComponent<PlayerMovement>().enabled = true;
    }

}
