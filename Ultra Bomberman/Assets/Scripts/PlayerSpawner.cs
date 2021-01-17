using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] players;

    public void SpawnPlayers()
    {
        for (int i=0; i<G.playerCount; i++)
        {
            players[i].SetActive(true);
        }
    }
}
