using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerationLighting : MonoBehaviour
{
    int[,] perlinArr;
    float[,] lightValueArr;
    Tilemap tilemapLight;
    Vector3Int bottomLeft;

    public Tile tileLight;
    public int offset;
    public int lightRadius = 5;
    [Range(0, 1)]
    public float dropoff = 1;

    void Start()
    {
        perlinArr = Generation.perlinArr;
        tilemapLight = GetComponent<Tilemap>();

        //bottomLeft = tilemapLight.WorldToCell(Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)));
    }

    // Update is called once per frame
    void Update()
    {
        if (PositionHasChangedByAmount(ref bottomLeft, 5))
        {

            Vector3Int topRight = tilemapLight.WorldToCell(Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0)));
            Vector3Int topRightOff = topRight + new Vector3Int(offset, offset, 0);
            Vector3Int bottomLeftOff = bottomLeft - new Vector3Int(offset, offset, 0);

            lightValueArr = new float[Mathf.Abs(bottomLeftOff.x - topRightOff.x), Mathf.Abs(bottomLeftOff.y - topRightOff.y)];
            float lowestVal = GetLowestLightValue(1, lightRadius);
            SetBaseLight(ref lightValueArr, lowestVal);

            lightValueArr = GenerateLightValueArray(perlinArr, bottomLeftOff, topRightOff);
            GenerateLightMap(tilemapLight, lightValueArr, bottomLeftOff, tileLight);


            //tilemapLight.ClearAllTiles();
            //StartCoroutine(RefreshLighting(bottomLeftOff, topRightOff));
        }
    }



    bool PositionHasChangedByAmount(ref Vector3Int pos, int distance)
    {
        Vector3Int newPos = tilemapLight.WorldToCell(Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)));

        if (Mathf.Abs(pos.x - newPos.x) > distance || Mathf.Abs(pos.y - newPos.y) > distance)
        {
            pos = newPos;
            return true;
        }

        return false;
    }

    bool IsWithinBounds<T>(int x, int y, T[,] arr)
    {
        return x >= 0 && y >= 0 && x < arr.GetLength(0) && y < arr.GetLength(1);
    }



    float[,] GenerateLightValueArray(int[,] perlinArr, Vector3Int bottomLeftOff, Vector3Int topRightOff)
    {


        //Debug.Log($"({lightValueArr.GetLength(0)}, {lightValueArr.GetLength(1)})");

        for (int x = 0; x < lightValueArr.GetLength(0); x++)
        {
            for (int y = 0; y < lightValueArr.GetLength(1); y++)
            {
                if (IsWithinBounds(x + bottomLeftOff.x, y + bottomLeftOff.y, perlinArr) && perlinArr[x + bottomLeftOff.x, y + bottomLeftOff.y] == 0)
                {
                    SetLightValue(new Vector2Int(x, y), 1, lightRadius, bottomLeftOff, topRightOff);
                }
            }
        }

        return lightValueArr;
    }

    void SetLightValue(Vector2Int arrPos, float lightLevel, int lightRadius, Vector3Int bottomLeftOff, Vector3Int topRightOff)
    {
        if (lightRadius == 0)
        {
            return;
        }

        lightValueArr[arrPos.x, arrPos.y] = lightLevel;

        for (int x = arrPos.x - 1; x <= arrPos.x + 1; x++)
        {
            for (int y = arrPos.y - 1; y <= arrPos.y + 1; y++)
            {
                if ((x != arrPos.x || y != arrPos.y) && IsWithinBounds(x, y, lightValueArr) && perlinArr[x + bottomLeftOff.x, y + bottomLeftOff.y] != 0)
                {
                    float dist = Mathf.Sqrt((x - arrPos.x) * (x - arrPos.x) + (y - arrPos.y) * (y - arrPos.y));
                    float targetLightLevel = lightLevel * Mathf.Pow(dropoff, dist);

                    if (lightValueArr[x, y] < targetLightLevel)
                    {
                        //Debug.Log("1");
                        SetLightValue(new Vector2Int(x, y), targetLightLevel, lightRadius - 1, bottomLeftOff, topRightOff);
                    }
                }
            }
        }
    }

    float GetLowestLightValue(float lightLevel, int lightRadius)
    {
        if (lightRadius == 0)
        {
            return lightLevel;
        }

        float dist = 1;
        float targetLightLevel = lightLevel * Mathf.Pow(dropoff, dist);

        return GetLowestLightValue(targetLightLevel, lightRadius - 1);
    }

    void SetBaseLight(ref float[,] arr, float lightLevel)
    {
        for (int x = 0; x < arr.GetLength(0); x++)
        {
            for (int y = 0; y < arr.GetLength(1); y++)
            {
                arr[x, y] = lightLevel;
            }
        }
    }

    void GenerateLightMap(Tilemap tilemapLight, float[,] lightValueArr, Vector3Int bottomLeft, Tile tileLight)
    {
        for (int x = 0; x < lightValueArr.GetLength(0); x++)
        {
            for (int y = 0; y < lightValueArr.GetLength(1); y++)
            {
                Vector3Int tilePos = new Vector3Int(x + bottomLeft.x, y + bottomLeft.y, 0);

                if (tilemapLight.GetTile(tilePos) == null)
                {
                    tilemapLight.SetTile(tilePos, tileLight);
                    tilemapLight.SetTileFlags(tilePos, TileFlags.None);
                    tilemapLight.SetColor(tilePos, new Color(0, 0, 0, 1 - lightValueArr[x, y]));
                }

                //Debug.Log($"LightValue at ({x},{y}): {lightValueArr[x, y]}");
            }
        }
    }
}
