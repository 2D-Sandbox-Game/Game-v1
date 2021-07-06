using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Direction : MonoBehaviour
{
    public Vector3 newPos;
    public Vector3 startpos;
    SpriteRenderer Sr;
    public GameObject go;
    Vector3 temp;
    public float speed;
    public Transform Target;
    public Animator animator;
    public bool pathBlocked;
    public int dir = 1;
    public float range = 0;
    // Start is called before the first frame update
    void Start()
    {
        Sr = go.GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, Target.position) > range && !pathBlocked)
        {
            transform.position = Vector2.MoveTowards(transform.position, Target.position, speed * Time.deltaTime);

            if (animator)
            {
                animator.SetFloat("Speed", Mathf.Abs(speed));
            }

            
        }

        if (transform.position.x > Target.position.x) // Enemies are flipped so that they always run forward
        {
            transform.localScale = new Vector3(-1, 1, 1);
            dir = -1;
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
            dir = 1;
        }
    }
}