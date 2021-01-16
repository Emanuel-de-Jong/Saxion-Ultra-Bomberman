using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCountMenu : MonoBehaviour
{
    [SerializeField] GameObject playerCount;
    [SerializeField] int playerCountNumber = 2;
    [SerializeField] int playerCountMin= 2;
    [SerializeField] int playerCountMax = 4;

    private TextMeshProUGUI playerCountTMP;

    void Start()
    {
        playerCountTMP = playerCount.GetComponent<TextMeshProUGUI>();
        UpdatePlayerCount();
    }

    public void DecreasePlayerCount()
    {
        if ((playerCountNumber - 1) >= playerCountMin)
        {
            playerCountNumber--;
            UpdatePlayerCount();
        }
    }

    public void IncreasePlayerCount()
    {
        if ((playerCountNumber + 1) <= playerCountMax)
        {
            playerCountNumber++;
            UpdatePlayerCount();
        }
    }

    private void UpdatePlayerCount()
    {
        playerCountTMP.text = playerCountNumber.ToString();
    }

    public void StartGame()
    {
        G.playerCount = playerCountNumber;
        SceneManager.LoadScene("Level1Scene", LoadSceneMode.Single);
    }
}
