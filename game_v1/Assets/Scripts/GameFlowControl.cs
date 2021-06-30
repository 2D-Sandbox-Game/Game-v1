using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowControl : MonoBehaviour
{
    public GameObject player;
    public GameObject enemySpawner;
    public GameObject MinimapCamera;
    public GameObject MinimapCanvas;

    bool mapIsOpen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) && !mapIsOpen)
        {
            player.SetActive(false);
            enemySpawner.SetActive(false);
            MinimapCamera.SetActive(true);
            MinimapCanvas.SetActive(true);

            mapIsOpen = !mapIsOpen;
        }
        else if (Input.GetKeyDown(KeyCode.M) && mapIsOpen)
        {
            player.SetActive(true);
            enemySpawner.SetActive(true);
            MinimapCamera.SetActive(false);
            MinimapCanvas.SetActive(false);

            mapIsOpen = !mapIsOpen;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
