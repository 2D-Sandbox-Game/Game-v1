using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            GameObject enemyCopy = Instantiate(enemy, transform.position + new Vector3(50f, 5f, 0), Quaternion.identity);
            enemyCopy.SetActive(true);
        }
    }
}
