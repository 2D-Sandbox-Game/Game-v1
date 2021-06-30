using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

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

    public Tilemap tilemapFG;
    int[] highestPointArray;
    Vector3Int topRight, bottomLeft;
    bool spawnSuccessful = false;
    int spawnRadius = 10;
    Generation.BlockType[,] mapArr;
    int spawncap = 10;
    int enemyCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        highestPointArray = Generation.perlinHeight;
        mapArr = Generation.perlinArr;
    }

    // Update is called once per frame
    void Update()
    {
        topRight = tilemapFG.WorldToCell(Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0)));
        bottomLeft = tilemapFG.WorldToCell(Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)));
        enemyCount = GetEnemyCount();
        elapsedTime += Time.deltaTime * DayAndNightCycle.multiplier;

        if (enemyCount < spawncap && !DayAndNightCycle.isDay && elapsedTime >= waitTime)
        {
            SpawnEnemiesOverworldOnly();
        }

    }

    void SpawnEnemiesOverworldOnly()
    {
        int xCoord = Random.Range(0, 2) == 0 ? Random.Range(bottomLeft.x - spawnRadius, bottomLeft.x) : Random.Range(topRight.x, topRight.x + spawnRadius);
        int yCoord = highestPointArray[xCoord] + 1;

        Vector3 spawnpoint = new Vector3Int(xCoord, yCoord, 0);

        Debug.Log(bottomLeft);
        Debug.Log(topRight);

        if (mapArr[xCoord, yCoord] < Generation.BlockType.Dirt || mapArr[xCoord, yCoord] > Generation.BlockType.Wood)
        {
            if (mapArr[xCoord, yCoord + 1] < Generation.BlockType.Dirt || mapArr[xCoord, yCoord + 1] > Generation.BlockType.Wood)
            {
                SpawnEnemy(spawnpoint);
                //spawnSuccessful = true;
                elapsedTime = 0f;
                waitTime = Random.Range(10, 20);
            }
        }
    }

    void SpawnEnemies()
    {
        int xCoord = Random.Range(0, 2) == 0 ? Random.Range(bottomLeft.x - spawnRadius, bottomLeft.x) : Random.Range(topRight.x, topRight.x + spawnRadius);
        //int xCoord = Random.Range(0, 2) == 0 ? bottomLeft.x : bottomLeft.x + topRight.x;
        int yCoord = Random.Range(bottomLeft.y - spawnRadius, topRight.y);

        if (yCoord > highestPointArray[xCoord])
        {
            if (DayAndNightCycle.isDay)
            {
                return;
            }

            yCoord = highestPointArray[xCoord] + 1;
        }

        Vector3 spawnpoint = new Vector3Int(xCoord, yCoord, 0);

        Debug.Log(bottomLeft);
        Debug.Log(topRight);

        if (mapArr[xCoord, yCoord] < Generation.BlockType.Dirt || mapArr[xCoord, yCoord] > Generation.BlockType.Wood)
        {
            if (mapArr[xCoord, yCoord + 1] < Generation.BlockType.Dirt || mapArr[xCoord, yCoord + 1] > Generation.BlockType.Wood)
            {
                SpawnEnemy(spawnpoint);
                //spawnSuccessful = true;
                elapsedTime = 0f;
                waitTime = Random.Range(10, 20);
            }
        }
    }

    int GetEnemyCount()
    {
        return GetComponentsInChildren<Transform>().Length - 1;
    }

    void SpawnEnemy(Vector3 spawnpoint)
    {
        int randEnemy = Random.Range(0, enemyPrefabs.Length);
        //int randEnemy = 2;
        //instantiates enemy
        GameObject enemy = Instantiate(enemyPrefabs[randEnemy], spawnpoint, transform.rotation);
        enemy.transform.parent = gameObject.transform;
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
    }

    //void SpawnEnemiesOld()
    //{
    //    elapsedTime += Time.deltaTime;
    //    if (elapsedTime >= waitTime)
    //    {
    //        //generates random position
    //        System.Random random = new System.Random();
    //        int playerPosition = (int)player.transform.position.x;
    //        int enemyNewPosition = random.Next(playerPosition + 50, playerPosition + 150);
    //        spawnPoints[0].position = new Vector3(enemyNewPosition, highestPointArray[enemyNewPosition], 1f);
    //        spawnPoints[1].position = new Vector3(enemyNewPosition - 200, highestPointArray[enemyNewPosition - 200], 1f);

    //        int randEnemy = Random.Range(0, enemyPrefabs.Length);
    //        int randSpwanPoint = Random.Range(0, spawnPoints.Length);

    //        //instantiates enemy
    //        GameObject enemy = Instantiate(enemyPrefabs[randEnemy], spawnPoints[randSpwanPoint].position, transform.rotation);
    //        enemy.GetComponent<Direction>().target = GameObject.Find("Player").transform;

    //        //instantiates HealthbarForeground
    //        GameObject hbF = Instantiate(prefabHbF, prefabHbF.transform.position, prefabHbF.transform.rotation) as GameObject;
    //        hbF.transform.parent = life.transform;
    //        hbF.GetComponent<HealthBarPosition>().enemy = enemy;
    //        hbF.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);

    //        //instantiates HealthbarBackground
    //        GameObject hbB = Instantiate(prefabHbB, prefabHbB.transform.position, prefabHbB.transform.rotation) as GameObject;
    //        hbB.transform.parent = life.transform;
    //        hbB.GetComponent<HealthBarPosition>().enemy = enemy;
    //        hbB.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);

    //        enemy.GetComponent<HealthBar>().healthBar = hbF;
    //        enemy.GetComponent<HealthBar>().healthBarBackground = hbB;

    //        elapsedTime = 0f;
    //        waitTime = random.Next(5, 10);
    //    }
    //}
}
