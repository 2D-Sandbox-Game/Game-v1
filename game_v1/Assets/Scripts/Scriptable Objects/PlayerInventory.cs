using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public ItemDatabaseObject database;
    public InventoryObject inventory; // carries the player's inventory
    /// <summary>
    /// On collision add item to players inventory
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter2D(Collider2D other) // activates when the collider is triggered
    {
        var item = other.GetComponent<GroundItem>(); // tries to get the item component of the triggering object
        if (item)
        {
            inventory.AddItem(new Item(item.item), 1); // adds item to the inventory
            Destroy(other.gameObject); // deletes the item as it is now in the inventory
        }
    }
    public void Start()
    {
        inventory.AddItem(new Item(database.Items[3]), 1);
        inventory.AddItem(new Item(database.Items[4]), 1);
    }
    public void Update()
    {
        ChangeSelectedSlot();
    }
    private void OnApplicationQuit() // clears the inventory after the game is quit
    {
        inventory.Container.Items = new InventorySlot[36];
    }
    public void ChangeSelectedSlot()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            inventory.selectedSlot++;
            inventory.selectedSlot = inventory.selectedSlot % 9;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            inventory.selectedSlot--;
            inventory.selectedSlot = (inventory.selectedSlot + 9) % 9;
        }
    }
}
