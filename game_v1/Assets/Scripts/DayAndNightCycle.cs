using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class DayAndNightCycle : MonoBehaviour
{
    public Gradient lightColor;
    Light2D light;

    int days;
    public float time = 50;
    public float multiplier = 1;
    bool canChangeDay = true;
    public delegate void OnDayChanged();
    public OnDayChanged DayChanged;
    // Start is called before the first frame update
    void Start()
    {
        light = gameObject.GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 500)
        {
            time = 0;
        }

        if ((int) time == 250 && canChangeDay)
        {
            canChangeDay = false;
            //DayChanged();
            days++;
        }

        if ((int) time == 255)
        {
            canChangeDay = true;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            multiplier = 50;
        }
        if (Input.GetKeyUp(KeyCode.Z))
        {
            multiplier = 5;
        }

        time += Time.deltaTime * multiplier;
        lightColor.mode = GradientMode.Blend;
        light.color = lightColor.Evaluate(time * 0.002f);
    }
}
