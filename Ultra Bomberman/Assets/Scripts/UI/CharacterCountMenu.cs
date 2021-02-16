using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterCountMenu : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerCount;
    [SerializeField]
    private TextMeshProUGUI agentCount;
    
    private int playerCountNumber;
    private int agentCountNumber;
    private int characterCountNumber;

    void Start()
    {
        playerCountNumber = Int32.Parse(playerCount.text);
        agentCountNumber = Int32.Parse(agentCount.text);
        UpdateCharacterCount();
    }

    public void IncreasePlayerCount()
    {
        if ((characterCountNumber + 1) <= 4)
        {
            UpdatePlayerCount(1);
        }
        else if (agentCountNumber >= 1 && playerCountNumber < 4)
        {
            UpdatePlayerCount(1);
            UpdateAgentCount(-1);
        }
    }

    public void DecreasePlayerCount()
    {
        if ((characterCountNumber - 1) >= 2)
        {
            UpdatePlayerCount(-1);
        }
    }

    public void IncreaseAgentCount()
    {
        if ((characterCountNumber + 1) <= 4)
        {
            UpdateAgentCount(1);
        }
        else if (playerCountNumber >= 1 && agentCountNumber < 4)
        {
            UpdateAgentCount(1);
            UpdatePlayerCount(-1);
        }
    }

    public void DecreaseAgentCount()
    {
        if ((characterCountNumber - 1) >= 2)
        {
            UpdateAgentCount(-1);
        }
    }

    private void UpdatePlayerCount(int change)
    {
        playerCountNumber += change;
        playerCount.text = playerCountNumber.ToString();
        UpdateCharacterCount();
    }

    private void UpdateAgentCount(int change)
    {
        agentCountNumber += change;
        agentCount.text = agentCountNumber.ToString();
        UpdateCharacterCount();
    }

    private void UpdateCharacterCount()
    {
        characterCountNumber = playerCountNumber + agentCountNumber;
    }

    public void StartGame()
    {
        G.playerCount = playerCountNumber;
        G.agentCount = agentCountNumber;
        G.characterCount = characterCountNumber;
        SceneManager.LoadScene("Level1Scene", LoadSceneMode.Single);
    }
}
