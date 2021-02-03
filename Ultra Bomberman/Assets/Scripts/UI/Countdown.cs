using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    [HideInInspector]
    public int startTime = 90;

    private int currentTime;
    private TextMeshProUGUI textMesh;

    private void Start()
    {
        if (!G.train)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        startTime = GameObject.Find("GameController").GetComponent<GameController>().roundDuration;
        Reset();

        textMesh = GetComponent<TextMeshProUGUI>();
        textMesh.text = currentTime.ToString();
        InvokeRepeating(nameof(UpdateCountdown), 1, 1);
    }

    public void Reset()
    {
        currentTime = startTime;
    }

    private void UpdateCountdown()
    {
        currentTime--;
        if (currentTime >= 0)
            textMesh.text = currentTime.ToString();
    }
}
