using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttack : MonoBehaviour
{
    public float speed;
    public GameObject arrow;
    public GameObject target;
    public SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = arrow.GetComponent<SpriteRenderer>();
        target = GameObject.Find("Player");
        if (transform.position.x > target.transform.position.x)
        {
            sr.flipX = true;            
        }
        else
        {
            sr.flipX = false;
            speed *= -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag != "Enemy")
            Destroy(arrow);
    }
}
