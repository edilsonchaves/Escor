using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class menupause : MonoBehaviour
{

    [SerializeField]Popup popupTeste;
    public GameObject pauseMenuUI;
    public Text textPercentual, textCoins, textLife; 


    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        //gameIsPaused = false;
       
    }

    void Pause(int percentualComplete, int Coins, int LifePlayer)
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        textPercentual.text = percentualComplete + "/0";
        textCoins.text = Coins + "/0";
        textLife.text = LifePlayer + "/0";
    }
      

    public void menuConfig()
    {
        SceneManager.LoadScene("menu_config");
    }

    public void voltarMenu()
    {
        Debug.Log("Ola");
        ManagerEvents.UIConfig.ReturnedMenu();
    }

    public void Sair()
    {
        ManagerEvents.UIConfig.ExitedMenu();
    }
    private void OnEnable()
    {
        ManagerEvents.UIConfig.onPauseGame += Pause;
        ManagerEvents.UIConfig.onResumeGame += Resume;
    }
    private void OnDisable()
    {
        ManagerEvents.UIConfig.onPauseGame -= Pause;
        ManagerEvents.UIConfig.onResumeGame -= Resume;
    }

}










