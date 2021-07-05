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
    // Existing template for the display of an inventory slot
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
        // Creates an empty dictionary that is used to connect the object in the game with the slot in the inventory
        itemsDisplayed = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < inventory.container.items.Length; i++)
        {
            // Creates the objects in the game foreach inventoryslot
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            // Changes the position
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

            // Adds the trigger for player interaction
            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });

            // Adds the in game object the according inventory slot
            itemsDisplayed.Add(obj, inventory.container.items[i]);
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
                // If a slot holds an item
                // Set the displayed image to the image linked to the item
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.idToItem[slot.Value.item.id].uiDisplay;
                // Image of the item is made visable
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                // Amount is displayed
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = slot.Value.amount == 1 ? "" : slot.Value.amount.ToString();
                // Backround is set to the default value
                slot.Key.transform.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            }
            else
            {
                // Image is set to null
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                // Default image is set to invisible
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                // Amount is set to none
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
                // Backround is set to the default value
                slot.Key.transform.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            }
            if (idx == inventory.selectedSlot)
            {
                // Selected slot gets highlighted by changing the color of the backround
                slot.Key.transform.GetComponent<Image>().color = new Color(0, 0, 1, 1);
            }
            idx++;
        }
    }
    public Vector3 GetPosition(int i)
    {
        // retruns the next position for a slot based upon varibales set in the editor
        return new Vector3(xStart + (xSpaceBetweenItems * (i % NumberOfColums)), yStart - (ySpaceBetweenItems * (i / NumberOfColums)), 0f);
    }

    private void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        // Assigns the EventTrigger of a slot to a variable
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        // Creates a new trigger on the object
        var eventTrigger = new EventTrigger.Entry();
        // Sets the type of the trigger
        eventTrigger.eventID = type;
        // Sets a delegate to the trigger
        eventTrigger.callback.AddListener(action);
        // Adds the trigger to triggers of the object
        trigger.triggers.Add(eventTrigger);
    }

    // If the mouse cursor enters the object
    public void OnEnter(GameObject obj)
    {
        MouseData.hoverObj = obj;
        // If the object is linked to an item
        if (itemsDisplayed.ContainsKey(obj))
        {
            // Hoveritem is set to the item linked to the game object
            MouseData.hoverItem = itemsDisplayed[obj];
        }
    }

    // If the mouse cursor exists the object
    public void OnExit(GameObject obj)
    {
        // All references are reset
        MouseData.hoverObj = null;
        MouseData.hoverItem = null;
    }

    // If the player starts dragging by holding mouse1
    public void OnDragStart(GameObject obj)
    {
        // Object that is displayed under the cursor is created
        var mouseObject = new GameObject();
        var rt = mouseObject.AddComponent<RectTransform>();
        // Size gets adjusted
        rt.sizeDelta = new Vector2(2.332f, 2.332f);
        // Mouseobject gets linked to the current object
        mouseObject.transform.SetParent(transform.parent);
        if (itemsDisplayed[obj].id >= 0)
        {
            // If the slot holds an item
            var img = mouseObject.AddComponent<Image>();
            // Image is set image of the item
            img.sprite = inventory.database.idToItem[itemsDisplayed[obj].id].uiDisplay;
            img.raycastTarget = false;
        }
        MouseData.obj = mouseObject;
        MouseData.item = itemsDisplayed[obj];
    }

    // If the player lets go of mouse1
    public void OnDragEnd(GameObject obj)
    {
        if (MouseData.hoverObj)
        {
            // If the player hovers over another inventory slot swap the items of these two slots
            inventory.MoveItem(itemsDisplayed[obj], itemsDisplayed[MouseData.hoverObj]);
        }
        else
        {
            // Item dropping feature
        }
        // Mouseobject is destroyed
        Destroy(MouseData.obj);
        MouseData.item = null;
    }

    // While dragging
    public void OnDrag(GameObject obj)
    {
        if (MouseData.obj != null)
        {
            // Position of the mouseobject is updated to be the position of the cursor
            MouseData.obj.GetComponent<RectTransform>().position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);
        }
    }

    // Static class that holds the variables for the display of dragged items
    public static class MouseData
    {
        public static GameObject obj;
        public static InventorySlot item;
        public static InventorySlot hoverItem;
        public static GameObject hoverObj;
    }
}
