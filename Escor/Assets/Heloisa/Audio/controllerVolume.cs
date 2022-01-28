using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class controllerVolume : MonoBehaviour
{
    [SerializeField] private AudioMixer masterMixer;

    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Slider vozSlider;

    private void Start()
    {
        Volumebgsound();
        Volumevozsound();
    }

    public void Volumebgsound()
    {
        masterMixer.SetFloat("bgsound", volumeSlider.value);
    }

    public void Volumevozsound()
    {
        masterMixer.SetFloat("vozsound", vozSlider.value);
    }
}