using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 10;
    public float JumpForce = 1;
    public Rigidbody2D rb;
    public Animator animator;

    float mx;
    float prevMx = 1;

    private void Update()
    {
        mx = Input.GetAxisRaw("Horizontal");
        animator.SetFloat("Speed", Mathf.Abs(mx * movementSpeed));

        if (mx != prevMx && mx != 0)
        {
            if (mx == -1)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }

            Debug.Log($"mx: {mx}");

            prevMx = mx;
        }

        if (Input.GetButtonDown("Jump") && Mathf.Abs(rb.velocity.y) < 0.001f)
        {
            rb.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
            animator.SetBool("IsJumping", true);
        }

        if (Mathf.Abs(rb.velocity.y) < 0.001f)
        {
            animator.SetBool("IsJumping", false);
        }
    }

    private void FixedUpdate()
    {
        Vector2 movement = new Vector2(mx * movementSpeed, rb.velocity.y);
        rb.velocity = movement;
    }
}
