using UnityEngine;
using TMPro;

public class DefinidorTamanhoLetra : MonoBehaviour
{
    
    private TextMeshProUGUI _text;
    
    
    

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        UpdateSize();

        //Salvar o método de atualizar o tamanho no delegate
        configurações.SizeChangeDelegate += UpdateSize;
    }

    private void OnDisable()
    {
        //Remove o método de atualizar o tamanho no delegate
        configurações.SizeChangeDelegate -= UpdateSize;
    }

    public void UpdateSize()
    {
        _text.fontSize = PlayerPrefs.GetFloat("tamanholetra", 20f);
    }
}
