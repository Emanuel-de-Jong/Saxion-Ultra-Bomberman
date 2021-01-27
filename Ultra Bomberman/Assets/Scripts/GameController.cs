using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] int roundDuration = 90;
    [SerializeField] GameObject[] players;

    private int playersAliveCount;
    private bool[] playersAlive;

    void Start()
    {
        playersAliveCount = G.playerCount;
        playersAlive = new bool[G.playerCount];

        EnablePlayers();

        if (G.train)
        {
            StartCoroutine(CountdownRoundRestart());
            GameObject.Find("GameUI").GetComponent<GameUI>().currentTime = roundDuration;
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            if (G.train)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
                SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
            }
        }
    }

    private IEnumerator CountdownRoundRestart()
    {
        yield return new WaitForSeconds(roundDuration);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
        if (G.train)
        {
            return;
        }

        playersAliveCount--;
        playersAlive[playerController.playerNumber - 1] = false;

        if (playersAliveCount == 1)
        {
            for (int i = 0; i < playersAlive.Length; i++)
            {
                if (playersAlive[i])
                {
                    G.playerWon = i + 1;
                    SceneManager.LoadScene("WinScene", LoadSceneMode.Single);
                }
            }
        }
    }
}
