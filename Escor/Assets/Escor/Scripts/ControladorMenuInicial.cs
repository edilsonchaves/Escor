using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControladorMenuInicial : MonoBehaviour
{

    [Header("ConfigVolume")]
    [SerializeField] private Slider volumeSlider = null;

    [SerializeField] private GameObject confirmacaoPrompt = null;

    [Header("Carregamento de Jogo")]


    public string _novoJogo;
    private string jogoPraCarregar;
    [SerializeField] private GameObject nenhumJogoSalvo = null;

    public void NovoJogoSim()
    {
        SceneManager.LoadScene(_novoJogo);
    }

    public void CarregarJogoSim()
    {
        if (PlayerPrefs.HasKey("JogoSalvo"))
        {
            jogoPraCarregar = PlayerPrefs.GetString("JogoSalvo");
            SceneManager.LoadScene(jogoPraCarregar);
        } else
        {
            nenhumJogoSalvo.SetActive(true);
        }
    }
   
}
