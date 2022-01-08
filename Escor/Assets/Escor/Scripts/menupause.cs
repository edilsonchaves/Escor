using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using UnityEditor;

public class menupause : MonoBehaviour
{
    //private Popup _popup;
   // Action action1;
    //Action action2;
    //Action action3;
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;

   // private void Start()
    //{
     //   action1 = () => { };
    //    action2 = () => { };
     //   action3 = () => { };
   // }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            } else
            {
                Pause();
            }
        }

        
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
       
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }


   

    public void menuConfig()
    {
        SceneManager.LoadScene("menu_config");
    }

    public void voltarMenu()
    {
        SceneManager.LoadScene("menu_inicial");
    }

    public void Sair()
    {
        Application.Quit();
        Debug.Log("Saiu do jogo");
    }

    

}










