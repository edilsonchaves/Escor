using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Cinemachine;
public class LevelManager : MonoBehaviour
{
    public enum LevelStatus {Game, Pause, CutScene,EndGame};
    public static LevelStatus levelstatus;
    private LevelStatus auxLevelStatus;
    [SerializeField]UIController control;
    [SerializeField] GameObject[] levelsAvaibles;
    GameObject currentLevel;
    [SerializeField] GameObject characterPrefab;
    GameObject currentCharacter;
    [SerializeField] Camera cam;
    [SerializeField] AudioSource levelAudioSource;
    [SerializeField] AudioClip[] levelsMusicsBG;
    [SerializeField] CinemachineVirtualCamera virtualCam;
    [SerializeField]Popup popup;
    void Start()
    {
        popup = control.CreatePopup();
        popup.gameObject.SetActive(false);
        if (Manager_Game.Instance.levelData == null)
        {
            CreateLevel(3);
        }
        else
        {
            CreateLevel(Manager_Game.Instance.levelData.LevelGaming);
            if(Manager_Game.Instance.LevelStatus==LevelInfo.LevelStatus.ContinueLevel)
                PlayerSetupInformation();
        }
        SfxManager.Initialize();
        levelstatus = LevelStatus.Game;
    }

    void CreateLevel(int level)
    {
        currentLevel=Instantiate(levelsAvaibles[level-1],Vector3.zero,Quaternion.identity);
        currentLevel.GetComponent<LevelInformation>().initializeLevelInformation(out Transform initialSpawnPosition);
        currentCharacter = Instantiate(characterPrefab, initialSpawnPosition.position, initialSpawnPosition.rotation);
        cam.transform.localPosition = new Vector3(0, 0, -10);
        virtualCam.Follow = currentCharacter.transform;
        levelAudioSource.clip = levelsMusicsBG[level - 1];
        levelAudioSource.Play();
    }
    void PlayerSetupInformation()
    {
        Vector2 pos = Manager_Game.Instance.levelData.CharacterPosition;
        currentCharacter.transform.position = new Vector3(pos.x, pos.y,0);
        int id = 0;
        foreach(bool powerID in Manager_Game.Instance.levelData.Powers)
        {
            if (powerID)
            {
                ManagerEvents.PlayerMovementsEvents.PlayerGetedPower(id);
            }
            id++;
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (levelstatus == LevelStatus.Pause)
            {
                ManagerEvents.UIConfig.ResumedGame();
                Resume();
            }
            else
            {
                ManagerEvents.UIConfig.PausedGame(10, 20, 30);
                Pause();
            }
        }
    }
    private void OnEnable()
    {
        ManagerEvents.UIConfig.onReturnMenu += VoltarMenuPressButton;
        ManagerEvents.UIConfig.onExitMenu += ExitMenuPressButton;
        ManagerEvents.UIConfig.onResumeGame += Resume;
        ManagerEvents.UIConfig.onSaveGame += SaveGameButton;
        ManagerEvents.PlayerDeadUI.onReplayLevel += Recome�arFasePressButton;
    }
    private void OnDisable()
    {
        ManagerEvents.UIConfig.onReturnMenu -= VoltarMenuPressButton;
        ManagerEvents.UIConfig.onExitMenu -= ExitMenuPressButton;
        ManagerEvents.UIConfig.onResumeGame -= Resume;
        ManagerEvents.UIConfig.onSaveGame -= SaveGameButton;
        ManagerEvents.PlayerDeadUI.onReplayLevel -= Recome�arFasePressButton;


    }
    void Resume()
    {
        Time.timeScale = 1f;
        levelstatus = auxLevelStatus;

    }

    void Pause()
    {
        auxLevelStatus = levelstatus;
        levelstatus = LevelStatus.Pause;
        Time.timeScale = 0f;
    }

    void VoltarMenuPressButton()
    {
        Debug.Log("Teste");
        popup.InitPopup("Voc� deseja salvar o progresso da fase antes de voltar para o menu?", "Sim", SaveGame, "Nao", () => SceneManager.LoadScene("SelectLevel"));
    }

    public void SaveGame()
    {
        Manager_Game.Instance.SaveLevelData(Manager_Game.Instance.levelData.LevelGaming,currentCharacter.transform.position.x, currentCharacter.transform.position.y, currentCharacter.GetComponent<Movement>().Life, currentCharacter.GetComponent<Movement>().PowerHero);
        SceneManager.LoadScene("SelectLevel");
    }
    void SaveGameButton()
    {
        SaveGame();
    }
    void ExitMenuPressButton()
    {        
        popup.InitPopup("Voce deseja sair do jogo e perder todo o progresso da fase?", "Sim", Exit, "Nao", () => Debug.Log("")); ;
    }


    public void Recome�arFasePressButton()
    {
        popup.InitPopup("Cuidado! Esta a��o ir� fazer com que voc� perca o progresso salvo. Deseja continuar?", "Sim", () => Debug.Log("Sim Action"), "Nao", () => Debug.Log("Nao"));
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
