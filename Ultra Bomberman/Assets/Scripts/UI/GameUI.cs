using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] GameObject[] players;
    [SerializeField] GameObject[] playerUIs;

    private PlayerController[] playerControllers;
    private TextMeshProUGUI[] playerLifes;

    void Start()
    {
        playerControllers = new PlayerController[players.Length];
        playerLifes = new TextMeshProUGUI[players.Length];

        for (int i=0; i<players.Length; i++)
        {
            GameObject player = players[i];
            if (player.activeSelf)
            {
                PlayerController playerController = player.GetComponent<PlayerController>();
                playerControllers[i] = playerController;

                GameObject playerUI = playerUIs[i];
                playerUI.SetActive(true);

                TextMeshProUGUI life = playerUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
                life.text = playerController.health.ToString();
                playerLifes[i] = life;
            }
        }
    }

    public void SetHealth(CharacterController playerController)
    {
        playerLifes[playerController.playerNumber - 1].text = playerController.health.ToString();
    }

    public void HidePlayerUIs(CharacterController playerController)
    {
        playerUIs[playerController.playerNumber - 1].SetActive(false);
    }
}
