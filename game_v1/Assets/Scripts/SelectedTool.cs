using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedTool : MonoBehaviour
{
    public InventoryObject inventory;
    public GameObject pickaxe;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inventory.Container.Items[inventory.selectedSlot].id == 3)
        {
            pickaxe.SetActive(true);
            GetComponent<PlayerMine>().enabled = true;
        }
        else
        {
            pickaxe.SetActive(false);
            GetComponent<PlayerMine>().enabled = false;
        }
    }
}
