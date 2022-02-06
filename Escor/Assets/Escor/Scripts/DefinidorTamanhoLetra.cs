using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DefinidorTamanhoLetra : MonoBehaviour
{

    //private TextMeshProUGUI _text;
    private Text _text;
    
    

    private void Awake()
    {
        _text = GetComponent<Text>();
    }

    private void OnEnable()
    {
        UpdateSize();

        //Salvar o m�todo de atualizar o tamanho no delegate
        configura��es.SizeChangeDelegate += UpdateSize;
    }

    private void OnDisable()
    {
        //Remove o m�todo de atualizar o tamanho no delegate
        configura��es.SizeChangeDelegate -= UpdateSize;
    }

    public void UpdateSize()
    {
        _text.fontSize = Manager_Game.Instance.saveGameData.LetterSize;
    }
}
