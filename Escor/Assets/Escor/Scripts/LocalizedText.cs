using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LocalizedText : MonoBehaviour
{
    public string keyDefault;
    TextMeshProUGUI text;
    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
    void SetupLanguage(int value)
    {
        text.text = CSVParser.GetTextFromID(keyDefault, value);
    }
    public void Setup(string key, int value)
    {
        text.text = CSVParser.GetTextFromID(key, value);
    }
    private void OnEnable()
    {
        ManagerEvents.GameConfig.onChangeLanguage += SetupLanguage;
    }
    private void OnDisable()
    {
        ManagerEvents.GameConfig.onChangeLanguage -= SetupLanguage;

    }
}
