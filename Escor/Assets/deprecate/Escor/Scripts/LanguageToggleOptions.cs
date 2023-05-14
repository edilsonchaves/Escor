using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class LanguageToggleOptions : MonoBehaviour
{
    [SerializeField] private ToggleGroup tg;
    private List<Toggle> toggles;
    // Start is called before the first frame update
    void Start()
    {
        InitializeToggles();
        PopulateToggles(CSVParser.GetAvaibleLanguages());
    }
    void Update()
    {
         int value=VerifyToggleIsOn();
        if (value == Manager_Game.Instance.saveGameData.LanguageSelect)
            return;
        Manager_Game.Instance.saveGameData.LanguageSelect = value;
        ManagerEvents.GameConfig.ChangedLanguage(value);
        PopulateToggles(CSVParser.GetAvaibleLanguages());
    }
    void InitializeToggles()
    {
        toggles = new List<Toggle>(GetComponentsInChildren<Toggle>()); 
    }
    int VerifyToggleIsOn()
    {
        int value = -1; ;
        foreach (Toggle toggle in toggles)
        {
            if (toggle.isOn)
            {
                value=toggle.GetComponent<toggleScript>().Value;
            }
        }
        return value;
    }
    void PopulateToggles(List<string> textLinguas)
    {
        int i = 0;
        foreach (Toggle toggle in toggles)
        {
            toggle.GetComponent<toggleScript>().SetText(textLinguas[i]);
            i++;
        }
    }
}
