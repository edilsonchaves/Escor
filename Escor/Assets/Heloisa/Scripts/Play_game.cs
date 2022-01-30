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
		popup.InitPopup("Voc� possui um jogo salvo. Deseja continuar?", "Sim", () => CarregarJogo(), "N�o", () => Debug.Log("Novo jogo"));		
	}
	void CarregarJogo()
    {
        if (Manager_Game.Instance.levelData.LevelGaming == 0)
        {
			SceneManager.LoadScene("SelectLevel");
        }
        else
        {
			SceneManager.LoadScene("GameLevel");
		}
    }

	void NovoJogo() 
	{
		SceneManager.LoadScene("Prologo");

	}
	public void BTN_Config()
	{
		SceneManager.LoadScene("menu_config");
	}

	public void BTN_Credits()
	{
		SceneManager.LoadScene("CreditsScene");
	}

	public void BTN_Exit()
	{
		popup.InitPopup("Voc� realmente deseja sair do Jogo?", "Sim", ExitAction, "N�o", () => Debug.Log(""));
		
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