using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
public class Slider_Controller : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider slider;

    private void Start()
    {
        mixer.SetFloat("volume", slider.value);
    }

    public void AjustaVolume(float volume)
    {
        mixer.SetFloat("volume", volume);
    }

}
