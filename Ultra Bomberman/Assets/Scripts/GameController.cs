using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject[] players;

    private int playersAliveCount;
    private bool[] playersAlive;

    void Start()
    {
        playersAliveCount = G.playerCount;
        playersAlive = new bool[G.playerCount];

        EnablePlayers();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void EnablePlayers()
    {
        for (int i = 0; i < G.playerCount; i++)
        {
            playersAlive[i] = true;
            players[i].SetActive(true);
        }
    }

    public void DecreasePlayersAlive(CharacterController playerController)
    {
        playersAliveCount--;
        playersAlive[playerController.playerNumber - 1] = false;

        if (playersAliveCount == 1)
        {
            for (int i = 0; i < playersAlive.Length; i++)
            {
                if (playersAlive[i])
                {
                    G.playerWon = i + 1;
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
            }
        }
    }
}
