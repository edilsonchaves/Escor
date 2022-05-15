using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Audio;
// using System.Collections.Generic;

public class configurações : MonoBehaviour
{
    [SerializeField] private Slider volume = null;
    [SerializeField] private Slider volumeVoz = null;
    [SerializeField] private Slider _fontSlider;
    public AudioSource musicaMenu;
    public AudioSource[] vozDoJogo;

    public AudioMixer volumeJogo;
    int currentFontSize=0;

    private void Start()
    {
        _fontSlider.value   = Manager_Game.Instance.saveGameData.LetterSize;
        volume.value        = Manager_Game.Instance.saveGameData.VolumeAmbient;
        volumeVoz.value     = Manager_Game.Instance.saveGameData.Volume;

        // ---
        volumeJogo.SetFloat("vozsound", -80 + Manager_Game.Instance.saveGameData.Volume * 0.8f); // Range de volume.value é [0, 100]
        volumeJogo.SetFloat("bgsound", -80 + Manager_Game.Instance.saveGameData.VolumeAmbient * 0.8f); // Range de volume.value é [0, 100]

        StartCoroutine("UpdateValuesFromSlider");
        // ---

        salvarAsConfig();
        ChangeFontSize();


    }
    // ---
    private IEnumerator UpdateValuesFromSlider()
    {
        while(true)
        {
            Manager_Game.Instance.saveGameData.LetterSize      = (int) _fontSlider.value;
            Manager_Game.Instance.saveGameData.VolumeAmbient   = (int) volume.value;
            Manager_Game.Instance.saveGameData.Volume          = (int) volumeVoz.value;
            if (currentFontSize != (int)_fontSlider.value)
            {
                Debug.Log((int)_fontSlider.value);
                ManagerEvents.GameConfig.ChangedLanguageSize((int)_fontSlider.value);
                currentFontSize = (int)_fontSlider.value;
            }
            if (volumeVoz.value == 0)
            {
                volumeJogo.SetFloat("vozsound", -80); // Range de volume.value é [0, 100]

            }
            else
            {
                volumeJogo.SetFloat("vozsound", volumeVoz.value); // Range de volume.value é [0, 100]

            }

            if (volume.value == -40)
            {
                volumeJogo.SetFloat("bgsound", -80); // Range de volume.value é [0, 100]

            }
            else
            {
                volumeJogo.SetFloat("bgsound", volume.value); // Range de volume.value é [0, 100]

            }


            yield return null;
        }
    }
    // ---

    public void ChangeFontSize()
    {

        //Muda o tamanho das fontes visíveis
        if (currentFontSize == (int)_fontSlider.value)
            return;
        Debug.Log((int)_fontSlider.value);
        ManagerEvents.GameConfig.ChangedLanguageSize((int)_fontSlider.value);
    }

    private void salvarAsConfig()
    {

        //musicaMenu.volume = volumeFloat;

        for (int i = 0; i < vozDoJogo.Length; i++)
        {
            //vozDoJogo[i].volume = vozFloat;
        }
    }

    public void BTN_Voltar()
    {
        Manager_Game.Instance.saveGameData.LetterSize = (int)_fontSlider.value;
        Manager_Game.Instance.saveGameData.VolumeAmbient= (int)volume.value;
        Manager_Game.Instance.saveGameData.Volume= (int)volumeVoz.value;
        SaveLoadSystem.SaveFile<GameData>(Manager_Game.Instance.saveGameData);
    }
}
