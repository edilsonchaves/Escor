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
    [SerializeField]LevelInformation levelInformation;
    [SerializeField] GameObject characterPrefab;
    GameObject currentCharacter;
    [SerializeField] Camera cam;
    [SerializeField] AudioSource levelAudioSource;
    [SerializeField] AudioClip[] levelsMusicsBG;
    [SerializeField] CinemachineVirtualCamera virtualCam;
    [SerializeField]Popup popup;

    float currentMaxVolume;

    void Start()
    {
        currentMaxVolume = levelAudioSource.volume;
        Debug.Log("Teste: "+Manager_Game.Instance.LevelStatus);
        popup = control.CreatePopup();
        popup.gameObject.SetActive(false);
        SfxManager.Initialize();
        if (Manager_Game.Instance.levelData == null)
        {
            Debug.Log("Teste 2");
            CreateLevel(3);
        }
        else
        {
            levelstatus = LevelStatus.Game;
            int level = Manager_Game.Instance.levelData.LevelGaming;
            CreateLevelOficial(level);
            Debug.Log("Teste 3");
            CreatePlayer(level);
            Debug.Log("Teste 4");
            InitializeSoundBG(level);
            Debug.Log("Teste 5");
            MapSetupInformation();
            Debug.Log("Teste 6");
            PlayerSetupGeneric();
            if (Manager_Game.Instance.LevelStatus == LevelInfo.LevelStatus.NewLevel)
            {
                
                Debug.Log("New Level");
                return;
                
            }
            Debug.Log("Teste 7");
            Debug.Log("Continue Level");
            BossSetupInformation();
            PlayerSetupInformation();
        }
        
    }
    void PlayerSetupGeneric()
    {
        int id = 0;
        foreach (bool powerID in Manager_Game.Instance.levelData.Powers)
        {
            Debug.Log("Teste Power ID: "+powerID);
            if (powerID)
            {
                ManagerEvents.PlayerMovementsEvents.PlayerGetedPower(id);
            }
            id++;
        }
    }
    void CreateLevelOficial(int level)
    {
        levelstatus = LevelStatus.Game;
        currentLevel = Instantiate(levelsAvaibles[Manager_Game.Instance.levelData.LevelGaming - 1], Vector3.zero, Quaternion.identity);
        levelInformation = currentLevel.GetComponent<LevelInformation>();
    }
    void CreatePlayer(int level)
    {
        levelInformation.initializeLevelInformation(out Transform initialSpawnPosition);
        currentCharacter = Instantiate(characterPrefab, initialSpawnPosition.position, initialSpawnPosition.rotation);
        cam.transform.localPosition = new Vector3(0, 0, -10);
        virtualCam.Follow = currentCharacter.transform;
    }
    void InitializeSoundBG(int level)
    {
        levelAudioSource.clip = levelsMusicsBG[level - 1];
        levelAudioSource.Play();
        FadeInMusic(5);
    }
    void CreateLevel(int level)
    {
        levelstatus = LevelStatus.Game;
        currentLevel=Instantiate(levelsAvaibles[level-1],Vector3.zero,Quaternion.identity);
        levelInformation = currentLevel.GetComponent<LevelInformation>();
        levelInformation.initializeLevelInformation(out Transform initialSpawnPosition);
        currentCharacter = Instantiate(characterPrefab, initialSpawnPosition.position, initialSpawnPosition.rotation);
        cam.transform.localPosition = new Vector3(0, 0, -10);
        virtualCam.Follow = currentCharacter.transform;
        levelAudioSource.clip = levelsMusicsBG[level - 1];
        // print(levelAudioSource == null);
        levelAudioSource.Play();
        FadeInMusic(5);
    }

    public void FadeInMusic(float time=2)
    {
        FadeAudio.Fade.In(levelAudioSource, currentMaxVolume, time);
    }

    public void FadeOutMusic(float time=2)
    {
        // currentMaxVolume = levelAudioSource.volume;
        FadeAudio.Fade.Out(levelAudioSource, time);
    }

    void BossSetupInformation()
    {
        BossScript boss = GameObject.FindObjectOfType<BossScript>();
        if (boss != null)
        {
            boss.SetActiveBoss(Manager_Game.Instance.bossData.GetActiveBoss());
            boss.transform.position = Manager_Game.Instance.bossData.GetBossPosition();
            boss.InitializeBossLife(Manager_Game.Instance.bossData.LifeBoss());
            ManagerEvents.Boss.LoadedLife(Manager_Game.Instance.bossData.GetActiveBoss(), Manager_Game.Instance.bossData.LifeBoss());
            boss.SetStatusBoss(Manager_Game.Instance.bossData.GetBossStatus());
        }
    }
    void PlayerSetupInformation()
    {
        Vector2 pos = Manager_Game.Instance.levelData.CharacterPosition;
        Debug.Log(pos.x+", "+ pos.y);
        currentCharacter.transform.position = new Vector3(pos.x, pos.y,0);
        currentCharacter.GetComponent<Movement>().LoadLifeInitial(Manager_Game.Instance.levelData.GetLifePlayer());
    }

    void MapSetupInformation()
    {
        levelInformation.LoadLifeShardInformation(Manager_Game.Instance.levelData.FragmentLifeStatus);
        Debug.Log("Situation memory shard in level data: "+ Manager_Game.Instance.levelData.FragmentMemoryStatus);
        levelInformation.LoadMemoryShardInformation(Manager_Game.Instance.levelData.FragmentMemoryStatus);
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

        ManagerEvents.PlayerDeadUI.onReplayLevel += RecomecarFasePressButton;

    }
    private void OnDisable()
    {
        ManagerEvents.UIConfig.onReturnMenu -= VoltarMenuPressButton;
        ManagerEvents.UIConfig.onExitMenu -= ExitMenuPressButton;
        ManagerEvents.UIConfig.onResumeGame -= Resume;
        ManagerEvents.UIConfig.onSaveGame -= SaveGameButton;

        ManagerEvents.PlayerDeadUI.onReplayLevel -= RecomecarFasePressButton;



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
        popup.InitPopup("Voce deseja salvar o progresso da fase antes de voltar para o menu?", "Sim", ReturnMenuSave, "Nao", NoReturnToMenuSave);

    }

    public void NoReturnToMenuSave()
    {
        SaveLoadSystem.DeleteFile<LevelData>();
        SaveLoadSystem.DeleteFile<BossData>();
        SceneManager.LoadScene("SelectLevel");
    }

    public void ReturnMenuSave()
    {
        SaveGame();
        SceneManager.LoadScene("SelectLevel");
    }
    public void SaveGame()
    {
        Movement playerInfo= currentCharacter.GetComponent<Movement>();
        LevelInformation levelInformation = currentLevel.GetComponent<LevelInformation>();
        Manager_Game.Instance.SaveLevelMemory(Manager_Game.Instance.levelData.LevelGaming,currentCharacter.transform.position.x, currentCharacter.transform.position.y, playerInfo.Life, playerInfo.PowerHero, levelInformation.LevelLifeInfo(),levelInformation.LevelMemoryInfo());
        BossScript bossInfo = GameObject.FindObjectOfType<BossScript>();
        if (bossInfo != null)
        {
            Debug.Log("Tenho boss no level");
            GameObject boss = bossInfo.gameObject;
            float[] pos = new float[3];
            pos[0] = boss.transform.position.x;
            pos[1] = boss.transform.position.y;
            pos[2] = boss.transform.position.z;
            Debug.Log(boss + ", " + bossInfo.GetStatusBoss());
            Manager_Game.Instance.SaveLevelBossMemory(bossInfo.GetStatusBoss(),bossInfo.GetLifeBoss(),pos,bossInfo.currentStateName);
        }
        else
        {
            Debug.Log("Nao tenho boss no level");
        }
        //SceneManager.LoadScene("SelectLevel");
    }
    void SaveGameButton()
    {
        SaveGame();
    }
    void ExitMenuPressButton()
    {
        popup.InitPopup("Voce deseja sair do jogo e perder todo o progresso da fase?", "Sim", Exit, "Nao", () => Debug.Log("")); ;
    }


    public void RecomecarFasePressButton()
    {
        popup.InitPopup("Cuidado! Esta acao ira fazer com que voce perca o progresso salvo. Deseja continuar?", "Sim", () => Debug.Log("Sim Action"), "Nao", () => Debug.Log("Nao"));

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
