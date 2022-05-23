using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class Play_game : MonoBehaviour
{
	private Popup popup;
	[SerializeField] private UIController uicontroller;


    private void Start()
    {
		popup = uicontroller.CreatePopup();
		popup.gameObject.SetActive(false);

	}
    public void BTN_Play()
	{
		Manager_Game.Instance.LoadSectionGameMemory();
		Manager_Game.Instance.LoadLevelGame();
		if (Manager_Game.Instance.sectionGameData!=null)
			popup.InitPopup("Você possui um jogo salvo. Deseja continuar?", "Sim", () => CarregarJogo(), "Não", NovoJogo);
        else
        {
			NovoJogo();
		}
	}
	void CarregarJogo()
    {
        if (Manager_Game.Instance.levelData.LevelGaming==0)
        {

			SceneManager.LoadScene("Prologo");
        }
        else
        {
			Manager_Game.Instance.LevelStatus = LevelInfo.LevelStatus.ContinueLevel;
			SceneManager.LoadScene("LoadGameScene");
		}
    }

	void NovoJogo() 
	{
		Manager_Game.Instance.InitialNewSectionGame();
		SceneManager.LoadScene("Prologo");

	}
	public void BTN_Config()
	{
		SceneManager.LoadScene("menu_config");
	}

	public void BTN_Credits()
	{
		Debug.Log("Teste");
		SceneManager.LoadScene("CreditsScene");
	}

	public void BTN_Exit()
	{
		popup.InitPopup("Você realmente deseja sair do Jogo?", "Sim", ExitAction, "Não", () => Debug.Log(""));
		
	}

	private void ExitAction()
	{		
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#else
                Application.Quit();
		#endif
	}


	public void BTN_Voltar()
	{
		SceneManager.LoadScene("menu_inicial");
	}
}