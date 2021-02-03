using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    private int currentTime;
    private TextMeshProUGUI textMesh;

    private void Start()
    {
        if (!G.train)
        {
            gameObject.SetActive(false);
            return;
        }

        G.gameController.reset.AddListener(Reset);

        Reset();

        textMesh = GetComponent<TextMeshProUGUI>();
        textMesh.text = currentTime.ToString();
        InvokeRepeating(nameof(UpdateCountdown), 0, 1);
    }

    public void Reset()
    {
        currentTime = G.roundDuration;
    }

    private void UpdateCountdown()
    {
        currentTime--;
        if (currentTime >= 0)
            textMesh.text = currentTime.ToString();
    }
}
