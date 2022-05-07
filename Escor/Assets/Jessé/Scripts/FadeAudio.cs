using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// [Jessé]
public class FadeAudio : MonoBehaviour
{
    public static FadeAudio Fade;

    Dictionary<AudioSource, Coroutine> coroutines = new Dictionary<AudioSource, Coroutine>();

    void Awake()
    {
        if(Fade!=null)
            DestroyImmediate(this);

        Fade = this;
    }


    public void Out(AudioSource audioSource, float time=2)
    {
        Coroutine cou = StartCoroutine(FadeOutMusic_(audioSource, time));

        if(coroutines.ContainsKey(audioSource))
        {
            StopCoroutine(coroutines[audioSource]);
            coroutines[audioSource] = cou;
        }
        else
        {
            coroutines.Add(audioSource, cou);
        }

    }

    IEnumerator FadeOutMusic_(AudioSource audioSource, float time=2)
    {
        float aux = audioSource.volume;
        while(aux > 0)
        {
            aux -= Time.deltaTime/time;
            audioSource.volume = aux;
            yield return null;
        }
        audioSource.volume = 0;
    }


    public void In(AudioSource audioSource, float maxVolume=1, float time=2)
    {
        Coroutine cou = StartCoroutine(FadeInMusic_(audioSource, maxVolume, time));

        if(coroutines.ContainsKey(audioSource))
        {
            StopCoroutine(coroutines[audioSource]);
            coroutines[audioSource] = cou;
        }
        else
        {
            coroutines.Add(audioSource, cou);
        }
    }



    IEnumerator FadeInMusic_(AudioSource audioSource, float maxVolume=1, float time=2)
    {
        if(!audioSource.isPlaying)
            audioSource.Play();

        float aux = 0;
        audioSource.volume = 0;
        while(aux < maxVolume)
        {
            aux += Time.deltaTime/time;
            audioSource.volume = aux;
            yield return null;
        }
        audioSource.volume = maxVolume;
    }

}
