using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerToolSelect : MonoBehaviour
{
    public InventoryObject inventory;
    public GameObject pickaxe;
    public GameObject sword;
    public GameObject axe;
    //public Sprite[] sprites;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

        if (inventory.container.items[inventory.selectedSlot].item.name.ToLower().Contains("sword"))
        {
            sword.SetActive(true);
            sword.GetComponent<SpriteRenderer>().sprite = inventory.container.items[inventory.selectedSlot].item.uiDisplay;
            sword.transform.GetChild(0).GetComponent<SendDamageCollision>().damageValue = (int)inventory.container.items[inventory.selectedSlot].item.attribute;
            GetComponent<PlayerAttack>().enabled = true;
        }
        else
        {
            sword.SetActive(false);
            GetComponent<PlayerAttack>().enabled = false;
        }

        if (inventory.container.items[inventory.selectedSlot].item.name.ToLower().Contains("axe") && !inventory.container.items[inventory.selectedSlot].item.name.ToLower().Contains("pick"))
        {
            axe.SetActive(true);
            axe.GetComponent<SpriteRenderer>().sprite = inventory.container.items[inventory.selectedSlot].item.uiDisplay;
            axe.GetComponent<CutTrees>().miningSpeed = inventory.container.items[inventory.selectedSlot].item.attribute;
        }
        else
        {
            axe.SetActive(false);
        }


        if (inventory.container.items[inventory.selectedSlot].item.name.ToLower().Contains("pickaxe"))
        {
            pickaxe.SetActive(true);
            pickaxe.GetComponent<SpriteRenderer>().sprite = inventory.container.items[inventory.selectedSlot].item.uiDisplay;
            GetComponent<PlayerMine>().enabled = true;
            GetComponent<PlayerMine>().miningSpeed = inventory.container.items[inventory.selectedSlot].item.attribute;
            GameObject.Find("Trees").GetComponent<MineSapling>().enabled = true;
            GameObject.Find("Trees").GetComponent<MineSapling>().miningSpeed = inventory.container.items[inventory.selectedSlot].item.attribute;

        }
        else
        {
            pickaxe.SetActive(false);
            GetComponent<PlayerMine>().enabled = false;
            GameObject.Find("Trees").GetComponent<MineSapling>().enabled = false;
        }

    }

}
