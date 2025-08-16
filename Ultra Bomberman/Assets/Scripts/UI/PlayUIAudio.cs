using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayUIAudio : MonoBehaviour
{
    private AudioSource sound;

    void Start()
    {
        sound = GetComponent<AudioSource>();
    }

    public void PlayAudio()
    {
        AudioSource.PlayClipAtPoint(sound.clip, new Vector3());
    }
}
