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
                PlayerController controller = player.GetComponent<PlayerController>();
                playerControllers[i] = controller;

                GameObject playerUI = playerUIs[i];
                playerUI.SetActive(true);

                TextMeshProUGUI life = playerUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
                life.text = controller.health.ToString();
                playerLifes[i] = life;
            }
        }
    }

    public void SetHealth(CharacterController controller)
    {
        playerLifes[controller.playerNumber - 1].text = controller.health.ToString();
    }

    public void HidePlayerUIs(CharacterController controller)
    {
        playerUIs[controller.playerNumber - 1].SetActive(false);
    }
}
