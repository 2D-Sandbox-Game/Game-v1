using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall_Damage : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.gameObject.tag.Equals("Ground")&&PlayerMovement.rb.velocity.y < -30)
        {
            PlayerHealth.animator.Play("PlayerDamage");
            PlayerHealth.health += (int)(PlayerMovement.rb.velocity.y + 27)/3;
            Debug.Log(PlayerMovement.rb.velocity.y);
        }
    }

}
