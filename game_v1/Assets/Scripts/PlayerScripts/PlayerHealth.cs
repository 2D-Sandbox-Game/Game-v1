using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    // Health Variables
    public int health = 6;
    public Image[] lives;
    public Sprite fullHeart;
    public Sprite hollowHeart;
    public Sprite halfHeart;

    Rigidbody2D rb;
    Animator animator;
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
            health = 0;
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

    void HealthDamage(float damage)  // Laeuft nicht vollstaendig
    {
        animator.Play("PlayerDamage");
        if (damage < 0)
        {
            rb.AddForce(new Vector2(0, 10f), ForceMode2D.Impulse);

            damage = damage * -1;
        }
        else
        {
            rb.AddForce(new Vector2(0, 10f), ForceMode2D.Impulse);
            rb.velocity = new Vector2(5f, rb.velocity.y);
        }
        health -= (int)damage;

    }
}
