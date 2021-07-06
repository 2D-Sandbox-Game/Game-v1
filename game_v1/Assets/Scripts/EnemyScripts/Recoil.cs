using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    float force = 3;
    public GameObject Enemy;
    float waitTime = 2.0f; 
    float elapsedTime = 0.0f;
    Rigidbody2D R;


    public PlayerMovement PlayerMovement;
    // Start is called before the first frame update
    void Start()
    {
        R = GetComponent<Rigidbody2D>();     
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Enemy.GetComponent<Direction>().enabled = false;
            Enemy.GetComponent<Animator>().enabled = false;
        }

    }

}
