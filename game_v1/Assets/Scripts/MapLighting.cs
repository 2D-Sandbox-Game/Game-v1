using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapLighting : MonoBehaviour
{
    public int height = 512;
    public int width = 512;
    public Tilemap tilemap;

    public int lightRadius = 5;
    [Range(0, 1)]
    public float dropoff = 1;

    Vector3Int bottomLeft, bottomLeftOld, topRight;

    bool[,] boolArr;
    // Start is called before the first frame update
    void Start()
    {
        bottomLeft = tilemap.WorldToCell(Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)));
        SetMapLighting();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bottomLeft = tilemap.WorldToCell(Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)));

        if (bottomLeft.magnitude - bottomLeftOld.magnitude > 10)
        {
            SetMapLighting();
        }
        
    }


    void SetMapLighting()
    {

        Vector3Int offset = new Vector3Int(20, 20, 0);
        bottomLeftOld = bottomLeft;
        Vector3Int bottomLeftOff = bottomLeftOld - offset;
        topRight = tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0))) + offset;

        //Debug.Log(topRight);

        for (int x = bottomLeftOff.x; x < topRight.x; x++)
        {
            for (int y = bottomLeftOff.y; y < topRight.y; y++)
            {
                if (tilemap.GetTile(new Vector3Int(x,y,0)) == null)
                {
                    ChangeBrightness(new Vector3Int(x, y, 0), 1, dropoff, lightRadius);
                }
            }
        }

        
    }

    void ChangeBrightness(Vector3Int tilePos, float lightLevel, float dropoff, int lightRadius)
    {
        //if (tilePos.x < bottomLeft.x || tilePos.y < bottomLeft.y || tilePos.x > topRight.x || tilePos.y > topRight.y)
        //{
        //    return;
        //}
        if (lightRadius == 0)
        {
            return;
        }


        if (tilemap.GetTile(tilePos) != null)
        {
            tilemap.SetTileFlags(tilePos, TileFlags.None);
            tilemap.SetColor(tilePos, Color.HSVToRGB(0, 0, lightLevel));
        }

        for (int x = tilePos.x - 1; x <= tilePos.x + 1; x++)
        {
            for (int y = tilePos.y - 1; y <= tilePos.y + 1; y++)
            {
                Vector3Int currPos = new Vector3Int(x, y, 0);

                if ((x != tilePos.x || y != tilePos.y) && tilemap.GetTile(currPos) != null)
                {
                    float h, s, v;
                    Color.RGBToHSV(tilemap.GetColor(currPos), out h, out s, out v);

                    float dist = Mathf.Sqrt(Mathf.Pow(x - tilePos.x, 2) + Mathf.Pow(y - tilePos.y, 2));
                    float targetLightLevel = lightLevel * Mathf.Pow(dropoff, dist);

                    //if (lightRadius == 1)
                    //{
                    //    Debug.Log(targetLightLevel);
                    //}

                    if (v < targetLightLevel)
                    {
                        ChangeBrightness(currPos, targetLightLevel, dropoff, lightRadius - 1);
                    }

                    
                }
            }
        }
    }


}
