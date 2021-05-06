using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplayInventory : MonoBehaviour
{
    public InventoryObject inventory;

    public GameObject inventoryPrefab;
    public int xStart;
    public int yStart;
    public int xSpaceBetweenItems;
    private int NumberOfColums = 10;
    public int ySpaceBetweenItems;
    Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();
    //Start is called before the first frame update
    void Start()
    {
        CreateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDisplay();
    }
    public void UpdateDisplay()
    {
        for (int i = 0; i < inventory.Container.Items.Count; i++)
        {
            InventorySlot slot = inventory.Container.Items[i];

            if (itemsDisplayed.ContainsKey(slot))
            {
                itemsDisplayed[slot].GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString();
            }
            else
            {
                InstatiateItemInDisplay(i, slot);
            }
        }
    }
    public void CreateDisplay()
    {
        for (int i = 0; i < inventory.Container.Items.Count; i++)
        {
            InventorySlot slot = inventory.Container.Items[i];
            InstatiateItemInDisplay(i, slot);
        }
    }

    public void InstatiateItemInDisplay(int i, InventorySlot slot)
    {
        var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
        obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetItem[slot.item.id].uiDisplay;
        obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
        obj.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString();
        itemsDisplayed.Add(slot, obj);
    }

    public Vector3 GetPosition(int i)
    {
        return new Vector3(xStart + (xSpaceBetweenItems * (i % NumberOfColums)), yStart - (ySpaceBetweenItems * (i / NumberOfColums)), 0f);
    }
}
