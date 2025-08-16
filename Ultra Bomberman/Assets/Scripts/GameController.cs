using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    public UnityEvent reset;

    public Character[] characters;

    [SerializeField]
    private int twoPlayerEpisodes = 5;
    [SerializeField]
    private int threePlayerEpisodes = 15;
    [SerializeField]
    private int fourPlayerEpisodes = 5;

    private int totalPlayerEpisodes;
    private int currentPlayerEpisode = 1;
    private int charactersAliveCount;
    private bool[] charactersAlive;

    private void Start()
    {
        G.gameController = this;

        if (G.train)
        {
            if (!G.record)
            {
                G.characterCount = 2;
                totalPlayerEpisodes = twoPlayerEpisodes + threePlayerEpisodes + fourPlayerEpisodes;
            }

            InvokeRepeating(nameof(Reset), G.roundDuration, G.roundDuration);
        }
        else
        {
            charactersAliveCount = G.characterCount;

            charactersAlive = new bool[G.characterCount];
            for (int i = 0; i < G.characterCount; i++)
                charactersAlive[i] = true;

            foreach (Character character in characters)
                character.die.AddListener(DecreaseCharactersAlive);
        }

        if (G.train && !G.record)
            QualitySettings.SetQualityLevel(0);
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
        if (G.train && !G.record)
        {
            currentPlayerEpisode++;
            if (currentPlayerEpisode > totalPlayerEpisodes)
                currentPlayerEpisode = 1;

            int nextCharacterCount = 4;
            if (currentPlayerEpisode <= twoPlayerEpisodes)
            {
                nextCharacterCount = 2;
            }
            else if (currentPlayerEpisode <= (twoPlayerEpisodes + threePlayerEpisodes))
            {
                nextCharacterCount = 3;
            }

            G.characterCount = nextCharacterCount;
        }

        reset.Invoke();
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
