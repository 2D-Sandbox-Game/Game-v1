using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(Generation.width / 2, Generation.height / 2, transform.position.z);
        GetComponent<Camera>().orthographicSize = Generation.height / 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
