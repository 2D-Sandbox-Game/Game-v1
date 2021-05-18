using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generation : MonoBehaviour
{
    [SerializeField] int width, height;
    [SerializeField] GameObject dirt;
    // Start is called before the first frame update
    void Start()
    {
        Generator();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Generator()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                SpawnObj(dirt, x, y);
            }

        }
    }

    void SpawnObj(GameObject obj, int width, int height)
    {
        obj = Instantiate(dirt, new Vector2(width, height), Quaternion.identity);
        obj.AddComponent<BoxCollider2D>();
        obj.transform.parent = this.transform;
    }
}
