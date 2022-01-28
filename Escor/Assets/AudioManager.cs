using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private static readonly string VolumeDoJogo = "VolumeDoJogo";
    private static readonly string VolumePref = "VolumePref";
    private static readonly string VozPref = "VozPref";
    private int volumeDoJogo;
    public Slider volumeJogoSlider, volumeVozSlider;
    private float volumeFloat, vozFloat;
    public AudioSource musicaMenu;
    public AudioSource[] vozDoJogo;
    void Start()
    {
        volumeDoJogo = PlayerPrefs.GetInt(VolumeDoJogo);

        if(volumeDoJogo == 0)
        {
            volumeFloat = .25f;
            vozFloat = .75f;
            volumeJogoSlider.value = volumeFloat;
            volumeVozSlider.value = vozFloat;
            PlayerPrefs.SetFloat(VolumePref, volumeFloat);
            PlayerPrefs.SetFloat(VozPref, vozFloat);
            PlayerPrefs.SetInt(VolumeDoJogo, -1);
        } 
        
        else
        {
            volumeFloat = PlayerPrefs.GetFloat(VolumePref);
            volumeJogoSlider.value = volumeFloat;
            vozFloat = PlayerPrefs.GetFloat(VozPref);
            volumeVozSlider.value = vozFloat;
        }
    }

    public void SalvarConfigAudio()
    {
        PlayerPrefs.SetFloat(VolumePref, volumeJogoSlider.value);
        PlayerPrefs.SetFloat(VozPref, volumeVozSlider.value);
    }

    void NãoEstaMexendo(bool mexendo)
    {
        if (!mexendo)
        {
            SalvarConfigAudio();
        }
    }

    public void UpdateMusica()
    {
        musicaMenu.volume = volumeJogoSlider.value;

        for(int i = 0; i < vozDoJogo.Length; i++)
        {
            vozDoJogo[i].volume = volumeVozSlider.value;
        }
    } 
}
