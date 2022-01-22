using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizedText : MonoBehaviour
{
    public string keyDefault;
    Text text;
    private void Awake()
    {
        text = GetComponent<Text>();
    }
    void SetupLanguage(int value)
    {
        text.text = CSVParser.GetTextFromID(keyDefault, value );
    }
    public void Setup(string key,int value)
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
