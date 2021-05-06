using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public InventoryObject inventory; // carries the player's inventory
    public GameObject createableItem;
    public ItemDatabaseObject database;
    
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
    Vector3 vec = new Vector3(2, 2);
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            for (int i = 0; i < database.Items.Length; i++)
            {
                if ("Dirt" == database.Items[i].name)
                {
                    Instantiate(createableItem, transform.localPosition + vec, Quaternion.identity);
                    createableItem.GetComponent<GroundItem>().item = database.Items[i];
                }
                if ("Potion" == database.Items[i].name)
                {
                    Instantiate(createableItem, transform.localPosition + vec, Quaternion.identity);
                    createableItem.GetComponent<GroundItem>().item = database.Items[i];
                }
            }
        }
    }
    private void OnApplicationQuit() // clears the inventory after the game is quit
    {
        inventory.Container.Items.Clear();
    }
}
