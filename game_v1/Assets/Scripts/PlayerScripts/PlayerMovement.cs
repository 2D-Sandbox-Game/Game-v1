using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    float _curentSpeed = 1;
    bool _godModeActivated;
    Animator _animator;
    public float movementSpeed = 10;
    public float JumpForce = 1;
    public static Rigidbody2D s_rb;
    public float mx;

    // Start is called before the first frame update
    // Getting components that are for movements needed
    private void Start()
    {
        _animator = GetComponent<Animator>();
        s_rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            _godModeActivated = !_godModeActivated;
            GetComponent<Rigidbody2D>().simulated = !GetComponent<Rigidbody2D>().simulated;
        }

        if (_godModeActivated)
        {
            EnterGodMode();
        }
        else
        {
            EnterNormalMode();
        }
    }
    private void FixedUpdate()
    {
        if(!_animator.GetBool("Damage"))
        {
            Vector2 movement = new Vector2(mx * _curentSpeed, s_rb.velocity.y);
            s_rb.velocity = movement;
        }
    }

    // Function move the player in normal game mode
    void EnterNormalMode()
    {
        mx = Input.GetAxisRaw("Horizontal");

        // Exponential growth of speed of players moving
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {
            if (_curentSpeed < movementSpeed)
            {
                _curentSpeed += Time.deltaTime * 10;
            }
        }
        else
        {
            _curentSpeed = 1;
        }
        _animator.SetFloat("Speed", Mathf.Abs(mx * _curentSpeed));

        // Movimg of player
        if (mx != 0)
        {
            if (mx < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
        // Jumping of Player
        if (Input.GetButtonDown("Jump") && Mathf.Abs(s_rb.velocity.y) < 0.001f)
        {
            s_rb.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
            _animator.SetBool("IsJumping", true);
        }
        if (Mathf.Abs(s_rb.velocity.y) < 0.001f)
        {
            _animator.SetBool("IsJumping", false);
        }
    }
    // Function let to move player in the world out the game mode
    void EnterGodMode()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(1, 0, 0);
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(0, 1, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += new Vector3(0, -1, 0);
        }
    }
}
