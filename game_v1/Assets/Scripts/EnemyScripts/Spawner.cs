using UnityEngine;
using UnityEngine.Tilemaps;

public class Spawner : MonoBehaviour
{
    // Public variables
    public Transform[] SpawnPoints;
    public GameObject[] EnemyPrefabs;
    public GameObject PrefabHbF;
    public GameObject PrefabHbB;
    public GameObject Life;
    public GameObject Player;
    public Tilemap TilemapFG;
    public int Spawncap = 10;

    // Private variables
    float _waitTime = 5.0f;
    float _elapsedTime = 0.0f;
    int[] _highestPointArray;
    Vector3Int _topRight, _bottomLeft;
    int _spawnRadius = 10;
    Generation.BlockType[,] _mapArr;
    int _enemyCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        _highestPointArray = Generation.s_perlinHeight;
        _mapArr = Generation.s_perlinArr;
    }

    // Update is called once per frame
    void Update()
    {
        // Positions of the camera corner points in tilemap units
        _topRight = TilemapFG.WorldToCell(Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0)));
        _bottomLeft = TilemapFG.WorldToCell(Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)));

        // Gets current enemy count
        _enemyCount = GetEnemyCount();

        _elapsedTime += Time.deltaTime * DayAndNightCycle.s_multiplier;

        // Conditions for spawing enemies are met
        if (_enemyCount < Spawncap && !DayAndNightCycle.s_isDay && _elapsedTime >= _waitTime)
        {
            SpawnEnemiesOverworldOnly();
        }
    }

    /// <summary>
    /// Creates spawnpoint and spawn enemy. (on surface only)
    /// </summary>
    void SpawnEnemiesOverworldOnly()
    {
        // Generates a random number within a certain area on the left or the right of the player (outside camera view)
        int xCoord = Random.Range(0, 2) == 0 ? Random.Range(_bottomLeft.x - _spawnRadius, _bottomLeft.x) : Random.Range(_topRight.x, _topRight.x + _spawnRadius);
        int yCoord = _highestPointArray[xCoord] + 2;

        Vector3 spawnpoint = new Vector3Int(xCoord, yCoord, 0);

        if (_mapArr[xCoord, yCoord] < Generation.BlockType.Dirt || _mapArr[xCoord, yCoord] > Generation.BlockType.Wood)
        {
            if (_mapArr[xCoord, yCoord + 1] < Generation.BlockType.Dirt || _mapArr[xCoord, yCoord + 1] > Generation.BlockType.Wood)
            {
                SpawnEnemy(spawnpoint);
                _elapsedTime = 0f;
                _waitTime = Random.Range(50, 100);
            }
        }
    }

    /// <summary>
    /// Gets current enemy count.
    /// </summary>
    /// <returns></returns>
    int GetEnemyCount()
    {
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        int directChilds = 0;

        foreach (Transform childTransform in allChildren)
        {
            // Child's parent is this gameobject -> direct child
            if (childTransform.parent == transform)
            {
                directChilds++;
            }
        }

        return directChilds;
    }

    /// <summary>
    /// Creates and positions the game objects of the enemy.
    /// </summary>
    /// <param name="spawnpoint"></param>
    void SpawnEnemy(Vector3 spawnpoint)
    {
        int randEnemy = Random.Range(0, EnemyPrefabs.Length);

        // Instantiates enemy
        GameObject enemy = Instantiate(EnemyPrefabs[randEnemy], spawnpoint, transform.rotation);        
        enemy.transform.SetParent(gameObject.transform);
        enemy.GetComponent<Direction>().target = GameObject.Find("Player").transform;

        // Instantiates healthbarForeground
        GameObject hbF = Instantiate(PrefabHbF, PrefabHbF.transform.position, PrefabHbF.transform.rotation) as GameObject;
        hbF.transform.SetParent(Life.transform);
        hbF.GetComponent<HealthBarPosition>().enemy = enemy;
        hbF.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);

        // Instantiates HealthbarBackground
        GameObject hbB = Instantiate(PrefabHbB, PrefabHbB.transform.position, PrefabHbB.transform.rotation) as GameObject;
        hbB.transform.SetParent(Life.transform);
        hbB.GetComponent<HealthBarPosition>().enemy = enemy;
        hbB.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);

        enemy.GetComponent<HealthBar>().healthBar = hbF;
        enemy.GetComponent<HealthBar>().healthBarBackground = hbB;
    }
}

