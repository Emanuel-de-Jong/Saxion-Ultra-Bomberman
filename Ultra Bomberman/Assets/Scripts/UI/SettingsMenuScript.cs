using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenuScript : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;

    public void UpdateMusicVolume(float volume)
    {
        musicSource.volume = volume;
        G.musicVolume = volume;
    }

    public void UpdateEffectVolume(float volume)
    {
        G.effectVolume = volume;
    }
}
