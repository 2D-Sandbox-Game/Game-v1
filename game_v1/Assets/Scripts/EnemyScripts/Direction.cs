using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Direction : MonoBehaviour
{
    public Vector3 newPos;
    public Vector3 startpos;
    public SpriteRenderer sr;
    public GameObject go;
    Vector3 temp;
    public float speed;
    public Transform target;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        sr = go.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {   
        if (Vector2.Distance(transform.position, target.position) > 2)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            animator.SetFloat("Speed", Mathf.Abs(speed));
            if (transform.position.x > target.position.x)
            {
                sr.flipX = true;
            }
            else
            {
                sr.flipX = false;
            }
            temp = newPos;
        }       
    }
}
