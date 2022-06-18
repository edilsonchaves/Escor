using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class GameAssets : MonoBehaviour
{
    static GameAssets _i;
    public static GameAssets i
    {
        get
        {
            if (_i == null)
            {
                _i = (Instantiate(Resources.Load("GameAssets"))as GameObject).GetComponent<GameAssets>();
                DontDestroyOnLoad(_i.gameObject);
            }
            return _i;
        }
    }
    public SFXAudioClip[] sfxAudiosClips;
    [System.Serializable]
    public class SFXAudioClip
    {
        public SfxManager.Sound sound;
        public AudioClip audioClip;
        public AudioMixerGroup audioMixer;
    }
}