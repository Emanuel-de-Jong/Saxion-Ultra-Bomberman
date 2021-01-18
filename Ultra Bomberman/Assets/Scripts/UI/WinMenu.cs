using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour
{
    [SerializeField] Image CharacterImage;
    [SerializeField] Sprite[] CharacterSprites;

    void Start()
    {
        CharacterImage.sprite = CharacterSprites[G.playerWon - 1];
    }

    public void LoadLevelScene()
    {
        SceneManager.LoadScene("Level1Scene", LoadSceneMode.Single);
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
    }
}
