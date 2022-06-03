using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TextSizeAdapt : MonoBehaviour
{
    TextMeshProUGUI text;
    [SerializeField]float minLanguageSize;
    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        minLanguageSize = (int)text.fontSize;
    }
    private void OnEnable()
    {
        ManagerEvents.GameConfig.onChangeLanguageSize += SetupLanguageSize;
        SetupLanguageSizeEnabled();
    }
    private void OnDisable()
    {
        ManagerEvents.GameConfig.onChangeLanguageSize -= SetupLanguageSize;

    }


    void SetupLanguageSize(float value)
    {
        float valueLetter = minLanguageSize * (value / 100);
        text.fontSize = Mathf.RoundToInt(minLanguageSize * (value/100));
    }
    void SetupLanguageSizeEnabled()
    {
        if(text.fontSize != Mathf.RoundToInt(minLanguageSize * (Manager_Game.Instance.saveGameData.LetterSize / 100)))
        text.fontSize = Mathf.RoundToInt(minLanguageSize * (Manager_Game.Instance.saveGameData.LetterSize/100));
    }
}
