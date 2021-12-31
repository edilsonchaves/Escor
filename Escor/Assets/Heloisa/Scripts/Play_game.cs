using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Play_game : MonoBehaviour
{
	public void CarregarJogo()
	{
		SceneManager.LoadScene("SelectLevel");
		Manager_Game.Instance.LoadSectionGame();
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
		Application.Quit();
	}

	public void Voltar()
	{
		SceneManager.LoadScene("menu_inicial");
	}
}