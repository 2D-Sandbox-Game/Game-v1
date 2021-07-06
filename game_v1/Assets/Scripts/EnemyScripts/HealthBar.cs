using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public GameObject healthBar;
    public GameObject healthBarBackground;
    public float maxHealth;
    public float currentHealth;
    public GameObject objectToDestroy;
    public float damage = 1;
    float waitTime = 0.5f;
    float elapsedTime = 0.0f;
    bool doAttack = true;
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.GetComponent<Image>().color = Color.green;
    }

    // Update is called once per frame
    void Update()
    {

        if ((elapsedTime >= waitTime) && (objectToDestroy.GetComponent<Direction>().enabled == false))
        {
            objectToDestroy.GetComponent<Direction>().enabled = true;
            if (objectToDestroy.GetComponent<Animator>() != null)
            {
                objectToDestroy.GetComponent<Animator>().enabled = true;
            }
        }
        elapsedTime += Time.deltaTime;
    }

    void ApplyDamage(GameObject player)
    {
        float damage = player.GetComponent<SendDamageCollision>().damageValue;

        objectToDestroy.GetComponent<Direction>().enabled = false;
        if (objectToDestroy.GetComponent<Animator>() != null)
        {
            objectToDestroy.GetComponent<Animator>().enabled = false;
        }
        elapsedTime = 0;

        if (currentHealth > 0)
        {
            currentHealth -= damage;
            if (currentHealth / maxHealth < 0.3)
            {
                healthBar.GetComponent<Image>().color = Color.red;
            }
            else if (currentHealth / maxHealth <= 0.5)
            {
                healthBar.GetComponent<Image>().color = Color.yellow;
            }

        }
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Destroy(objectToDestroy);
            Destroy(healthBar);
            Destroy(healthBarBackground);
        }
        healthBar.GetComponent<Image>().fillAmount = currentHealth / 10f;

        
        if (GetComponent<Rigidbody2D>())
        {
            if (transform.position.x < player.transform.position.x)
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(-8f, 8), ForceMode2D.Impulse); 
            }
            else
            {
                GetComponent<Rigidbody2D>().AddForce(new Vector2(8f, 8), ForceMode2D.Impulse);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Ground")
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>(), true);
        }
    }
}
