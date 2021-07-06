using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathfindingGround : MonoBehaviour
{
    public float speed;
    public Transform Target;
    public Animator animator;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, Target.position) > 2)
        {
            transform.position = Vector2.MoveTowards(transform.position, Target.position, speed * Time.deltaTime);
            animator.SetFloat("Speed", Mathf.Abs(speed));
        }
    }
}
