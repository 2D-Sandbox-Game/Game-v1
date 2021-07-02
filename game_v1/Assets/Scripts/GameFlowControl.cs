using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameFlowControl : MonoBehaviour
{
    public GameObject player;
    public GameObject mapPointer;
    public GameObject enemySpawner;
    public GameObject MinimapCamera;
    public GameObject MinimapCanvas;
    public GameObject enemyLifeCanvas;

    bool mapIsOpen = false;
    bool spawnerIsActive = true;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) && !mapIsOpen)
        {
            player.GetComponent<PlayerMovement>().enabled = false;
            player.GetComponent<PlayerToolSelect>().enabled = false;
            mapPointer.SetActive(true);
            
            MinimapCamera.SetActive(true);
            MinimapCanvas.SetActive(true);

            if (spawnerIsActive)
            {
                enemySpawner.SetActive(false);
            }

            mapIsOpen = !mapIsOpen;
        }
        else if ((Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.Escape)) && mapIsOpen)
        {
            player.GetComponent<PlayerMovement>().enabled = true;
            player.GetComponent<PlayerToolSelect>().enabled = true;
            mapPointer.SetActive(false);
            MinimapCamera.SetActive(false);
            MinimapCanvas.SetActive(false);

            if (spawnerIsActive)
            {
                enemySpawner.SetActive(true);
            }

            mapIsOpen = !mapIsOpen;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.B) && spawnerIsActive)
        {
            enemySpawner.SetActive(false);
            enemyLifeCanvas.SetActive(false);

            spawnerIsActive = !spawnerIsActive;
        }
        else if (Input.GetKeyDown(KeyCode.B) && !spawnerIsActive)
        {
            enemySpawner.SetActive(true);
            enemyLifeCanvas.SetActive(true);

            spawnerIsActive = !spawnerIsActive;
        }
    }
}
