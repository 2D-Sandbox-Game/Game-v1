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
    public int damageGeist = 1;
    float waitTime = 0.5f;
    float elapsedTime = 0.0f;
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

    void ApplyDamage(float damage)
    {
        objectToDestroy.GetComponent<Direction>().enabled = false;
        if (objectToDestroy.GetComponent<Animator>() != null)
        {
            objectToDestroy.GetComponent<Animator>().enabled = false;
        }
        elapsedTime = 0;

        if (currentHealth > 0)
        {
            currentHealth -= damage;
            if(currentHealth / maxHealth < 0.3)
            {
                healthBar.GetComponent<Image>().color = Color.red;
            }
            else if(currentHealth / maxHealth <= 0.5)
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.transform.position.x < transform.position.x)
                collision.gameObject.SendMessage("HealthDamage", -damageGeist, SendMessageOptions.DontRequireReceiver);
            else
                collision.gameObject.SendMessage("HealthDamage", damageGeist, SendMessageOptions.DontRequireReceiver);

        }
    }
}
