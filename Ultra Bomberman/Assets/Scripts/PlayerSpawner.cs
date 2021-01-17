using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] players;
    [SerializeField] string[] playerModels = new string[]
       {
            "MechanicalGolem",
            "BarbarianGiant",
            "BigOrk",
            "RedDemon"
       };

    public void SpawnPlayers()
    {
        for (int i=0; i<G.playerCount; i++)
        {
            string model = playerModels[i];
            GameObject tempPlayer = players[i];
            PlayerController tempScript = tempPlayer.GetComponent<PlayerController>();

            tempScript.model = model;
            tempPlayer.SetActive(true);
        }
    }
}
