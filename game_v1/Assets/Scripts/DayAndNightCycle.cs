using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class DayAndNightCycle : MonoBehaviour
{
    public Gradient lightColor;
    Light2D light2D;

    public static int days;

    public float time = 50;
    public static float multiplier = 1;
    bool canChangeDay = true;
    public delegate void OnDayChanged();
    public OnDayChanged DayChanged;

    public GameObject trees;
    public Image celestialBody;
    public Sprite sunSprite;
    public Sprite moonSprite;

    public Tilemap tilemapFG;
    Vector3 topRight;
    Vector3 bottomLeft;

    public static bool isDay = true;
    // Start is called before the first frame update
    void Start()
    {
        light2D = gameObject.GetComponent<Light2D>();
        DayChanged += new OnDayChanged(trees.GetComponent<PlaceSapling>().GrowSapling);

        topRight = tilemapFG.WorldToCell(Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0)));
        bottomLeft = tilemapFG.WorldToCell(Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)));
        //sun.transform.position = bottomLeft;

        //Debug.Log(bottomLeft);
        //Debug.Log(topRight);
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 500)
        {
            time = 0;
        }

        if ((int)time == 125)
        {
            
            celestialBody.sprite = moonSprite;
            isDay = false;
        }

        

        if ((int)time == 375 && canChangeDay)
        {
            days++;
            canChangeDay = false;
            DayChanged();
            celestialBody.sprite = sunSprite;
            isDay = true;
        }

        if ((int)time == 380)
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
        celestialBody.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)) + new Vector3(0, 0.7f * (topRight.y - bottomLeft.y), 10) + GetSunPosition((((time + 125) % 250) / 250f) * (topRight.x - bottomLeft.x), (topRight.x - bottomLeft.x));
        light2D.color = lightColor.Evaluate(time * 0.002f);
    }

    Vector3 GetSunPosition(float x, float width)
    {
        float y = (-1 / (4 * width)) * x * (x - width);

        return new Vector3(x, y, 0);
    }
}
