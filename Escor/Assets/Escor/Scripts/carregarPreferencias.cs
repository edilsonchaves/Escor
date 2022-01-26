using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class carregarPreferencias : MonoBehaviour
{
    [Header("Configura��es")]
    [SerializeField] private bool usar = false;
    [SerializeField] private MenuConfigApply menuControlador;

    [Header("Configura��es Volume")]
    [SerializeField] private Slider volumeVozSlider = null;

    [Header("Configura��es Volume da Voz")]
    [SerializeField] private Slider volumeSlider = null;
    

    private void Awake()
    {
        if (usar)
        {
            if (PlayerPrefs.HasKey("volumePrincipal"))
            {
                float localVolume = PlayerPrefs.GetFloat("volumePrincipal");

                volumeSlider.value = localVolume;
                AudioListener.volume = localVolume;
            } 
        }

        if(PlayerPrefs.HasKey("volumeVoz"))
        {
            float localVolume = PlayerPrefs.GetFloat("volumeVoz");

            volumeSlider.value = localVolume;
            AudioListener.volume = localVolume;
        }
    }

}
