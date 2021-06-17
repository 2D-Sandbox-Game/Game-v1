using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject[] enemyPrefabs;
    public GameObject prefabHbF;
    public GameObject prefabHbB;
    public GameObject life;
    public GameObject player;
    float waitTime = 5.0f;
    float elapsedTime = 0.0f;
    int[] highestPointArray;
    // Start is called before the first frame update
    void Start()
    {
        highestPointArray = Generation.perlinHeight;
    }

    // Update is called once per frame
    void Update()
    {       
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= waitTime)
        {
            //generates random position
            System.Random random = new System.Random(); 
            int playerPosition = (int)player.transform.position.x;
            int enemyNewPosition = random.Next(playerPosition + 50, playerPosition + 150);
            spawnPoints[0].position = new Vector3(enemyNewPosition, highestPointArray[enemyNewPosition], 1f);
            spawnPoints[1].position = new Vector3(enemyNewPosition - 200, highestPointArray[enemyNewPosition], 1f);

            int randEnemy = Random.Range(0, enemyPrefabs.Length);
            int randSpwanPoint = Random.Range(0, spawnPoints.Length);

            //instantiates enemy
            GameObject enemy = Instantiate(enemyPrefabs[randEnemy], spawnPoints[randSpwanPoint].position, transform.rotation);
            enemy.GetComponent<Direction>().target = GameObject.Find("Player").transform;

            //instantiates HealthbarForeground
            GameObject hbF = Instantiate(prefabHbF, prefabHbF.transform.position, prefabHbF.transform.rotation) as GameObject;
            hbF.transform.parent = life.transform;
            hbF.GetComponent<HealthBarPosition>().enemy = enemy;
            hbF.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);

            //instantiates HealthbarBackground
            GameObject hbB = Instantiate(prefabHbB, prefabHbB.transform.position, prefabHbB.transform.rotation) as GameObject;
            hbB.transform.parent = life.transform;
            hbB.GetComponent<HealthBarPosition>().enemy = enemy;
            hbB.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);

            enemy.GetComponent<HealthBar>().healthBar = hbF;
            enemy.GetComponent<HealthBar>().healthBarBackground = hbB;

            elapsedTime = 0f;
            waitTime = random.Next(30, 180);
        }
    }
}
