using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public static class SfxManager
{
    public enum Sound
    {
        playerMove,
        playerDefense,
        playerHurt,   // implementar com a função PlayRandomHurt()
        playerDie,
        playerJump,
        playerGetLife,
        playerGetNewPower,

        javaliMove,
        javaliAttack,
        javaliShoot,
        javaliStuned,
        javaliParado,

        gotaSplash_1,
        gotaSplash_2,
        gotaSplash_3,
        gotaSplash_4,

        armadilhaPedra,

        cordaPegar_1,
        cordaPegar_2,
        cordaBalancando,

        estalactiteBalancando,
        estalactiteDestroy,

        pegandoSino,  // implementar com a função PlayRandomSinoSound()
        pegandoSino_2,
        pegandoSino_3,
        pegandoSino_4,

        pegandoEasterEgg,

        playerHurt_2,  // implementar com a função PlayRandomHurt()
        playerHurt_3,
        playerHurt_4,

        playerMoveCaverna,
    }
    private static Dictionary<Sound, float> soundTimerDic;
    public static List<GameObject> TiposDeSom = new List<GameObject>();
    public static void Initialize()
    {
        soundTimerDic = new Dictionary<Sound, float>();
        soundTimerDic[Sound.playerMove] = 0;
        soundTimerDic[Sound.playerMoveCaverna] = 0;  // [Jessé]
    }
    private static bool CanPlaySound(Sound sound)
    {
        switch (sound)
        {
            default:
                return true;

            case Sound.playerMoveCaverna: // [Jessé]
            case Sound.playerMove:
                if (soundTimerDic.ContainsKey(sound))
                {
                    float lastTimePlayed = soundTimerDic[sound];
                    float playMoveTimerMax = 0.5f;
                    if (lastTimePlayed + playMoveTimerMax < Time.time)
                    {
                        soundTimerDic[sound] = Time.time;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
        }
    }
    public static void PlaySound(Sound sound)
    {
        if (CanPlaySound(sound))
        {
            foreach (GameObject som in TiposDeSom)
            {
                if (som.name == sound.ToString())
                {
                    som.GetComponent<AudioSource>().PlayOneShot(GetAudioClip(sound));
                    return;
                }
            }
            GameObject sfxManagerGO = new GameObject(sound.ToString());
            AudioSource sfxSource = sfxManagerGO.AddComponent<AudioSource>();
            sfxSource.outputAudioMixerGroup = GetAudioMixerGroup(sound);
            TiposDeSom.Add(sfxManagerGO);
            sfxSource.PlayOneShot(GetAudioClip(sound));
        }
    }

    private static AudioMixerGroup GetAudioMixerGroup(Sound sound)
    {
        foreach (GameAssets.SFXAudioClip sfxAudioClip in GameAssets.i.sfxAudiosClips)
        {
            if (sfxAudioClip.sound == sound)
            {
                return sfxAudioClip.audioMixer;
            }
        }

        Debug.LogError("Sound: " + sound + " not found");
        return null;
    }
    private static AudioClip GetAudioClip(Sound sound)
    {
        foreach(GameAssets.SFXAudioClip sfxAudioClip in GameAssets.i.sfxAudiosClips)
        {
            if (sfxAudioClip.sound == sound)
            {
                return sfxAudioClip.audioClip;
            }
        }

        Debug.LogError("Sound: " + sound + " not found");
        return null;
    }



    // [Jessé]
    // toca uma das 4 variações do som da gota de forma aleatória
    public static void PlayRandomGotaSplash()
    {
        Sound randomGotaSplash = new Sound[4]{
            Sound.gotaSplash_1,
            Sound.gotaSplash_2,
            Sound.gotaSplash_3,
            Sound.gotaSplash_4
        }[Random.Range(0,4)];

        // Debug.Log(randomGotaSplash);
        SfxManager.PlaySound(randomGotaSplash);
    }


    // [Jessé]
    // toca uma das 2 variações do som de pegar a corda de forma aleatória
    public static void PlayRandomCordaPegar()
    {
        Sound randomCordaPegar = new Sound[3]{
            Sound.cordaPegar_1,
            Sound.cordaPegar_1, // o som 1 parece melhor por isso dupliquei suas chaces de ser reproduzido
            Sound.cordaPegar_2,
        }[Random.Range(0,3)];

        // Debug.Log(randomCordaPegar);
        SfxManager.PlaySound(randomCordaPegar);
    }


    // [Jessé]
    // toca uma das 4 variações do som de tomando dano de forma aleatória
    public static void PlayRandomHurt()
    {
        // BUG
        // O som do dano não está sendo executado durante o som de ataque do javali
        // (já testei sem o som do javali e funcionou)

        Sound randomHurtSound = new Sound[4]{
            Sound.playerHurt,
            Sound.playerHurt_2,
            Sound.playerHurt_3,
            Sound.playerHurt_4,
        }[Random.Range(0,4)];

        // Debug.Log($"randomHurtSound: {randomHurtSound}");
        SfxManager.PlaySound(randomHurtSound);
    }


    // [Jessé]
    // toca uma das 4 variações do som de pegando sino de forma aleatória
    public static void PlayRandomSinoSound()
    {
        Sound randomPegarSinoSound = new Sound[4]{
            Sound.pegandoSino,
            Sound.pegandoSino_2,
            Sound.pegandoSino_3,
            Sound.pegandoSino_4,
        }[Random.Range(0,4)];

        // Debug.Log(randomPegarSinoSound);
        SfxManager.PlaySound(randomPegarSinoSound);
    }

}
