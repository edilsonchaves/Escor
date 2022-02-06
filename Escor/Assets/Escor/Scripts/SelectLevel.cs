using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SelectLevel : MonoBehaviour
{
    public void SelectedLevel(int level)
    {
        Manager_Game.Instance.InitialNewLevelGame(level);
        SceneManager.LoadScene("GameLevel");
    }

    public void BTN_BackButton()
    {
        SceneManager.LoadScene("menu_inicial");
    }
}
