using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    float curentSpeed = 1;
    public float movementSpeed = 10;
    public float JumpForce = 1;
    
    public static Rigidbody2D rb;
    Animator animator;
    float mx;
    float prevMx = 1;

    // Start is called before the first frame update

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //if (Input.mousePosition.x - Screen.width / 2 < 0)
        //{
        //    this.transform.localScale = new Vector3(-1, 1, 1);
        //}
        //else
        //{
        //    this.transform.localScale = new Vector3(1, 1, 1);
        //}

        mx = Input.GetAxisRaw("Horizontal");
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {
            if(curentSpeed < movementSpeed)
                curentSpeed+=Time.deltaTime*10;
        }
        else    
            curentSpeed = 1;

        
        animator.SetFloat("Speed", Mathf.Abs(mx * curentSpeed));
        if (mx != prevMx && mx != 0)
        {
            if (mx < 0 )
            {
                transform.localScale = new Vector3(-1,1,1);
            }
            else
            {
                transform.localScale = new Vector3(1,1,1);
            }

            //Debug.Log($"mx: {mx}");

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
        Vector2 movement = new Vector2(mx * curentSpeed, rb.velocity.y);
        rb.velocity = movement;
    }

}
