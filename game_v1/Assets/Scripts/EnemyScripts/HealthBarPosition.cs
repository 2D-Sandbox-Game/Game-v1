using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarPosition : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject bar;
    public GameObject Enemy;
    void Start()
    {
        Vector3 temp = Enemy.GetComponent<Transform>().position;
        Vector3 temp2 = new Vector3(0, 2, 0);
        temp += temp2;
        bar.GetComponent<Transform>().position = temp;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 temp = Enemy.GetComponent<Transform>().position;
        Vector3 temp2 = new Vector3(0, 2, 0);
        temp += temp2;
        bar.GetComponent<Transform>().position = temp;
    }
}
