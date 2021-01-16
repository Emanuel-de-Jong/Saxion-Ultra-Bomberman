using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] string[] playerModels = new string[]
       {
        "MechanicalGolem",
        "BarbarianGiant",
        "BigOrk",
        "RedDemon"
       };
    [SerializeField] KeyCode[][] playerControlls = new KeyCode[][]
    {
        new KeyCode[] { KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D, KeyCode.F },
        new KeyCode[] { KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.RightShift },
        new KeyCode[] { KeyCode.P, KeyCode.Semicolon, KeyCode.L, KeyCode.Quote, KeyCode.Backslash },
        new KeyCode[] { KeyCode.Y, KeyCode.H, KeyCode.G, KeyCode.J, KeyCode.K }
    };

    private Transform[] SpawnPoints;

    void Start()
    {
        SpawnPoints = new Transform[]
        {
            GameObject.Find("SP1").transform,
            GameObject.Find("SP2").transform,
            GameObject.Find("SP3").transform,
            GameObject.Find("SP4").transform
        };
    }

    public void SpawnPlayers()
    {
        for (int i=0; i<G.playerCount; i++)
        {
            Transform sp = SpawnPoints[i];
            string model = playerModels[i];
            KeyCode[] controlls = playerControlls[i];

            GameObject tempPlayer = Instantiate(player, new Vector3(sp.position.x, player.transform.position.y, sp.position.z), sp.rotation);
            PlayerController tempScript = tempPlayer.GetComponent<PlayerController>();

            tempScript.model = model;
            tempScript.forwardKey = controlls[0];
            tempScript.backKey = controlls[1];
            tempScript.leftKey = controlls[2];
            tempScript.rightKey = controlls[3];
            tempScript.bombKey = controlls[4];
        }
    }
}
