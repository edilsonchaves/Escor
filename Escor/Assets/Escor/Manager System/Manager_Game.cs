using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelInfo
{
    public enum LevelStatus {NewLevel, ContinueLevel};

}

public class Manager_Game : MonoBehaviour
{
    private static Manager_Game _instance;
    public static Manager_Game Instance
    {
        get
        {
            if (_instance == null)
                _instance = new GameObject().AddComponent<Manager_Game>();
            return _instance;
        }

    }

    [SerializeField]LevelInfo.LevelStatus _levelStatus;
    public LevelInfo.LevelStatus LevelStatus
    {
        get { return _levelStatus; }
        set { _levelStatus = value; }
    }
    public GameData saveGameData;
    public SectionData sectionGameData;
    public LevelData levelData;


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
            saveGameData = SaveLoadSystem.LoadFile<GameData>(Application.persistentDataPath+"/GameData.data");
            if (saveGameData == null)
            {
                saveGameData=InitializingGameDataSystem();
            }
        }
        else
        {
            Destroy(this.gameObject);
        }

        // [Jessé] se não estiver na cena 'GameLevel', o dialago da tartaruga será reiniciado
        if(SceneManager.GetActiveScene().name != "GameLevel")
            ResetTurtleDialog();
    }


    // [Jessé] deixa o diálogo da tartura ativado
    private void ResetTurtleDialog()
    {
        print("_> Diálogo reiniciado");
        PlayerPrefs.SetInt("SkipConversationOfTurtle", 0); // 0 = não pular diálogo
    }

    public GameData InitializingGameDataSystem()
    {
        GameData initializeGameData = new GameData(50, 50, 100, 0);

        SaveLoadSystem.SaveFile<GameData>(initializeGameData);
        return initializeGameData;
    }
    public void InitialNewSectionGame()
    {
        sectionGameData = new SectionData(1,3,new bool[3]);

        SaveLoadSystem.SaveFile<SectionData>(sectionGameData);
    }

    public void LoadSectionGameMemory()
    {
        Debug.Log(Application.persistentDataPath + "/SectionData.data");
        sectionGameData = SaveLoadSystem.LoadFile<SectionData>(Application.persistentDataPath+"/SectionData.data");
    }

    public void LoadLevelDataMemory()
    {
        Debug.Log(Application.persistentDataPath + "/LevelData.data");
        levelData = SaveLoadSystem.LoadFile<LevelData>(Application.persistentDataPath +"/LevelData.data");

    }

    public void InitialNewLevelGame(int levelSelected)
    {
        _levelStatus = LevelInfo.LevelStatus.NewLevel;
        LevelData data = new LevelData(levelSelected,0,0,3,sectionGameData.GetPowersAwarded());
        levelData = data;
        SaveLoadSystem.SaveFile<LevelData>(levelData);
    }

    public void LoadLevelGame()
    {
        LoadLevelDataMemory();
        if (levelData == null)
            _levelStatus = LevelInfo.LevelStatus.NewLevel;
    }

    public void AdaptLanguageInScene()
    {
        ManagerEvents.GameConfig.ChangedLanguage(saveGameData.LanguageSelect);
    }

    public void SaveLevelMemory(int level,float posPlayerX,float posPlayerY,int lifePlayer,bool[] powerLevel,string fragmentLife, string fragmentMemory)
    {
        SaveLoadSystem.SaveFile<LevelData>(new LevelData(level, posPlayerX, posPlayerY, lifePlayer,powerLevel,fragmentLife,fragmentMemory));
    }
}

[System.Serializable]
public class GameData
{
    [SerializeField] int _volumeLocal;
    [SerializeField] int _volumeAmbientLocal;
    [SerializeField] int _letterSizeLocal;
    [SerializeField] int _languageSelectLocal;
    public int Volume { get { return _volumeLocal; } set { _volumeLocal = value; } }
    public int VolumeAmbient { get { return _volumeAmbientLocal; } set { _volumeAmbientLocal = value; } }
    public int LetterSize { get { return _letterSizeLocal; } set { _letterSizeLocal = value; } }
    public int LanguageSelect { get { return _languageSelectLocal; } set { _languageSelectLocal = value; } }

    public GameData(int valueVolume,int valueVolumeAmbient,int valueLettrSize, int valueLanguageSelect)
    {
        _volumeLocal = valueVolume;
        _volumeAmbientLocal = valueVolumeAmbient;
        _letterSizeLocal = valueLettrSize;
        _languageSelectLocal = valueLanguageSelect;

    }
}
[System.Serializable]
public class SectionData
{
    [SerializeField] int _currentLevelLocal;
    [SerializeField] LevelData[] _levelsDataLocal;
    [SerializeField] bool[] powerAwardedLocal;
    public SectionData(int valueLevel, int valueNumberLevels,bool[] valuePowersAwarded)
    {
        _currentLevelLocal = valueLevel;
        _levelsDataLocal = new LevelData[valueNumberLevels];
        powerAwardedLocal = valuePowersAwarded;
    }



    // [Jessé] ----------------------------------

        public void SetSectionData(int valueLevel, int valueNumberLevels,bool[] valuePowersAwarded)
        {
            SetCurrentLevel(valueLevel);
            SetLevelsData(valueNumberLevels);
            SetPowersAwarded(valuePowersAwarded);
        }

        public void SetCurrentLevel(int level)
        {
            _currentLevelLocal = level;
        }

        public void SetLevelsData(int valueNumberLevels)
        {
            _levelsDataLocal = new LevelData[valueNumberLevels];
        }

    // ------------------------------------------



    public int GetCurrentLevel()
    {
        return _currentLevelLocal;
    }

    public void ConclusionLevelData(int level,int maxPercentualConclusion,bool[]targetsComplete,bool[] powerAwardedLevel)
    {
        _levelsDataLocal[level].SetPercentualConclusion(maxPercentualConclusion);
        _levelsDataLocal[level].SetTargetComplete(targetsComplete);
        powerAwardedLocal = powerAwardedLevel;
        _currentLevelLocal = level + 1;
    }

    public bool[] GetPowersAwarded()
    {
        return powerAwardedLocal;
    }

    public void SetPowersAwarded(bool[] newValuePowersAwarded)
    {
        powerAwardedLocal = newValuePowersAwarded;
    }
    [System.Serializable]
    public class LevelData{
        [SerializeField]int _currentMaxPercentualConclusionLocal;
        [SerializeField]bool[] _currentTargetsCompleteLocal;
        [SerializeField] bool[] powerAwardedLevelLocal;
        public LevelData(int valuePercentualConclusion,bool[] valueTargetsComplete, bool[] valuePowersAwarded)
        {
            _currentMaxPercentualConclusionLocal = valuePercentualConclusion;
            _currentTargetsCompleteLocal = valueTargetsComplete;
            powerAwardedLevelLocal = valuePowersAwarded;
        }
        public int GetPercentualConclusion()
        {
            return _currentMaxPercentualConclusionLocal;
        }
        public void SetPercentualConclusion(int newValue)
        {
            if (newValue > _currentMaxPercentualConclusionLocal)
            {
                _currentMaxPercentualConclusionLocal = newValue;
            }
        }
        public bool[] GetTargetsComplete()
        {
            return _currentTargetsCompleteLocal;
        }

        public void SetTargetComplete(bool[] newTargetsObjective)
        {
            _currentTargetsCompleteLocal = newTargetsObjective;
        }

        public bool[] GetPowersAwarded()
        {
            return powerAwardedLevelLocal;
        }

        public void SetPowersAwarded(bool[] newValuePowersAwarded)
        {
            powerAwardedLevelLocal = newValuePowersAwarded;
        }
    }
}
[System.Serializable]
public class LevelData
{
    [SerializeField] int _levelGamingLocal;
    [SerializeField] float _playerPositionXLocal, _playerPositionYLocal;
    [SerializeField] int _lifePlayerAmountLocal;
    [SerializeField] bool[] _powersLocal;
    [SerializeField] bool[] coinsLevelLocal;
    [SerializeField] string fragmentLifeStatusLocal;
    [SerializeField] string fragmentMemoryShardStatusLocal;

    //[SerializeField] int progressLevelLocal;
    //[SerializeField] bool[] regionsVisitedLocal;
    [SerializeField] Vector2?[] bossLevelPositionLocal;
    public int LevelGaming { get { return _levelGamingLocal; } private set { } }
    public Vector2 CharacterPosition { get { return new Vector2(_playerPositionXLocal, _playerPositionYLocal); } private set { } }
    public bool[] Powers { get { return _powersLocal; }  private set{}}
    public string FragmentLifeStatus { get { return fragmentLifeStatusLocal; } private set { } }
    public string FragmentMemoryStatus { get { return fragmentMemoryShardStatusLocal; } private set { } }
    public LevelData(int valueLevel, float posPlayerX=0,float posPlayerY=0, int valueLifePlayerAmount = 3, bool[] valuePowerPlayer = null, string fragmentLifeStatus="", string fragmentMemoryStatus = "") // falta colocar aqui "coinsLevel"
    {
        _levelGamingLocal = valueLevel;
        _playerPositionXLocal = posPlayerX;
        _playerPositionYLocal = posPlayerY;
        _lifePlayerAmountLocal = valueLifePlayerAmount;
        _powersLocal = valuePowerPlayer;
        fragmentLifeStatusLocal = fragmentLifeStatus;
        fragmentMemoryShardStatusLocal = fragmentMemoryStatus;

    }


    // [Jessé] -----------------------------------------------------------------------

        public void SetLevelData(int valueLevel, float posPlayerX=0,float posPlayerY=0, int valueLifePlayerAmount = 3, bool[] valuePowerPlayer = null, bool[] coinsLevel = null, string fragmentLifeStatus = "", string fragmentMemoryStatus = "")
        {
            SetLevelGaming(valueLevel);
            SetPlayerPosition(posPlayerX, posPlayerY);
            SetLifePlayerAmount(valueLifePlayerAmount);
            SetPowers(valuePowerPlayer);
            SetCoinsLevel(coinsLevel);
            SetFragmentLifeLevel(fragmentLifeStatus);
            SetFragmentMemoryLevel(fragmentMemoryStatus);
        }


        public void SetLevelGaming(int value)
        {
            _levelGamingLocal = value;
        }

        public void SetLifePlayerAmount(int value=3)
        {
            _lifePlayerAmountLocal = value;
        }

        public void ResetPlayerPosition()
        {
            _playerPositionXLocal = _playerPositionYLocal = 0;
        }

        public void SetPlayerPosition(float x, float y)
        {
            _playerPositionXLocal = x;
            _playerPositionYLocal = y;
        }

        public void SetPowers(bool[] powers)
        {
            _powersLocal = powers;
        }

        public void SetCoinsLevel(bool[] _coinsLevel)
        {
            coinsLevelLocal = _coinsLevel;
        }

    public void SetFragmentLifeLevel(string fragmentLife)
    {
        fragmentLifeStatusLocal = fragmentLife;
    }

    public void SetFragmentMemoryLevel(string fragmentMemory)
    {
        fragmentMemoryShardStatusLocal = fragmentMemory;
    }

    // -------------------------------------------------------------------------------

}
