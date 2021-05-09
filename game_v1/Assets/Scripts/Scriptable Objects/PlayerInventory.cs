using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
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
    public void Update()
    {
       
    }
    private void OnApplicationQuit() // clears the inventory after the game is quit
    {
        inventory.Container.Items.Clear();
    }
}
