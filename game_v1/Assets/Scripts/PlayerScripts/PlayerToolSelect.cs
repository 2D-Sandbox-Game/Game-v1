using UnityEngine;

public class PlayerToolSelect : MonoBehaviour
{
    // Public variables
    public InventoryObject Inventory;
    public GameObject Pickaxe;
    public GameObject Sword;
    public GameObject Axe;

    // Update is called once per frame
    void Update()
    {
        // Activates / deactivates the sword object and the "PlayerAttack" script
        if (Inventory.container.items[Inventory.selectedSlot].item.name.ToLower().Contains("sword"))
        {
            Sword.SetActive(true);
            Sword.GetComponent<SpriteRenderer>().sprite = Inventory.container.items[Inventory.selectedSlot].item.uiDisplay;
            Sword.transform.GetChild(0).GetComponent<SendDamageCollision>().damageValue = (int)Inventory.container.items[Inventory.selectedSlot].item.attribute;
            GetComponent<PlayerAttack>().enabled = true;
        }
        else
        {
            Sword.SetActive(false);
            GetComponent<PlayerAttack>().enabled = false;
        }

        // Activates / deactivates the sword object and the "CutTrees" script
        if (Inventory.container.items[Inventory.selectedSlot].item.name.ToLower().Contains("axe") && !Inventory.container.items[Inventory.selectedSlot].item.name.ToLower().Contains("pick"))
        {
            Axe.SetActive(true);
            Axe.GetComponent<SpriteRenderer>().sprite = Inventory.container.items[Inventory.selectedSlot].item.uiDisplay;
            Axe.GetComponent<CutTrees>().MiningSpeed = Inventory.container.items[Inventory.selectedSlot].item.attribute;
        }
        else
        {
            Axe.SetActive(false);
        }

        // Activates / deactivates the pickaxe object, the "PlayerMine" and the "MineSapling" script
        if (Inventory.container.items[Inventory.selectedSlot].item.name.ToLower().Contains("pickaxe"))
        {
            Pickaxe.SetActive(true);
            Pickaxe.GetComponent<SpriteRenderer>().sprite = Inventory.container.items[Inventory.selectedSlot].item.uiDisplay;
            GetComponent<PlayerMine>().enabled = true;
            GetComponent<PlayerMine>().MiningSpeed = Inventory.container.items[Inventory.selectedSlot].item.attribute;
            GameObject.Find("Trees").GetComponent<MineSapling>().enabled = true;
            GameObject.Find("Trees").GetComponent<MineSapling>().MiningSpeed = Inventory.container.items[Inventory.selectedSlot].item.attribute;

        }
        else
        {
            Pickaxe.SetActive(false);
            GetComponent<PlayerMine>().enabled = false;
            GameObject.Find("Trees").GetComponent<MineSapling>().enabled = false;
        }
    }
}
