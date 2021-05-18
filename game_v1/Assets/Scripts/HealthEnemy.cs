using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthEnemy : MonoBehaviour
{
    public float currentHealth = 5; //Leben und Methode zum Schaden erhalten
    // Start is called before the first frame update
    public GameObject objectToDestroy;
    public int damageGeist=1;
    void Start()
    {
        
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
        }
        if(currentHealth <= 0)
        {
                currentHealth = 0;
                Destroy(objectToDestroy);
        }
    }

        private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Debug.Log(collision.transform.position.x +" | "+ transform.position.x);
            if(collision.transform.position.x < transform.position.x)
                collision.gameObject.SendMessage("HealthDamage", -damageGeist, SendMessageOptions.DontRequireReceiver);
            else 
                collision.gameObject.SendMessage("HealthDamage", damageGeist, SendMessageOptions.DontRequireReceiver);

        }
    }
}
