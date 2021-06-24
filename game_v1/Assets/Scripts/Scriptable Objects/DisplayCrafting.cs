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
    public GameObject inventoryPrefab;
    public GameObject craftingPreview;
    private GameObject craftingPreviewDisplay;
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
        for (int i = 0; i < craftingInventory.Container.Items.Length; i++)
        {
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.PointerClick, delegate { OnClick(obj); });

            itemsDisplayed.Add(obj, craftingInventory.Container.Items[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSelectedSlot();
    }
    public void OnDisable()
    {
        Destroy(craftingPreviewDisplay);
    }
    public void UpdateSelectedSlot()
    {
        foreach (KeyValuePair<GameObject, InventorySlot> slot in itemsDisplayed)
        {
            if (slot.Value.id >= 0)
            {
                slot.Key.SetActive(true);
                slot.Key.transform.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = craftingInventory.database.GetItem[slot.Value.item.id].uiDisplay;
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = slot.Value.amount == 1 ? "" : slot.Value.amount.ToString();
            }
            else
            {
                slot.Key.transform.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
                slot.Key.SetActive(false);
            }
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
        if ((itemsDisplayed.ContainsKey(obj)) && craftingPreviewDisplay == null)
        {
            craftingPreviewDisplay = Instantiate(craftingPreview, Vector3.zero , Quaternion.identity, obj.transform.parent);
            craftingPreviewDisplay.transform.position = obj.transform.position + new Vector3(0f, -5f,0f);
            int i = 0;
            foreach (RecipeComponent component in GetComponent<CraftingInventory>().ItemsToRecipe(itemsDisplayed[obj].item.id).recipeComponents)
            {
                craftingPreviewDisplay.transform.GetChild(i).GetChild(0).GetComponentInChildren<Image>().sprite = component.item.uiDisplay;
                craftingPreviewDisplay.transform.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = component.amountNeeded == 1 ? "" : component.amountNeeded.ToString();
                i++;
            }
        }
    }
    public void OnExit(GameObject obj)
    {
        Destroy(craftingPreviewDisplay);
    }
    public void OnClick(GameObject obj)
    {
        CraftingRecipe recipe = GetComponent<CraftingInventory>().ItemsToRecipe(itemsDisplayed[obj].id);
        GetComponent<CraftingInventory>().Craft(itemsDisplayed[obj].item.id);
        if (!GetComponent<CraftingInventory>().CanCraft(recipe))
        {
            Destroy(craftingPreviewDisplay);
        }
    }
}
