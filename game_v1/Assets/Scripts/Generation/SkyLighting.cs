using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Tilemaps;
using System;

public class SkyLighting : MonoBehaviour
{
    Generation.BlockType[,] mapArr;
    int[] highestPointsArr;

    // Start is called before the first frame update
    void Start()
    {
        mapArr = Generation.s_perlinArr;
        highestPointsArr = Generation.s_perlinHeight;

        //gameObject.transform.position = new Vector3(0, 0);
        //SetShapePath(GetComponent<Light2D>(), Test());
        //SetShapePath(GetComponent<Light2D>(), GetSkyVectorPath(mapArr, highestPointsArr));

        GenerateLightSources(mapArr);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    SetShapePath(GetComponent<Light2D>(), GetSkyVectorPath(mapArr, highestPointsArr));
        //}
    }

    void SetFieldValue<T>(object obj, string name, T val)
    {
        var field = obj.GetType().GetField(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        field?.SetValue(obj, val);
    }

    void SetShapePath(Light2D light, Vector3[] path)
    {
        SetFieldValue<Vector3[]>(light, "m_ShapePath", path);
    }

    Vector3[] Test()
    {
        List<Vector3> vectorArr = new List<Vector3>();

        vectorArr.Add(new Vector3(0, 0));
        vectorArr.Add(new Vector3(mapArr.GetLength(0), 0));
        vectorArr.Add(new Vector3(mapArr.GetLength(0), mapArr.GetLength(1)));
        vectorArr.Add(new Vector3(0, mapArr.GetLength(1)));

        return vectorArr.ToArray();
    }

    Vector3[] GetSkyVectorPath(Generation.BlockType[,] mapArr, int[] highestPointsArr)
    {
        int x = 0;
        int y = highestPointsArr[0] + 1;

        int test = 20;

        Vector3 startingPoint = new Vector3(x, y, 0);
        Vector3 currentPoint = startingPoint;

        bool horizontal = true;

        List<Vector3> vectorArr = new List<Vector3>();
        vectorArr.Add(currentPoint);

        do
        {
            if (horizontal)
            {
                if (mapArr[x, y - 1] == 0)
                {
                    currentPoint = new Vector3(x, y);
                    vectorArr.Add(currentPoint);

                    y--;
                    horizontal = !horizontal;
                }
                else if (mapArr[x + 1, y] == 0)
                {
                    x++;
                }
                else
                {
                    currentPoint = new Vector3(x + 1, y);
                    vectorArr.Add(currentPoint);

                    x++;
                    y++;
                    horizontal = !horizontal;
                }
            }
            else
            {
                if (mapArr[x, y - 1] == 0 && mapArr[x + 1, y - 1] == 0)
                {
                    y--;
                }
                else if (mapArr[x + 1, y] == 0)
                {
                    currentPoint = new Vector3(x, y);
                    vectorArr.Add(currentPoint);

                    x++;
                    horizontal = !horizontal;
                }
                else
                {
                    y++;
                }

            }

            //Debug.Log($"Coords x: {x} y: {y}");

        } while (x < mapArr.GetLength(0) - 1);

        //vectorArr.Add(new Vector3(mapArr.GetLength(0), mapArr.GetLength(1)));
        vectorArr.Add(new Vector3(mapArr.GetLength(0), mapArr.GetLength(1)));
        vectorArr.Add(new Vector3(0, mapArr.GetLength(1)));

        //foreach (var item in vectorArr)
        //{
        //    Debug.Log(item);
        //}

        return vectorArr.ToArray();
    }

    void GenerateLightSources(Generation.BlockType[,] perlinArr)
    {
        for (int i = 0; i < perlinArr.GetLength(0); i++)
        {
            for (int j = 0; j < perlinArr.GetLength(1); j++)
            {
                if (perlinArr[i, j] == 0 && perlinArr[i, j - 1] != 0)
                {
                    GameObject obj = new GameObject();
                    
                    obj.AddComponent<Light2D>();
                    Light2D light = obj.GetComponent<Light2D>();
                    light.lightType = Light2D.LightType.Point;
                    light.pointLightInnerRadius = 1.3f;
                    light.pointLightOuterRadius = 25.3f;
                    obj.transform.parent = gameObject.transform;
                    obj.transform.position = new Vector3(i + 0.5f, j + 0.5f, 0);
                    
                }
            }
        }
    }

}
