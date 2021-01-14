using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeSliderScript : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    public void UpdateVolume(float volume)
    {
        audioSource.volume = volume;
    }
}
