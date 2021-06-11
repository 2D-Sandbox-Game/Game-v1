using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public GameObject healthBar;
    public GameObject healthBarBackground;
    public float maxHealth;
    public float currentHealth; //Leben und Methode zum Schaden erhalten
    // Start is called before the first frame update
    public GameObject objectToDestroy;
    public int damageGeist = 1;
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.GetComponent<Image>().color = Color.green;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ApplyDamage(float damage)
    {
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
            //Debug.Log(collision.transform.position.x +" | "+ transform.position.x);
            if (collision.transform.position.x < transform.position.x)
                collision.gameObject.SendMessage("HealthDamage", -damageGeist, SendMessageOptions.DontRequireReceiver);
            else
                collision.gameObject.SendMessage("HealthDamage", damageGeist, SendMessageOptions.DontRequireReceiver);

        }

        //Debug.Log(collision.gameObject.tag);
    }
}
