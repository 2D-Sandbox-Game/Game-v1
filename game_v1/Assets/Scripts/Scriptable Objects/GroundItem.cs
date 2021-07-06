using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundItem : MonoBehaviour
{
    // Script is added to the item in game that the player can pick up
    // Holds the item component
    public ItemObject item;
    // Prevents the collider from triggering twice
    public bool collected = false;

    public void Start()
    {
        // Correct image is set to display the item
        GetComponentInChildren<SpriteRenderer>().sprite = item.uiDisplay;
    }
}
