using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxPlay : MonoBehaviour
{
    [SerializeField] private SfxManager.Sound sound;
    [SerializeField] private bool playOnAwake;


    void Start()
    {
        if(playOnAwake)
            PlaySound();
    }


    public void PlaySound()
    {
        SfxManager.PlaySound(sound);
    }
}
