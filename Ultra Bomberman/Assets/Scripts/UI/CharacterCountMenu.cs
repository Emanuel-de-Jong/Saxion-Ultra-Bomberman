using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterCountMenu : MonoBehaviour
{
    [SerializeField] GameObject characterCount;
    [SerializeField] int characterCountNumber = 2;
    [SerializeField] int characterCountMin= 2;
    [SerializeField] int characterCountMax = 4;

    private TextMeshProUGUI characterCountTMP;

    void Start()
    {
        characterCountTMP = characterCount.GetComponent<TextMeshProUGUI>();
        UpdateCharacterCount();
    }

    public void DecreaseCharacterCount()
    {
        if ((characterCountNumber - 1) >= characterCountMin)
        {
            characterCountNumber--;
            UpdateCharacterCount();
        }
    }

    public void IncreaseCharacterCount()
    {
        if ((characterCountNumber + 1) <= characterCountMax)
        {
            characterCountNumber++;
            UpdateCharacterCount();
        }
    }

    private void UpdateCharacterCount()
    {
        characterCountTMP.text = characterCountNumber.ToString();
    }

    public void StartGame()
    {
        G.characterCount = characterCountNumber;
        SceneManager.LoadScene("Level1Scene", LoadSceneMode.Single);
    }
}
