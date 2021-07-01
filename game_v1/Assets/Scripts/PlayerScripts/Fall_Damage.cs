using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall_Damage : MonoBehaviour
{
    public static float velocity = 0;

    void Start()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        transform.parent.gameObject.GetComponent<PlayerMovement>().enabled = false;
        velocity = transform.parent.gameObject.GetComponent<Rigidbody2D>().velocity.y;

        transform.parent.gameObject.GetComponent<PlayerMovement>().enabled = true;

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //transform.parent.gameObject.GetComponent<PlayerMovement>().enabled = false;

        //if (collision.gameObject.tag.Equals("Ground") && velocity < -30) // if the player falls more than 30 
        //{
        //    PlayerHealth.animator.Play("Damage"); // Run animation of damage
        //    PlayerHealth.health += (int)(transform.parent.gameObject.GetComponent<Rigidbody2D>().velocity.y + 27) / 3;
        //    Debug.Log(transform.parent.gameObject.GetComponent<Rigidbody2D>().velocity.y);
        //    velocity = 0;
        //}

        //transform.parent.gameObject.GetComponent<PlayerMovement>().enabled = true;

    }

}
