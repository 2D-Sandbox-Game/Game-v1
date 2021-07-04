using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class DayAndNightCycle : MonoBehaviour
{
    // Static variables
    public static int s_days;
    public static float s_multiplier = 5;
    public static bool s_isDay = true;

    // Public variables
    public Gradient LightColor;
    public float CurrentTime = 50;
    public GameObject Trees;
    public Image CelestialBody;
    public Sprite SunSprite;
    public Sprite MoonSprite;
    public Tilemap TilemapFG;

    // Private variables
    Light2D _light2D;
    bool _canChangeDay = true;
    Vector3 _topRight;
    Vector3 _bottomLeft;

    // Delegates
    public delegate void OnDayChanged();
    public OnDayChanged DayChanged;

    // Start is called before the first frame update
    void Start()
    {
        _light2D = gameObject.GetComponent<Light2D>();
        DayChanged += new OnDayChanged(Trees.GetComponent<PlaceSapling>().GrowSapling);

        // Positions of the camera corner points in tilemap units
        _topRight = TilemapFG.WorldToCell(Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0)));
        _bottomLeft = TilemapFG.WorldToCell(Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)));
    }

    // Update is called once per frame
    void Update()
    {
        // Resets time cycle
        if (CurrentTime > 500)
        {
            CurrentTime = 0;
        }

        // Change: day -> night
        if ((int)CurrentTime >= 125 && (int)CurrentTime <= 130)
        {
            CelestialBody.sprite = MoonSprite;
            s_isDay = false;
        }

        // Change: night -> day
        if ((int)CurrentTime >= 375 && (int)CurrentTime <= 380 && _canChangeDay)
        {
            s_days++;
            _canChangeDay = false;
            DayChanged();
            CelestialBody.sprite = SunSprite;
            s_isDay = true;
        }

        if ((int)CurrentTime > 380 && (int)CurrentTime <= 385)
        {
            _canChangeDay = true;
        }

        // Changes time multiplier if "Z" key is pressed
        if (Input.GetKeyDown(KeyCode.Z) && s_multiplier == 5)
        {
            s_multiplier = 100;
        }
        else if (Input.GetKeyDown(KeyCode.Z) && s_multiplier == 100)
        {
            s_multiplier = 5;
        }

        CurrentTime += Time.deltaTime * s_multiplier;

        // Changes position of the sun / moon
        CelestialBody.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)) +
                                            new Vector3(0, 0.7f * (_topRight.y - _bottomLeft.y), 10) +
                                            GetSunPosition((((CurrentTime + 125) % 250) / 250f) * (_topRight.x - _bottomLeft.x), (_topRight.x - _bottomLeft.x));

        // Changes global lighting according to gradient
        LightColor.mode = GradientMode.Blend;
        _light2D.color = LightColor.Evaluate(CurrentTime * 0.002f);
    }

    /// <summary>
    /// Calculates the suns current position using a quadratic function.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="width"></param>
    /// <returns></returns>
    Vector3 GetSunPosition(float x, float width)
    {
        float y = (-1 / (4 * width)) * x * (x - width);

        return new Vector3(x, y, 0);
    }
}
