using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerToolSelect : MonoBehaviour
{
    public InventoryObject inventory;
    public GameObject pickaxe;
    public GameObject sword;
    public GameObject axe;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inventory.Container.Items[inventory.selectedSlot].id == 11)
        {
            pickaxe.SetActive(true);
            GetComponent<PlayerMine>().enabled = true;
        }
        else
        {
            pickaxe.SetActive(false);
            GetComponent<PlayerMine>().enabled = false;
        }

        if (inventory.Container.Items[inventory.selectedSlot].id == 10)
        {
            sword.SetActive(true);
            GetComponent<PlayerAttack>().enabled = true;
        }
        else
        {
            sword.SetActive(false);
            GetComponent<PlayerAttack>().enabled = false;
        }

        if (inventory.Container.Items[inventory.selectedSlot].id == 12)
        {
            axe.SetActive(true);
            //GetComponent<PlayerAttack>().enabled = true;
        }
        else
        {
            axe.SetActive(false);
            //GetComponent<PlayerAttack>().enabled = false;
        }
    }
}
