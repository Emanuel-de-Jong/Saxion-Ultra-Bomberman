using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterUI : MonoBehaviour
{
    [SerializeField] int characterNumber = 1;

    private GameObject character;
    private CharacterController characterScript;
    private TextMeshProUGUI lifesText;

    void Start()
    {
        if (G.characterCount < characterNumber)
        {
            gameObject.SetActive(false);
            return;
        }

        character = GameObject.Find("Character" + characterNumber);

        characterScript = character.GetComponent<CharacterController>();
        characterScript.takeDamager.AddListener(SetHealth);
        characterScript.die.AddListener(HideCharacterUIs);

        lifesText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        lifesText.text = characterScript.startHealth.ToString();
    }

    public void SetHealth(CharacterController characterController)
    {
        lifesText.text = characterController.health.ToString();
    }

    public void HideCharacterUIs(CharacterController characterController)
    {
        gameObject.SetActive(false);
    }
}
