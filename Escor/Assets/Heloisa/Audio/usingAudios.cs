using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class usingAudios : MonoBehaviour
{
    [SerializeField] private AudioClip volumeBack;
    [SerializeField] private AudioClip volumeCabrinha;

    public void TocarBack()
    {
        volumePlayer.instance.TocarVolume(volumeBack);
    }

    public void TocarCabrinha()
    {
        volumePlayer.instance.TocarVoz(volumeCabrinha);
    }

    public void PararMusica()
    {
        volumePlayer.instance.PararVolume();
    }

    public void PararCabrinha()
    {
        volumePlayer.instance.PararVoz();
    }

}
