using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class menupause : MonoBehaviour
{

    [SerializeField]Popup _popup;
    public GameObject pauseMenuUI;
    public Text textPercentual, textCoins, textLife;

    private void Start()
    {
    }
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
    }

    void Pause(int percentualComplete, int Coins, int LifePlayer)
    {
        pauseMenuUI.SetActive(true);
        textPercentual.text = percentualComplete + "/0";
        textCoins.text = Coins + "/0";
        textLife.text = LifePlayer + "/0";
    }

    public void BTN_SalvarJogo()
    {
        ManagerEvents.UIConfig.SavedGame();
    }

    public void BTN_VoltarMenu()
    {
        ManagerEvents.UIConfig.ResumedGame();
        ManagerEvents.UIConfig.ReturnedMenu();
    }
    public void BTN_VoltarJogo()
    {
        ManagerEvents.UIConfig.ResumedGame();
    }
    public void BTN_MenuConfig()
    {
        Debug.Log("Configurações Jogo");

    }
    public void BTN_Exit()
    {
        Debug.Log("Sair Jogo");
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










