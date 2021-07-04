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
        if (Inventory.Container.Items[Inventory.selectedSlot].item.name.ToLower().Contains("sword"))
        {
            Sword.SetActive(true);
            Sword.GetComponent<SpriteRenderer>().sprite = Inventory.Container.Items[Inventory.selectedSlot].item.sprite;
            Sword.transform.GetChild(0).GetComponent<SendDamageCollision>().damageValue = (int)Inventory.Container.Items[Inventory.selectedSlot].item.stats;
            GetComponent<PlayerAttack>().enabled = true;
        }
        else
        {
            Sword.SetActive(false);
            GetComponent<PlayerAttack>().enabled = false;
        }

        // Activates / deactivates the sword object and the "CutTrees" script
        if (Inventory.Container.Items[Inventory.selectedSlot].item.name.ToLower().Contains("axe") && !Inventory.Container.Items[Inventory.selectedSlot].item.name.ToLower().Contains("pick"))
        {
            Axe.SetActive(true);
            Axe.GetComponent<SpriteRenderer>().sprite = Inventory.Container.Items[Inventory.selectedSlot].item.sprite;
            Axe.GetComponent<CutTrees>().MiningSpeed = Inventory.Container.Items[Inventory.selectedSlot].item.stats;
        }
        else
        {
            Axe.SetActive(false);
        }

        // Activates / deactivates the pickaxe object, the "PlayerMine" and the "MineSapling" script
        if (Inventory.Container.Items[Inventory.selectedSlot].item.name.ToLower().Contains("pickaxe"))
        {
            Pickaxe.SetActive(true);
            Pickaxe.GetComponent<SpriteRenderer>().sprite = Inventory.Container.Items[Inventory.selectedSlot].item.sprite;
            GetComponent<PlayerMine>().enabled = true;
            GetComponent<PlayerMine>().MiningSpeed = Inventory.Container.Items[Inventory.selectedSlot].item.stats;
            GameObject.Find("Trees").GetComponent<MineSapling>().enabled = true;
            GameObject.Find("Trees").GetComponent<MineSapling>().MiningSpeed = Inventory.Container.Items[Inventory.selectedSlot].item.stats;

        }
        else
        {
            Pickaxe.SetActive(false);
            GetComponent<PlayerMine>().enabled = false;
            GameObject.Find("Trees").GetComponent<MineSapling>().enabled = false;
        }
    }
}
