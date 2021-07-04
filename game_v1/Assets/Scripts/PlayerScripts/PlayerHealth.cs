using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    // Health Variables
    public static int s_health = 10;
    public static Animator s_animator;
    public Image[] lives;
    public Sprite fullHeart;
    public Sprite hollowHeart;
    public Sprite halfHeart;
    Rigidbody2D _rb;
    bool _IsGettingDamage;

    // Start is called before the first frame update
    void Start()
    {
        s_animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        // Display correction of health points
        if (s_health > lives.Length * 2)
        {
            s_health = lives.Length * 2;
        }
        else if (s_health < 0)
        {
            s_health = 0;
        }
        for (int i = 0; i < lives.Length; i++)
        {
            if (i < s_health / 2)
            {
                lives[i].sprite = fullHeart;
            }
            else
            {
                lives[i].sprite = hollowHeart;
            }
        }
        if (s_health % 2 != 0 && lives.Length > s_health / 2)
        {
            lives[s_health / 2].sprite = halfHeart;
        }
    }
    // Function do descrease in health of player and run animation of damage
    void DamageHealth(GameObject enemy) 
    {
        float damage;
        if (!_IsGettingDamage)
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
            s_health -= (int)damage;
            s_animator.Play("Damage");
            if (transform.position.x < enemy.transform.position.x)
            {
                _rb.AddForce(new Vector2(-8f, 8f), ForceMode2D.Impulse); 
            }
            else
            {
                _rb.AddForce(new Vector2(8f, 8f), ForceMode2D.Impulse);
            }
        }
    }
    // Function stops animation of damage
    void FinishDamageAnimation()
    {
        GetComponent<PlayerMovement>().enabled = true;
        s_animator.Play("Idle2");
    }
    // Fall damage function
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GetComponent<PlayerMovement>().enabled = false;
        // if the player falls more than 30 points he get a damage
        if (collision.gameObject.tag.Equals("Ground") && Fall_Damage.s_velocity < -30) 
        {
             // Run animation of damage
            PlayerHealth.s_animator.Play("Damage");
            PlayerHealth.s_health += (int)(Fall_Damage.s_velocity + 27) / 3;
            Fall_Damage.s_velocity = 0;
        }
        GetComponent<PlayerMovement>().enabled = true;
        if (collision.gameObject.tag != "Ground")
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>(), true);
        }
    }
}
