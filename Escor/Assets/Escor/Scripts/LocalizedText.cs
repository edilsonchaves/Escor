using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LocalizedText : MonoBehaviour
{
    public string keyDefault;
    [SerializeField]TextMeshProUGUI text;
    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
    void SetupLanguage(int value)
    {
        Debug.Log(gameObject.name);
        Debug.Log("Localize key:" + keyDefault);
        text.text = CSVParser.GetTextFromID(keyDefault, value);
        Debug.Log(text.text);
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
