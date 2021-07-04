using UnityEngine;

public class GameFlowControl : MonoBehaviour
{
    // Public variables
    public GameObject Player;
    public GameObject MapPointer;
    public GameObject EnemySpawner;
    public GameObject MinimapCamera;
    public GameObject MinimapCanvas;
    public GameObject EnemyLifeCanvas;

    // Private variables
    bool _mapIsOpen = false;
    bool _spawnerIsActive = true;

    // Update is called once per frame
    void Update()
    {
        // Opens / closes the in-game world map when the "M" key is pressed
        if (Input.GetKeyDown(KeyCode.M) && !_mapIsOpen)
        {
            Player.GetComponent<PlayerMovement>().enabled = false;
            Player.GetComponent<PlayerToolSelect>().enabled = false;
            MapPointer.SetActive(true);
            
            MinimapCamera.SetActive(true);
            MinimapCanvas.SetActive(true);

            if (_spawnerIsActive)
            {
                EnemySpawner.SetActive(false);
            }

            _mapIsOpen = !_mapIsOpen;
        }
        else if ((Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.Escape)) && _mapIsOpen)
        {
            Player.GetComponent<PlayerMovement>().enabled = true;
            Player.GetComponent<PlayerToolSelect>().enabled = true;
            MapPointer.SetActive(false);
            MinimapCamera.SetActive(false);
            MinimapCanvas.SetActive(false);

            if (_spawnerIsActive)
            {
                EnemySpawner.SetActive(true);
            }

            _mapIsOpen = !_mapIsOpen;
        }

        // Exits the game when the "ESC" key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        // Activates / deactivates monster spawing when the "B" key is pressed
        if (Input.GetKeyDown(KeyCode.B) && _spawnerIsActive)
        {
            EnemySpawner.SetActive(false);
            EnemyLifeCanvas.SetActive(false);

            _spawnerIsActive = !_spawnerIsActive;
        }
        else if (Input.GetKeyDown(KeyCode.B) && !_spawnerIsActive)
        {
            EnemySpawner.SetActive(true);
            EnemyLifeCanvas.SetActive(true);

            _spawnerIsActive = !_spawnerIsActive;
        }
    }
}
