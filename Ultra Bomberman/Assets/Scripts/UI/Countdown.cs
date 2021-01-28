using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    public int currentTime;

    private TextMeshProUGUI textMesh;

    void Start()
    {
        if (!G.train)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        textMesh = GetComponent<TextMeshProUGUI>();

        textMesh.text = currentTime.ToString();
        InvokeRepeating(nameof(UpdateCountdown), 1, 1);
    }

    private void UpdateCountdown()
    {
        currentTime--;
        textMesh.text = currentTime.ToString();
    }
}
