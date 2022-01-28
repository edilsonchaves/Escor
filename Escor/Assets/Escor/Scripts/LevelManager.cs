using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
public class LevelManager : MonoBehaviour
{
    public enum LevelStatus {Game, Pause, CutScene};
    public static LevelStatus levelstatus;
    private LevelStatus auxLevelStatus;
    [SerializeField]UIController control;
    [SerializeField] GameObject[] levelsAvaibles;
    Popup popup;
    void Start()
    {
        popup = control.CreatePopup();
        popup.gameObject.SetActive(false);
        CreateLevel(Manager_Game.Instance.levelData.LevelGaming);
    }

    void CreateLevel(int level=1)
    {
        Instantiate(levelsAvaibles[level-1],Vector3.zero,Quaternion.identity);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (levelstatus == LevelStatus.Pause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    private void OnEnable()
    {
        ManagerEvents.UIConfig.onReturnMenu += VoltarMenuPressButton;
        ManagerEvents.UIConfig.onExitMenu += ExitMenuPressButton;

    }
    private void OnDisable()
    {
        ManagerEvents.UIConfig.onReturnMenu -= VoltarMenuPressButton;
        ManagerEvents.UIConfig.onExitMenu -= ExitMenuPressButton;
    }
    void Resume()
    {
        ManagerEvents.UIConfig.ResumedGame();
        levelstatus = auxLevelStatus;

    }

    void Pause()
    {
        ManagerEvents.UIConfig.PausedGame(10, 20, 30);
        auxLevelStatus = levelstatus;
        levelstatus = LevelStatus.Pause;
    }

    void VoltarMenuPressButton()
    {
        popup.InitPopup("Você deseja salvar o progresso da fase antes de voltar para o menu?", "Sim", () => Debug.Log("Salvando o jogo corrente"), "Nao", () => SceneManager.LoadScene("SelectLevel"));
    }
    void ExitMenuPressButton()
    {        
        popup.InitPopup("Voce deseja sair do jogo e perder todo o progresso da fase?", "Sim", Exit, "Nao", () => Debug.Log("")); ;
    }

    private void Exit()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}
