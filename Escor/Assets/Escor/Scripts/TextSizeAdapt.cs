using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TextSizeAdapt : MonoBehaviour
{
    TextMeshProUGUI text;
    int minLanguageSize;
    [SerializeField] int maxLanguageletterAdapt;
    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        minLanguageSize = (int)text.fontSize;
    }
    private void OnEnable()
    {
        ManagerEvents.GameConfig.onChangeLanguageSize += SetupLanguageSize;
    }
    private void OnDisable()
    {
        ManagerEvents.GameConfig.onChangeLanguageSize -= SetupLanguageSize;

    }


    void SetupLanguageSize(int value)
    {
        text.fontSize = minLanguageSize + Mathf.RoundToInt(maxLanguageletterAdapt*value/100);
    }
}
