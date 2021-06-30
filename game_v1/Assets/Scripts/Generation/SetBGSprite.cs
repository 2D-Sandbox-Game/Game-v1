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
        width = Generation.width;
        
        gameObject.transform.localScale = new Vector3(width, height);
        gameObject.transform.position = new Vector3(width/2, height/2, 0);
    }

    // Update is called once per frame
    void Update()
    {

    }

    

}
