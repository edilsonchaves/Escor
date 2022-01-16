using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageAdaptScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ManagerEvents.GameConfig.ChangedLanguage(Manager_Game.Instance.saveGameData.LanguageSelect);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
