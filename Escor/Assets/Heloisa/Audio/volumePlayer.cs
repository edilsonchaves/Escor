using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class volumePlayer : MonoBehaviour
{
    [SerializeField] private AudioSource VolumeAudio;
    [SerializeField] private AudioSource VozAudio;

    public static volumePlayer instance;

    private void Awake()
    {
        instance = this;
    }

    public void TocarVolume(AudioClip _musica)
    {
        VolumeAudio.clip = _musica;
        VolumeAudio.Play();
    }

    public void PararVolume()
    {
        VolumeAudio.Stop();
    }

    public void TocarVoz(AudioClip _efeitoSonoro)
    {
        VozAudio.PlayOneShot(_efeitoSonoro);
    }

    public void PararVoz()
    {
        VozAudio.Stop();
    }
}
