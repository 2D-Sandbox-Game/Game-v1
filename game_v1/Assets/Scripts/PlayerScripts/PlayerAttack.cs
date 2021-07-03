using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    // Attak Variables 
    bool _isAttacking = false;
    float _attackDuration;
    Animator _swordAnim;
    GameObject _attackField;
    public GameObject sword;
    public float attackSpeed;

    // Start is called before the first frame update
    void Start()
    {
        _swordAnim = sword.GetComponent<Animator>();
        _attackField = sword.transform.GetChild(0).gameObject;

        _swordAnim.speed = attackSpeed;
        _attackDuration = 1 / attackSpeed;
    }
    // Update is called once per frame
    void Update()
    {
        // Attack 
        if (Input.GetButtonDown("Fire1") && !_isAttacking)
        {
            _isAttacking = true;
            _swordAnim.Play("Attack");
            StartCoroutine(DoAttack());
        }        
    }
    IEnumerator DoAttack()
    {
        _attackField.SetActive(true);
        yield return new WaitForSeconds(_attackDuration);
        _attackField.SetActive(false);
        _swordAnim.Play("Idle");
        _isAttacking = false;
    }
}
