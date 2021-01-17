using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] PlayerController referencePlayer;
    [SerializeField] TextMeshProUGUI[] playerLifes;

    void Start()
    {
        int startHealth = referencePlayer.health;
        foreach (TextMeshProUGUI ui in playerLifes)
        {
            ui.text = startHealth.ToString();
        }
    }

    public void SetHealth(int player, int value)
    {
        playerLifes[player - 1].text = value.ToString();
    }
}
