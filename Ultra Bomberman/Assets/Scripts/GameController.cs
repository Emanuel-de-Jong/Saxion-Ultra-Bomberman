using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject playerSpawner;

    void Start()
    {
        playerSpawner.GetComponent<PlayerSpawner>().SpawnPlayers();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
        }
    }
}
