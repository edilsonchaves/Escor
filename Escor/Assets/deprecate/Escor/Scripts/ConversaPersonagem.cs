using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ConversaPersonagem : MonoBehaviour
{
    private List<Conversa> demonstrarConversa;
    [SerializeField] Image dialogoImagem;
    [SerializeField] TextMeshProUGUI dialogoTexto;
    [SerializeField] AudioSource dialogoAudio;
    [SerializeField] GameObject prefab;
    bool _statusConversa;
    public bool StatusConversa { get { return _statusConversa; } }
    bool currentAudioFinished;
    public IEnumerator  ConversaFase(List<Conversa> conversa)
    {
        _statusConversa = false;
        Movement.KeepPlayerStopped(); // parar o player durante o diálogo [jessé]
        // Movement.canMove = false; // parar o player durante o diálogo [jessé]
        prefab.SetActive(true);
        foreach ( var estrofe in conversa)
        {
            currentAudioFinished = false;
            dialogoImagem.sprite = estrofe.personagemImagem;
            dialogoTexto.text = estrofe.personagemFalaTexto;
            dialogoAudio.clip = estrofe.personagemFalaAudio;
            dialogoAudio.Play();
            Coroutine checkAudioFinished = StartCoroutine("AudioFinished");
            // if (LevelManager.levelstatus != LevelManager.LevelStatus.Game)
            // {
            //     prefab.SetActive(false);
            //     yield return new WaitUntil(()=>LevelManager.levelstatus==LevelManager.LevelStatus.Game);
            //     prefab.SetActive(true);
            // }

            yield return new WaitUntil(()=>currentAudioFinished);
            StopCoroutine(checkAudioFinished);
            // yield return new WaitUntil(()=>!dialogoAudio.isPlaying);
        }
        _statusConversa = true;
        // Movement.canMove = true; // player pode se mover [jessé]
        Movement.StopKeepPlayerStopped();; // player pode se mover [jessé]
        prefab.SetActive(false);

    }


    IEnumerator AudioFinished()
    {
        currentAudioFinished = false;

        while(dialogoAudio.isPlaying)
        {
            if(LevelManager.levelstatus != LevelManager.LevelStatus.Game)
            {
                dialogoAudio.Pause();
                prefab.SetActive(false);

                yield return new WaitUntil(()=>LevelManager.levelstatus==LevelManager.LevelStatus.Game);

                prefab.SetActive(true);
                dialogoAudio.Play();
            }

            yield return null;
        }

        currentAudioFinished = true;

        yield return null;
    }
}

[System.Serializable]
public class Conversa
{
    public Sprite personagemImagem;
    public AudioClip personagemFalaAudio;
    [TextArea(1,4)]public string personagemFalaTexto;
}
