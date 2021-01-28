using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] int roundDuration = 90;

    private CharacterController[] characterControllers;
    private int charactersAliveCount;
    private bool[] charactersAlive;

    void Start()
    {
        characterControllers = new CharacterController[G.characterCount];
        charactersAlive = new bool[G.characterCount];
        charactersAliveCount = G.characterCount;
        for (int i=0; i< G.characterCount; i++)
        {
            characterControllers[i] = GameObject.Find("Character" + (i + 1)).GetComponent<CharacterController>();
            charactersAlive[i] = true;
        }

        foreach(CharacterController characterController in characterControllers)
        {
            characterController.die.AddListener(DecreaseCharactersAlive);
        }

        if (G.train)
        {
            GameObject.Find("Countdown").GetComponent<Countdown>().currentTime = roundDuration;
            StartCoroutine(CountdownRoundRestart());
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

    public void DecreaseCharactersAlive(CharacterController characterController)
    {
        if (G.train)
            return;

        charactersAliveCount--;
        charactersAlive[characterController.characterNumber - 1] = false;

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
