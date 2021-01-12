using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void ChangeScene()
    {
        SceneManager.LoadScene("Level1Scene", LoadSceneMode.Single);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
