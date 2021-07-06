using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthEnemy : MonoBehaviour
{
    public float currentHealth = 5; // Health of the enemy
    // Start is called before the first frame update
    public GameObject ObjectToDestroy;
    public int damageEnemy = 1;
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
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Destroy(ObjectToDestroy);
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.transform.position.x < transform.position.x)
                collision.gameObject.SendMessage("HealthDamage", -damageEnemy, SendMessageOptions.DontRequireReceiver);
            else
                collision.gameObject.SendMessage("HealthDamage", damageEnemy, SendMessageOptions.DontRequireReceiver);

        }
    }
}
