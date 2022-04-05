using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SelectLevel : MonoBehaviour
{
    [SerializeField] Button[] buttons;
    bool levelSelected;
    private void Start()
    {
        Manager_Game.Instance.LoadSectionGameMemory();
        levelSelected = false;
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i < Manager_Game.Instance.sectionGameData.GetCurrentLevel())
            {
                buttons[i].interactable = true;
            }
        }
    }
    public void SelectedLevel(int level)
    {
        if (levelSelected) 
            return;
        Manager_Game.Instance.InitialNewLevelGame(level);
        SceneManager.LoadScene("LoadGameScene");
        levelSelected = true;
    }

    public void BTN_BackButton()
    {
        SceneManager.LoadScene("menu_inicial");
        
    }


}
