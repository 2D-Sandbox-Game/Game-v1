using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject bulletObject;
    float waitTime = 2.5f; //time until new arrow is instantiate
    float elapsedTime = 0.0f;
    string levelName = "";
    public GameObject player;
    public GameObject enemy;

    public void Ini(float waitT, string level)
    {
        waitTime = waitT;
        levelName = level;
    }
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        double distance = Math.Abs(player.transform.position.x - enemy.transform.position.x);
        if (elapsedTime >= waitTime && distance < 20)
        {
            GameObject bullet = Instantiate(bulletObject, transform.position, transform.rotation);
            Destroy(bullet, 3f);
            elapsedTime = 0f;
            //Debug.Log("Pew pew");
        }
    }
}
