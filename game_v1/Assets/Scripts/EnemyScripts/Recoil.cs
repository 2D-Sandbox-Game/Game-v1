using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    float force = 3;
    public GameObject enemy;
    float waitTime = 2.0f;//2Sekunden warten als standart Wert
    float elapsedTime = 0.0f;
    Rigidbody2D r;


    public PlayerMovement playerMovement;
    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Rigidbody2D>();     
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            /*enemy.GetComponent<Direction>().enabled = false;
            enemy.GetComponent<Animator>().enabled = false;*/
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            enemy.GetComponent<Direction>().enabled = false;
            enemy.GetComponent<Animator>().enabled = false;
        }

    }

}
