using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    // Health Variables
    public static int health = 10;
    public Image[] lives;
    public Sprite fullHeart;
    public Sprite hollowHeart;
    public Sprite halfHeart;

    Rigidbody2D rb;
    public static Animator animator;
    bool IsGettingDamage;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Health
        if (health > lives.Length * 2)
            health = lives.Length * 2;
        else if (health < 0)
        {
            health = 0;
        }
        for (int i = 0; i < lives.Length; i++)
        {
            if (i < health / 2)
            {
                lives[i].sprite = fullHeart;
            }
            else
            {
                lives[i].sprite = hollowHeart;
            }
        }
        if (health % 2 != 0 && lives.Length > health / 2)
            lives[health / 2].sprite = halfHeart;
    }



    //void HealthDamage(float damage)  // in testing mode
    //{
    //    animator.Play("Damage");
    //    if (damage < 0)
    //    {
    //        //rb.AddForce(new Vector2(0, 10f), ForceMode2D.Impulse); //

    //        damage = damage * -1;
    //    }
    //    //else
    //    //{
    //    //    rb.AddForce(new Vector2(0, 10f), ForceMode2D.Impulse);
    //    //    rb.velocity = new Vector2(5f, rb.velocity.y);
    //    //}
    //    health -= (int)damage;

    //}

    void HealthDamage(GameObject enemy)  // in testing mode
    {
        float damage;

        if (!IsGettingDamage)
        {
            GetComponent<PlayerMovement>().enabled = false;

            if (enemy.GetComponent<HealthBar>())
            {
                damage = enemy.GetComponent<HealthBar>().damage;
            }
            else
            {
                damage = enemy.GetComponent<ArrowDamage>().damage;
            }

            health -= (int)damage;

            
            animator.Play("Damage");
            if (transform.position.x < enemy.transform.position.x)
            {
                rb.AddForce(new Vector2(-8f, 8f), ForceMode2D.Impulse); //
                //rb.velocity = new Vector2(-50f, rb.velocity.y);
                //rb.velocity = new Vector2(-50f, rb.velocity.y);
                //damage = damage * -1;
            }
            else
            {
                rb.AddForce(new Vector2(8f, 8f), ForceMode2D.Impulse);
                //rb.velocity = new Vector2(50f, rb.velocity.y);
            }
        }
    }

    void DamageAnimFinished()
    {
        GetComponent<PlayerMovement>().enabled = true;
        //animator.Play("Idle");
        animator.Play("Idle2");
        //IsGettingDamage = false;
        //Debug.Log("hhhhhhheeeeeeey");
    }

    void DamageAnimStarted()
    {
        //animator.enabled = false;
        //animator.enabled = true;

        //if (health == 0)
        //{
        //    animator.Play("Die");
        //}

        //IsGettingDamage = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GetComponent<PlayerMovement>().enabled = false;

        if (collision.gameObject.tag.Equals("Ground") && Fall_Damage.s_velocity < -30) // if the player falls more than 30 
        {
            PlayerHealth.animator.Play("Damage"); // Run animation of damage
            PlayerHealth.health += (int)(Fall_Damage.s_velocity + 27) / 3;
            Debug.Log(Fall_Damage.s_velocity);
            Fall_Damage.s_velocity = 0;
        }

        GetComponent<PlayerMovement>().enabled = true;

        if (collision.gameObject.tag != "Ground")
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>(), true);
        }
    }
}
