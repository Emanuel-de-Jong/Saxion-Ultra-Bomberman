using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public int currentTime;

    [SerializeField] TextMeshProUGUI countdown;
    [SerializeField] GameObject[] players;
    [SerializeField] GameObject[] playerUIs;

    private PlayerController[] playerControllers;
    private TextMeshProUGUI[] playerLifes;

    void Start()
    {
        countdown.text = currentTime.ToString();
        InvokeRepeating(nameof(UpdateCountdown), 1, 1);

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

    private void UpdateCountdown()
    {
        currentTime--;
        countdown.text = currentTime.ToString();
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
