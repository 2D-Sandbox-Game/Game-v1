using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public ItemDatabaseObject database;
    public InventoryObject inventory;

    // Activates when the collider of the gameobject is triggered
    public void OnTriggerEnter2D(Collider2D other)
    {
        // Tries to get the item component of the triggering object
        var item = other.GetComponent<GroundItem>();
        if (item && !item.collected)
        {
            // Set to true so the item can't be picked up twice
            item.collected = true;
            // Adds item to the inventory
            inventory.AddItem(new Item(item.item), 1);
            // Deletes the item as it is now stored in the inventory
            Destroy(other.gameObject);
        }
    }
    public void Start()
    {
        // Initial items for the player are set
        inventory.AddItem(new Item(database.items[10]), 1);
        inventory.AddItem(new Item(database.items[11]), 1);
        inventory.AddItem(new Item(database.items[12]), 1);

        inventory.AddItem(new Item(database.items[6]), 50);
    }
    public void Update()
    {
        ChangeSelectedSlot();
    }
    // Clears the inventory after the game is quit
    private void OnApplicationQuit()
    {
        inventory.container.items = new InventorySlot[36];
    }

    // Changes the selected slot based upon player input
    public void ChangeSelectedSlot()
    {
        // Scroll input
        if (Input.mouseScrollDelta.y > 0)
        {
            if (inventory.selectedSlot < 8)
            {
                inventory.selectedSlot++;
            }
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            if (inventory.selectedSlot > 0)
            {
                inventory.selectedSlot--;
            }
        }
        // Numberkeys
        if (Input.GetKey(KeyCode.Alpha1))
        {
            inventory.selectedSlot = 0;
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            inventory.selectedSlot = 1;
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            inventory.selectedSlot = 2;
        }
        else if (Input.GetKey(KeyCode.Alpha4))
        {
            inventory.selectedSlot = 3;
        }
        else if (Input.GetKey(KeyCode.Alpha5))
        {
            inventory.selectedSlot = 4;
        }
        else if (Input.GetKey(KeyCode.Alpha6))
        {
            inventory.selectedSlot = 5;
        }
        else if (Input.GetKey(KeyCode.Alpha7))
        {
            inventory.selectedSlot = 6;
        }
        else if (Input.GetKey(KeyCode.Alpha8))
        {
            inventory.selectedSlot = 7;
        }
        else if (Input.GetKey(KeyCode.Alpha9))
        {
            inventory.selectedSlot = 8;
        }
    }
}
