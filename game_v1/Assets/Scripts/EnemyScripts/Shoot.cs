using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject bulletObject;
    float waitTime = 2.5f; // Time until new arrow is instantiate
    float elapsedTime = 0.0f;
    string levelName = "";
    public GameObject Player;
    public GameObject enemy;

    public void Ini(float waitT, string level)
    {
        waitTime = waitT;
        levelName = level;
    }
    void Start()
    {
        Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        double distance = Math.Abs(Player.transform.position.x - enemy.transform.position.x);
        if (elapsedTime >= waitTime && distance < 20)
        {
            GameObject Bullet = Instantiate(bulletObject, transform.position, transform.rotation);
            Destroy(Bullet, 3f);
            elapsedTime = 0f;
        }
    }
}
