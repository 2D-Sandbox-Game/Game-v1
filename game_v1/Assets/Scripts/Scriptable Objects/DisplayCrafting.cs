using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class DisplayCrafting : MonoBehaviour
{
    public InventoryObject craftingInventory;
    // Existing template for the display of an inventory slot
    public GameObject inventoryPrefab;
    public GameObject craftingPreview;
    // Secondary display that shows the items required for a crafting recipe
    private GameObject _craftingPreviewDisplay;
    public int xStart;
    public int yStart;
    public int xSpaceBetweenItems;
    private int _numberOfColums = 9;
    public int ySpaceBetweenItems;
    private Dictionary<GameObject, InventorySlot> _craftableItemsOnDisplay = new Dictionary<GameObject, InventorySlot>();

    //Start is called before the first frame update
    void Start()
    {
        CreateDisplay();
    }
    public void CreateDisplay()
    {
        // Creates an empty dictionary that is used to connect the object in the game with the slot in the inventory
        _craftableItemsOnDisplay = new Dictionary<GameObject, InventorySlot>();

        for (int i = 0; i < craftingInventory.container.items.Length; i++)
        {
            // Creates the objects in the game foreach inventoryslot
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            // Changes the position
            obj.GetComponent<RectTransform>().localPosition = GetPositionForSlot(i);

            // Adds the trigger for player interaction
            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.PointerClick, delegate { OnClick(obj); });

            // Adds the in game object the according inventory slot
            _craftableItemsOnDisplay.Add(obj, craftingInventory.container.items[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDisplay();
    }
    public void OnDisable()
    {
        // If inventory is disables delete the secondary display
        Destroy(_craftingPreviewDisplay);
    }
    public void UpdateDisplay()
    {
        foreach (KeyValuePair<GameObject, InventorySlot> slot in _craftableItemsOnDisplay)
        {
            if (slot.Value.id >= 0)
            {
                // If a slot holds an item
                // All interactions with the object are enabled 
                slot.Key.SetActive(true);
                // Template is made visable
                slot.Key.transform.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                // Set the displayed image to the image linked to the item
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = craftingInventory.database.idToItem[slot.Value.item.id].uiDisplay;
                // Image of the item is made visable
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                // Amount is displayed
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = slot.Value.amount == 1 ? "" : slot.Value.amount.ToString();
            }
            else
            {
                // If a slot does not hold an item
                // Object gets set to invisible
                slot.Key.transform.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                // Image is set to null
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                // Defaultimage is set to invisible
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                // Amount is set to none
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
                // All interactions with the object are disabled  
                slot.Key.SetActive(false);
            }
        }
    }
    public Vector3 GetPositionForSlot(int i)
    {
        // retruns the next position for a slot based upon varibales set in the editor
        return new Vector3(xStart + (xSpaceBetweenItems * (i % _numberOfColums)), yStart - (ySpaceBetweenItems * (i / _numberOfColums)), 0f);
    }

    private void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        // Assigns the EventTrigger of a slot to a variable
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        // Creates a new trigger on the object
        var eventTrigger = new EventTrigger.Entry();
        // Sets the type of the tripper
        eventTrigger.eventID = type;
        // Sets a delegate to the trigger
        eventTrigger.callback.AddListener(action);
        // Adds the trigger to triggers of the object
        trigger.triggers.Add(eventTrigger);
    }

    // If the mouse cursor enters the object
    public void OnEnter(GameObject obj)
    {
        // If the object is linked to an item
        if ((_craftableItemsOnDisplay.ContainsKey(obj)) && _craftingPreviewDisplay == null)
        {
            // Crafting preview panel is created
            _craftingPreviewDisplay = Instantiate(craftingPreview, Vector3.zero , Quaternion.identity, obj.transform.parent);
            // Position is set
            _craftingPreviewDisplay.transform.position = obj.transform.position + new Vector3(0f, -5f,0f);
            int i = 0;
            foreach (RecipeComponent component in GetComponent<CraftingInventory>().itemsToRecipe[_craftableItemsOnDisplay[obj].item.id].recipeComponents)
            {
                // Image of crafting component is set in the preview panel
                _craftingPreviewDisplay.transform.GetChild(i).GetChild(0).GetComponentInChildren<Image>().sprite = component.item.uiDisplay;
                // Amount is set
                _craftingPreviewDisplay.transform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = component.amountRequired == 1 ? "" : component.amountRequired.ToString();
                i++;
            }
        }
    }

    // If the mouse cursor exists the object
    public void OnExit(GameObject obj)
    {
        // Preview is destroyed
        Destroy(_craftingPreviewDisplay);
    }

    // If the object is clicked
    public void OnClick(GameObject obj)
    {
        // Gets the recipe for the item displayed by the object
        CraftingRecipe recipe = GetComponent<CraftingInventory>().itemsToRecipe[_craftableItemsOnDisplay[obj].id];
        // CraftItem is called
        GetComponent<CraftingInventory>().CraftItem(_craftableItemsOnDisplay[obj].item.id);
        // If the item can't be crafted again the Preview is destroyed
        if (!GetComponent<CraftingInventory>().CanCraft(recipe))
        {
            Destroy(_craftingPreviewDisplay);
        }
    }
}
