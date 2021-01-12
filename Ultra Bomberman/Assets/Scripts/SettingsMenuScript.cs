using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenuScript : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;

    private float musicVolume = 1f;

    public void UpdateVolume()
    {
        musicSource.volume = musicVolume;
    }
}
