using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorVolume : MonoBehaviour
{
    public float volumeMenu, volumeVoz;
    public Slider sliderVolMenu, sliderVoz;
    void Start()
    {
        sliderVolMenu.value = PlayerPrefs.GetFloat("menuVolume");
        sliderVoz.value = PlayerPrefs.GetFloat("vozMenu");
        DontDestroyOnLoad(gameObject);

        GameObject audioSourceDoObj = GameObject.Find("AudioMG");
        AudioSource source = audioSourceDoObj.GetComponent<AudioSource>();

        source.Play();
        source.Stop();        


        if (SceneManager.GetActiveScene().name == "menu_inicial")
        {
            source.Play();
        } 

        else
        {
            source.Stop();
        }

    }

   

    public void VolumeMenu(float volume)
    {
        volumeMenu = volume;
        AudioListener.volume = volumeMenu;

        PlayerPrefs.SetFloat("menuVolume", volumeMenu);
    }

    public void VolumeVoz(float volume)
    {
        
        volumeVoz = volume;
        GameObject[] vozes = GameObject.FindGameObjectsWithTag("VozMenu");        
        for(int i = 0; i < vozes.Length; i++)
        {
            vozes[i].GetComponent<AudioSource>().volume = volumeVoz;
        }

        PlayerPrefs.SetFloat("vozMenu", volumeVoz);
    }
}
