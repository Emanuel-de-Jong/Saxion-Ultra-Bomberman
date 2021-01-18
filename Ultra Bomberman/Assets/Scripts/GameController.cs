using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject[] players;

    void Start()
    {
        EnablePlayers();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
        }
    }

    private void EnablePlayers()
    {
        for (int i = 0; i < G.playerCount; i++)
        {
            players[i].SetActive(true);
        }
    }
}
