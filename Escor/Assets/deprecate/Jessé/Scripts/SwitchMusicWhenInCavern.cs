using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SwitchMusicWhenInCavern : MonoBehaviour
{
    [SerializeField] private AudioSource musicInsideCavern;

    private LevelManager levelManager;

    // Start is called before the first frame update
    void Start()
    {
        if(!musicInsideCavern)
            musicInsideCavern = GetComponent<AudioSource>();

        // tem atualizar o volume

        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();

    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            FadeAudio.Fade.In(musicInsideCavern);
            levelManager.FadeOutMusic(2);
        }
    }


    void OnTriggerExit2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            levelManager.FadeInMusic(2);
            FadeAudio.Fade.Out(musicInsideCavern);
        }
    }
}
