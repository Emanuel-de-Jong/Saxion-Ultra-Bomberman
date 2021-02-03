using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    public UnityEvent reset;

    [SerializeField]
    private Character[] characters;

    private int charactersAliveCount;
    private bool[] charactersAlive;

    private void Start()
    {
        G.gameController = this;

        charactersAliveCount = G.characterCount;
        charactersAlive = new bool[G.characterCount];
        for (int i = 0; i < G.characterCount; i++)
        {
            charactersAlive[i] = true;
        }

        foreach (Character character in characters)
        {
            character.die.AddListener(DecreaseCharactersAlive);
        }

        if (G.train)
        {
            StartCoroutine(WaitForReset());
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            if (G.train)
            {
                Reset();
            }
            else
            {
                SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
            }
        }
    }

    private void Reset()
    {
        reset.Invoke();
        StartCoroutine(WaitForReset());
    }

    private IEnumerator WaitForReset()
    {
        yield return new WaitForSeconds(G.roundDuration);
        Reset();
    }

    public void DecreaseCharactersAlive(Character character)
    {
        if (G.train)
            return;

        charactersAliveCount--;
        charactersAlive[character.characterNumber - 1] = false;

        if (charactersAliveCount == 1)
        {
            for (int i = 0; i < charactersAlive.Length; i++)
            {
                if (charactersAlive[i])
                {
                    G.characterWon = i + 1;
                    SceneManager.LoadScene("WinScene", LoadSceneMode.Single);
                }
            }
        }
    }
}
