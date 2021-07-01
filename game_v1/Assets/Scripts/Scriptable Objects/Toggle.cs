using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toggle : MonoBehaviour
{
    GameObject craftingPanel;
    // Start is called before the first frame update
    void Start()
    {
        craftingPanel = GameObject.Find("CraftingPanel");
    }

    // Update is called once per frame
    void Update()
    {
        ToggleCrafting();
    }
    public void ToggleCrafting()
    {
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
