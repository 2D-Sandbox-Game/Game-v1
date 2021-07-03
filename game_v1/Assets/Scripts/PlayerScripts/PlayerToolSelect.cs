using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerToolSelect : MonoBehaviour
{
    public InventoryObject inventory;
    public GameObject pickaxe;
    public GameObject sword;
    public GameObject axe;

    // Update is called once per frame
    void Update()
    {
        if (inventory.Container.Items[inventory.selectedSlot].item.name.ToLower().Contains("sword"))
        {
            sword.SetActive(true);
            sword.GetComponent<SpriteRenderer>().sprite = inventory.Container.Items[inventory.selectedSlot].item.sprite;
            sword.transform.GetChild(0).GetComponent<SendDamageCollision>().damageValue = (int)inventory.Container.Items[inventory.selectedSlot].item.stats;
            GetComponent<PlayerAttack>().enabled = true;
        }
        else
        {
            sword.SetActive(false);
            GetComponent<PlayerAttack>().enabled = false;
        }
        if (inventory.Container.Items[inventory.selectedSlot].item.name.ToLower().Contains("axe") && !inventory.Container.Items[inventory.selectedSlot].item.name.ToLower().Contains("pick"))
        {
            axe.SetActive(true);
            axe.GetComponent<SpriteRenderer>().sprite = inventory.Container.Items[inventory.selectedSlot].item.sprite;
            axe.GetComponent<CutTrees>().miningSpeed = inventory.Container.Items[inventory.selectedSlot].item.stats;
        }
        else
        {
            axe.SetActive(false);
        }
        if (inventory.Container.Items[inventory.selectedSlot].item.name.ToLower().Contains("pickaxe"))
        {
            pickaxe.SetActive(true);
            pickaxe.GetComponent<SpriteRenderer>().sprite = inventory.Container.Items[inventory.selectedSlot].item.sprite;
            GetComponent<PlayerMine>().enabled = true;
            GetComponent<PlayerMine>().miningSpeed = inventory.Container.Items[inventory.selectedSlot].item.stats;
            GameObject.Find("Trees").GetComponent<MineSapling>().enabled = true;
            GameObject.Find("Trees").GetComponent<MineSapling>().miningSpeed = inventory.Container.Items[inventory.selectedSlot].item.stats;

        }
        else
        {
            pickaxe.SetActive(false);
            GetComponent<PlayerMine>().enabled = false;
            GameObject.Find("Trees").GetComponent<MineSapling>().enabled = false;
        }
    }
}
