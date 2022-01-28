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
    public void CarregarJogo()
	{
		popup.InitPopup("Voc� possui um jogo salvo. Deseja continuar?", "Sim", () => Debug.Log("Carregar jogo"), "N�o", () => Debug.Log("Novo jogo"));		
		//SceneManager.LoadScene("SelectLevel");
		//Manager_Game.Instance.LoadSectionGame();
		
	}

	public void CarregarConfig()
	{
		SceneManager.LoadScene("menu_config");
	}

	public void CarregarCreditos()
	{
		SceneManager.LoadScene("nome_cena_de_creditos");
	}

	public void Sair()
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


	public void Voltar()
	{
		SceneManager.LoadScene("menu_inicial");
	}
}