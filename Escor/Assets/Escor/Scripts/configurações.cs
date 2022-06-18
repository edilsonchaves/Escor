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
        volume.value        = VolumeRangeToUnit(Manager_Game.Instance.saveGameData.VolumeAmbient);
        volumeVoz.value     = VolumeRangeToUnit(Manager_Game.Instance.saveGameData.Volume);

        // [Jessé]
        volumeJogo.SetFloat("vozsound", UnitToVolumeRange(volumeVoz.value)); 
        volumeJogo.SetFloat("bgsound", UnitToVolumeRange(volume.value));


        StartCoroutine("UpdateValuesFromSlider");

        salvarAsConfig();
        ChangeFontSize();


    }

    // [Jessé] 

    // converte o volume para um valor entre [0, -80]
    // o grafico não é uma reta, e sim um arco, onde nao sofre tanta alteração perto do 0
    float UnitToVolumeRange(float unit)
    {
        return -Mathf.Abs(Mathf.Pow(unit, 3)*80); // quanto mais perto de -1, mais o volume diminui
    }


    // converte o volume para um valor entre [0, -1]
    // o grafico não é uma reta, e sim um arco, onde nao sofre tanta alteração perto do 0
    float VolumeRangeToUnit(float volume)
    {
        return -Mathf.Abs(Mathf.Pow(volume/-80, 1.0f/3.0f));
    }


    private IEnumerator UpdateValuesFromSlider()
    {
        while(true)
        {

            // [Jessé] 
            volumeJogo.SetFloat("vozsound", UnitToVolumeRange(volumeVoz.value)); 
            volumeJogo.SetFloat("bgsound", UnitToVolumeRange(volume.value));

            ManagerEvents.GameConfig.ChangedLanguageSize((int)_fontSlider.value);
            currentFontSize = (int)_fontSlider.value;

            yield return null;
        }
    }
    // ---

    public void ChangeFontSize()
    {

        //Muda o tamanho das fontes visíveis
        // if (currentFontSize == (int)_fontSlider.value)
            // return;
        // Debug.Log((int)_fontSlider.value);
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
        Manager_Game.Instance.saveGameData.LetterSize    = (int)_fontSlider.value;
        Manager_Game.Instance.saveGameData.VolumeAmbient = (int)UnitToVolumeRange(volume.value);
        Manager_Game.Instance.saveGameData.Volume        = (int)UnitToVolumeRange(volumeVoz.value);

        SaveLoadSystem.SaveFile<GameData>(Manager_Game.Instance.saveGameData);
    }
}
