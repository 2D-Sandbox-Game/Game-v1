using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDie : MonoBehaviour
{
    Animator animator;
    Transform player;
    //public Animator swordAnim;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerHealth.health == 0)
        {
            animator.Play("Die");
            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<PlayerAttack>().enabled = false;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
        }

        //GetComponent<SpriteRenderer>().color = Color.blue;

    }

    void Die()
    {
        gameObject.SetActive(false);
        /*Generation G = new Generation();
        Generation.playerSpawn = G.SpawnPoint(new Vector3Int(0,(int)Generation.playerSpawn.y,0));
        Destroy(G);*/
        player.transform.position = PlayerSpawn.playerSpawn;
        player.rotation = new Quaternion(0, 0, 0, 0);
        PlayerHealth.health = 10;
        animator.Play("Idle3");
        //swordAnim.Play("Idle");
        gameObject.SetActive(true);
        GetComponent<PlayerMovement>().enabled = true;
        GetComponent<PlayerAttack>().enabled = true;
        //GetComponent<SpriteRenderer>().color = Color.white;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
