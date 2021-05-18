using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement2 : MonoBehaviour
{
    bool runFaster = false;
    float curentSpeed = 1;
    public float movementSpeed = 10;
    public float JumpForce = 1;
    public Rigidbody2D rb;
    public Animator animator;

    float mx, my;
    float prevMx = 1;
    bool spriteChanged = false;

// Attak Variables 
    bool isAttacking = false;
    [SerializeField]
    GameObject attackField; 
    [SerializeField]
    GameObject schwert;
// Health Variables
    public int health = 6;
    public Image [] lives;
    public Sprite fullHeart;
    public Sprite hollowHeart;
    public Sprite halfHeart;
// Damage Variables
    int enemySite;
    // Start is called before the first frame update


    private void Start()
    {
        attackField.SetActive(false);
        schwert.SetActive(false);
    }

    private void Update()
    {
        // Health
        if(health > lives.Length * 2)
            health = lives.Length * 2;
        else if(health<0)
            health = 0;
        for(int i =0; i<lives.Length; i++)
        {
            if(i<health/2)
            {
                lives[i].sprite = fullHeart;
            } else 
            {
                lives[i].sprite = hollowHeart;
            }
        }
        if(health % 2 != 0 && lives.Length > health/2)
            lives[health/2].sprite = halfHeart;

        // Attack 
        if(Input.GetButtonDown("Fire1") && !isAttacking)
        {
            isAttacking = true;
            animator.Play("attack_beta");
            StartCoroutine(DoAttack());

            // Aenderung der Richtung von Attack

            if (Input.mousePosition.x - Screen.width/2 <  0)
            {
                this.transform.localScale = new Vector3(-1,1,1);
            }
            else
            {
                this.transform.localScale = new Vector3(1,1,1);
            }

        }
        // End of Attack

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

    IEnumerator DoAttack()
    {
        attackField.SetActive(true);
        schwert.SetActive(true);
        yield return new WaitForSeconds(.1f);
        attackField.SetActive(false);
        schwert.SetActive(false);
        isAttacking = false;
    }

    void HealthDamage(float damage)  // Laeuft nicht vollstaendig
    {        
        animator.Play("PlayerDamage");
        if(damage<0)
        {
            rb.AddForce(new Vector2(0,10f), ForceMode2D.Impulse);

            damage = damage * -1;
        } else
        {
            rb.AddForce(new Vector2(0, 10f), ForceMode2D.Impulse);
            rb.velocity = new Vector2(5f, rb.velocity.y);
        }
        health -= (int)damage;

    }
}
