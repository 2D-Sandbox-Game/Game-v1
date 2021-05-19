using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    // Attak Variables 
    bool isAttacking = false;
    GameObject attackField;
    public GameObject sword;
    public float attackSpeed;
    float attackDuration;
    Animator swordAnim;

    // Start is called before the first frame update
    void Start()
    {
        swordAnim = sword.GetComponent<Animator>();
        attackField = sword.transform.GetChild(0).gameObject;

        swordAnim.speed = attackSpeed;
        attackDuration = 1 / attackSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // Attack 
        if (Input.GetButtonDown("Fire1") && !isAttacking)
        {
            isAttacking = true;
            swordAnim.Play("Attack");
            StartCoroutine(DoAttack());
        }        
    }

    IEnumerator DoAttack()
    {
        attackField.SetActive(true);
        //schwert.SetActive(true);
        yield return new WaitForSeconds(attackDuration);
        attackField.SetActive(false);
        swordAnim.Play("Idle");
        isAttacking = false;
    }
}
