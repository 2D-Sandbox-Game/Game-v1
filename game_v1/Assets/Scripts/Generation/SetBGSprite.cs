using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Tilemaps;

public class SetBGSprite : MonoBehaviour
{
    int width, height;

    // Start is called before the first frame update
    void Start()
    {
        height = Generation.height;
        width = Generation.height;

        gameObject.transform.localScale = new Vector3(width, height);
    }

    // Update is called once per frame
    void Update()
    {

    }

    

}
