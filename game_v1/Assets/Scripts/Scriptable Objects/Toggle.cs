using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toggle : MonoBehaviour
{
    GameObject craftingPanel;

    // Start is called before the first frame update
    void Start()
    {
        // Finds the crafting panel in the scene
        craftingPanel = GameObject.Find("CraftingPanel");
    }

    // Update is called once per frame
    void Update()
    {
        ToggleCrafting();
    }
    public void ToggleCrafting()
    {
        // Enables the player to toggle the crafting display on and off
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (craftingPanel.active == true)
            {
                craftingPanel.SetActive(false);
            }
            else
            {
                craftingPanel.SetActive(true);
            }
        }
    }
}
