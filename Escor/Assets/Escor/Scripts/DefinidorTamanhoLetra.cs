using UnityEngine;
using TMPro;
using UnityEngine;

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
        FontSize.SizeChangeDelegate += UpdateSize;
    }

    private void OnDisable()
    {
        //Remove o método de atualizar o tamanho no delegate
        FontSize.SizeChangeDelegate -= UpdateSize;
    }

    private void UpdateSize()
    {
        _text.fontSize = PlayerPrefs.GetFloat("font_size", 60f);
    }
}
