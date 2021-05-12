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
    private int NumberOfColums = 9;
    public int ySpaceBetweenItems;
    Dictionary<GameObject, InventorySlot> itemsDisplayed = new Dictionary<GameObject, InventorySlot>();
    //Start is called before the first frame update
    void Start()
    {
        CreateDisplay();
    }
    public void CreateDisplay()
    {
        itemsDisplayed = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < inventory.Container.Items.Length; i++)
        {
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

            itemsDisplayed.Add(obj, inventory.Container.Items[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSelectedSlot();
    }
    public void UpdateSelectedSlot()
    {
        int idx = 0;
        foreach (KeyValuePair<GameObject, InventorySlot> slot in itemsDisplayed)
        {
            if (slot.Value.id >= 0)
            {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetItem[slot.Value.item.id].uiDisplay;
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = slot.Value.amount == 1 ? "" : slot.Value.amount.ToString();
                slot.Key.transform.GetComponent<Image>().color = new Color(1,1,1,1);
            }
            else
            {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
                slot.Key.transform.GetComponent<Image>().color = new Color(1,1,1,1);
            }
            if (idx == inventory.selectedSlot)
            {
                slot.Key.transform.GetComponent<Image>().color = new Color(0, 0, 1, 1);
            }
            idx++;
        }
    }
    public Vector3 GetPosition(int i)
    {
        return new Vector3(xStart + (xSpaceBetweenItems * (i % NumberOfColums)), yStart - (ySpaceBetweenItems * (i / NumberOfColums)), 0f);
    }

    //public void UpdateDisplay()
    //{
    //    foreach (KeyValuePair<GameObject, InventorySlot>  slot in itemsDisplayed)
    //    {
    //        if (slot.Value.id >= 0)
    //        {
    //            slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetItem[slot.Value.item.id].uiDisplay;
    //            slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
    //            slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = slot.Value.amount == 1 ? "" : slot.Value.amount.ToString();
    //        }
    //        else
    //        {
    //            slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
    //            slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
    //            slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
    //        }
    //    }
    //}

    //public void InstatiateItemInDisplay(int i, InventorySlot slot)
    //{
    //    var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
    //    obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetItem[slot.item.id].uiDisplay;
    //    obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
    //    obj.GetComponentInChildren<TextMeshProUGUI>().text = slot.amount.ToString();
    //    itemsDisplayed.Add(slot, obj);
    //}
}
