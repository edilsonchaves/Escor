using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuConfigApply : MonoBehaviour
{

    [Header("ConfigVolume")]
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private Slider volumeVoz = null;

    [SerializeField] private GameObject confirmacaoPrompt = null;
    public void configVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public void VolumeAplicar()
    {
        PlayerPrefs.SetFloat("volumePrincipal", AudioListener.volume);
        StartCoroutine(caixaDeConfirmacao());
    }

    public void VolumeVozAplicar()
    {
        PlayerPrefs.SetFloat("volumeVoz", AudioListener.volume);
        StartCoroutine(caixaDeConfirmacao());
    }

    public IEnumerator caixaDeConfirmacao()
    {
        confirmacaoPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmacaoPrompt.SetActive(false);
    }

}
