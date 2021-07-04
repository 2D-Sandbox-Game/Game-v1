using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDie : MonoBehaviour
{
    Animator _animator;
    Transform _player;

    // Getting of the components that are needed for death of player
    void Start()
    {
        _animator = GetComponent<Animator>();
        _player = GetComponent<Transform>();
    }
    // Update is called once per frame
    // Function kill the player if he has no more life points
    void Update()
    {
        if (PlayerHealth.s_health == 0)
        {
            _animator.Play("Die");
            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<PlayerAttack>().enabled = false;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
        }
    }
    // Function of players respawn
    void DiePlayer()
    {
        gameObject.SetActive(false);
        _player.transform.position = PlayerSpawn.s_playerSpawn;
        _player.rotation = new Quaternion(0, 0, 0, 0);
        PlayerHealth.s_health = 10;
        _animator.Play("Idle3");
        gameObject.SetActive(true);
        GetComponent<PlayerMovement>().enabled = true;
        GetComponent<PlayerAttack>().enabled = true;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
