using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject bulletObject;
    float waitTime = 2.0f;//2Sekunden warten als standart Wert
    float elapsedTime = 0.0f;
    string levelName = "";


    public void Ini(float waitT, string level)
    {
        waitTime = waitT;
        levelName = level;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetButtonDown("Fire1"))
        {
            GameObject bullet = Instantiate(bulletObject, transform.position, transform.rotation);
            Destroy(bullet, 1f);
        }*/
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= waitTime)
        {
            GameObject bullet = Instantiate(bulletObject, transform.position, transform.rotation);
            Destroy(bullet, 3f);
            elapsedTime = 0f;
        }
    }
}
