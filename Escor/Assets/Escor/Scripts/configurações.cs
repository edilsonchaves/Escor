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

    public delegate void OnSizeChange();
    public static OnSizeChange SizeChangeDelegate;
    public AudioSource musicaMenu;
    public AudioSource[] vozDoJogo;

    public AudioMixer volumeJogo;


    private void Start()
    {
        _fontSlider.value   = Manager_Game.Instance.saveGameData.LetterSize;
        volume.value        = Manager_Game.Instance.saveGameData.VolumeAmbient;
        volumeVoz.value     = Manager_Game.Instance.saveGameData.Volume;
        
        // ---
        volumeJogo.SetFloat("vozsound", -80 + volumeVoz.value * 0.8f); // Range de volume.value é [0, 100]
        volumeJogo.SetFloat("bgsound", -80 + volume.value * 0.8f); // Range de volume.value é [0, 100]
        
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
            ManagerEvents.GameConfig.ChangedLanguageSize ((int) _fontSlider.value);

            volumeJogo.SetFloat("vozsound", -80 + volumeVoz.value * 0.8f); // Range de volume.value é [0, 100]
            volumeJogo.SetFloat("bgsound", -80 + volume.value * 0.8f); // Range de volume.value é [0, 100]

            yield return null;
        }
    }
    // ---

    public void ChangeFontSize()
    {
        
        //Muda o tamanho das fontes visíveis
        if (SizeChangeDelegate != null)
        {
            SizeChangeDelegate.Invoke();
        }                
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
        SceneManager.LoadScene("menu_inicial");
    }
}
