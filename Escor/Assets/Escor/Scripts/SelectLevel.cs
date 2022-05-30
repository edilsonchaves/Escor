using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SelectLevel : MonoBehaviour
{
    [SerializeField] Button[] buttons;
    [SerializeField] Image[] cutScene;
    bool levelSelected;
    private void Start()
    {
        
        Manager_Game.Instance.LoadSectionGameMemory();
        string[] shardsMemoryStatus = Manager_Game.Instance.sectionGameData.GetMemoryFragment();
        levelSelected = false;
        for (int i = 0; i < buttons.Length; i++)
        {
            if (i < Manager_Game.Instance.sectionGameData.GetCurrentLevel())
            {
                buttons[i].interactable = true;
                if (shardsMemoryStatus[i].Length > 0)
                {
                    cutScene[i].fillAmount = PercentualMemoryFound(shardsMemoryStatus[i]);
                }
            }
        }
    }

    public float PercentualMemoryFound(string memoryStatus)
    {
        float count=0;
        for(int i = 0; i < memoryStatus.Length; i++)
        {
            if (memoryStatus[i].Equals('0'))
                count++;
        }
        float percentual = count / memoryStatus.Length;
        Debug.Log("count: " + count + " lenght: " + memoryStatus.Length+" percentual: "+ percentual);
        return count/ memoryStatus.Length;
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
