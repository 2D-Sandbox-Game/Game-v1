using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class DisplayInventory : MonoBehaviour
{
    public InventoryObject inventory;
    public ItemDatabaseObject database;
    public GameObject inventoryPrefab;
    public GroundItem creatableItem;
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

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });

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
                slot.Key.transform.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            }
            else
            {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
                slot.Key.transform.GetComponent<Image>().color = new Color(1, 1, 1, 1);
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

    private void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnter(GameObject obj)
    {
        MouseData.hoverObj = obj;
        if (itemsDisplayed.ContainsKey(obj))
        {
            MouseData.hoverItem = itemsDisplayed[obj];
        }
    }
    public void OnExit(GameObject obj)
    {
        MouseData.hoverObj = null;
        MouseData.hoverItem = null;
    }
    public void OnDragStart(GameObject obj)
    {
        var mouseObject = new GameObject();
        var rt = mouseObject.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(2.332f, 2.332f);
        mouseObject.transform.SetParent(transform.parent);
        if (itemsDisplayed[obj].id >= 0)
        {
            var img = mouseObject.AddComponent<Image>();
            img.sprite = inventory.database.GetItem[itemsDisplayed[obj].id].uiDisplay;
            img.raycastTarget = false;
        }
        MouseData.obj = mouseObject;
        MouseData.item = itemsDisplayed[obj];
    }
    public void OnDragEnd(GameObject obj)
    {
        if (MouseData.hoverObj)
        {
            inventory.MoveItem(itemsDisplayed[obj], itemsDisplayed[MouseData.hoverObj]);
        }
        else
        {
            //if (itemsDisplayed[obj] is InventorySlot inventorySlot && inventorySlot.id >= 0)
            //{               
            //    Transform t = GameObject.Find("Player").transform;
            //    creatableItem.GetComponent<GroundItem>().item = database.Items[inventorySlot.item.id];
                

            //    if (creatableItem.item.type == ItemType.Equipment)
            //    {
            //        Instantiate(creatableItem, t.position + new Vector3(3, 1, 0), Quaternion.identity);
            //        inventory.RemoveItem(inventorySlot.item);
            //    }
            //    else
            //    {
            //        for (int i = 0; i < inventorySlot.amount; i++)
            //        {
            //            Instantiate(creatableItem, t.position + new Vector3(3, 1, 0), Quaternion.identity);
            //        }
            //        inventory.RemoveItem(inventorySlot.item);
            //    }
            //}
            //drop item
            // unstable
        }
        Destroy(MouseData.obj);
        MouseData.item = null;
    }
    public void OnDrag(GameObject obj)
    {
        if (MouseData.obj != null)
        {
            MouseData.obj.GetComponent<RectTransform>().position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);
        }
    }

    public static class MouseData
    {
        public static GameObject obj;
        public static InventorySlot item;
        public static InventorySlot hoverItem;
        public static GameObject hoverObj;
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
