using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticJump : MonoBehaviour
{
    public GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnCollisionEnter2D(Collision2D collision)
    { 
        if(collision.gameObject.tag == "Ground" && Mathf.Abs(collision.contacts[0].normal.x) > 0.5f)
            enemy.transform.position = new Vector3(transform.position.x, transform.position.y + 1.2f, transform.position.z);
    }
}
