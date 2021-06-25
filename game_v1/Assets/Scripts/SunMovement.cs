using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunMovement : MonoBehaviour
{
    public float dayPeriodInSeconds = 60;
    public float nightPeriodInSeconds = 60;
    float currentTime;
    int dayStartTime;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = (int)Time.realtimeSinceStartup;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Time.realtimeSinceStartup % 1);

        if ((int)Time.realtimeSinceStartup > currentTime)
        {
            currentTime = (int)Time.realtimeSinceStartup;
            Debug.Log($"Time: {currentTime}");
        }
    }

    void Movement(int currentTime, float dayPeriod)
    {
        if (dayStartTime == 0)
        {
            dayStartTime = currentTime;
        }

    }

    private void FixedUpdate()
    {
        
    }

    Vector3 GetSunPosition(float x, float width)
    {
        float y = (-1 / (4 * width)) * x * (x - width);

        return new Vector3(x, y, 0);
    }
}
