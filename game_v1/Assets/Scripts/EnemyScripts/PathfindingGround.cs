using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathfindingGround : MonoBehaviour
{
    public float speed;
    public Transform target;
    public CharacterController controller;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, target.position) > 2)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
        controller = GetComponent<CharacterController>();
        controller.stepOffset = 2.0f;
    }
    /*private void OnCollisionEnter2D(Collision2D col)
    {
        foreach (ContactPoint2D cp in col.contacts)
        {
            if(cp.collider == myCol)
            {
                controller = GetComponent<CharacterController>();
                controller.stepOffset = 2.0f;
            }
        }
    }*/
}
