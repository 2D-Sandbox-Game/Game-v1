using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

public class GroundItem : MonoBehaviour//, ISerializationCallbackReceiver
{
    // item GameObject must have a 2D Collider with trigger turned on

    public ItemObject item; // holds the item component
    public bool collected = false;

    //public void OnAfterDeserialize()
    //{
    //}

    public void Start()
    {

        GetComponentInChildren<SpriteRenderer>().sprite = item.uiDisplay;
    }
    //public void OnBeforeSerialize()
    //{
       
    //    //EditorUtility.SetDirty(GetComponentInChildren<SpriteRenderer>());
    //    GetComponentInChildren<SpriteRenderer>().sprite = item.uiDisplay;
    //}

   
}
