using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticJump : MonoBehaviour
{
    public GameObject obj;
    public float yOffset = 1.2f;
   // public float xOffset = 0.6f;

    //int dir;
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
        //dir = obj.GetComponent<SpriteRenderer>().flipX ? -1 : 1;

        if (collision.gameObject.tag == "Ground" && Mathf.Abs(collision.contacts[0].normal.x) > 0.5f)
        {
            //Debug.Log("Jumpin");
            obj.transform.position = new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z);
        }
    }
}
