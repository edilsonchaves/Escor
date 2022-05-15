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
    public IEnumerator  ConversaFase(List<Conversa> conversa)
    {
        _statusConversa = false;
        Movement.KeepPlayerStopped(); // parar o player durante o diálogo [jessé]
        // Movement.canMove = false; // parar o player durante o diálogo [jessé]
        prefab.SetActive(true);
        foreach ( var estrofe in conversa)
        {
            dialogoImagem.sprite = estrofe.personagemImagem;
            dialogoTexto.text = estrofe.personagemFalaTexto;
            dialogoAudio.clip = estrofe.personagemFalaAudio;
            dialogoAudio.Play();
            yield return new WaitUntil(()=>!dialogoAudio.isPlaying);
        }
        _statusConversa = true;
        // Movement.canMove = true; // player pode se mover [jessé]
        Movement.StopKeepPlayerStopped();; // player pode se mover [jessé]
        prefab.SetActive(false);

    }
}

[System.Serializable]
public class Conversa
{
    public Sprite personagemImagem;
    public AudioClip personagemFalaAudio;
    [TextArea(1,4)]public string personagemFalaTexto;
}
