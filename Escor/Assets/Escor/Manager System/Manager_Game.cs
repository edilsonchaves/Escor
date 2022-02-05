using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            saveGameData = SaveLoadSystem.LoadFile<GameData>("C:/Users/Edilson Chaves/AppData/LocalLow/DefaultCompany/Escor/GameData.data");
            if (saveGameData == null)
            {
                saveGameData=InitializingGameDataSystem();
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public GameData InitializingGameDataSystem()
    {
        GameData initializeGameData = new GameData(50, 50, 32, 0);

        SaveLoadSystem.SaveFile<GameData>(initializeGameData);
        return initializeGameData;
    }
    public void InitialNewSectionGame()
    {
        sectionGameData = new SectionData(1,3,new bool[3]);

        SaveLoadSystem.SaveFile<SectionData>(sectionGameData);
    }

    public void LoadSectionGame()
    {
        sectionGameData = SaveLoadSystem.LoadFile<SectionData>("C:/Users/Edilson Chaves/AppData/LocalLow/DefaultCompany/Escor/SectionData.data");
    }

    public void LoadLevelData()
    {
        levelData = SaveLoadSystem.LoadFile<LevelData>("C:/Users/Edilson Chaves/AppData/LocalLow/DefaultCompany/Escor/LevelData.data");

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
        LoadLevelData();
        if (levelData == null)
            _levelStatus = LevelInfo.LevelStatus.NewLevel;    
        else
            _levelStatus = LevelInfo.LevelStatus.ContinueLevel;
    }

    public void AdaptLanguageInScene()
    {
        ManagerEvents.GameConfig.ChangedLanguage(saveGameData.LanguageSelect);
    }

    public void SaveLevelData(int level,float posPlayerX,float posPlayerY,int lifePlayer,bool[] powerLevel)
    {
        SaveLoadSystem.SaveFile<LevelData>(new LevelData(level, posPlayerX, posPlayerY, lifePlayer,powerLevel));
    }
}

[System.Serializable]
public class GameData
{
    [SerializeField] int _volume;
    [SerializeField] int _volumeAmbient;
    [SerializeField] int _letterSize;
    [SerializeField] int _languageSelect;
    public int Volume { get { return _volume; } set { _volume = value; } }
    public int VolumeAmbient { get { return _volumeAmbient; } set { _volumeAmbient = value; } }
    public int LetterSize { get { return _letterSize; } set { _letterSize = value; } }
    public int LanguageSelect { get { return _languageSelect; } set { _languageSelect = value; } }
    public GameData(int valueVolume,int valueVolumeAmbient,int valueLettrSize, int valueLanguageSelect)
    {
        _volume = valueVolume;
        _volumeAmbient = valueVolumeAmbient;
        _letterSize = valueLettrSize;
        _languageSelect = valueLanguageSelect;
    }
}
[System.Serializable]
public class SectionData
{
    [SerializeField] int _currentLevel;
    [SerializeField]LevelData[] _levelsData;
    [SerializeField] bool[] powerAwarded;
    public SectionData(int valueLevel, int valueNumberLevels,bool[] valuePowersAwarded)
    {
        _currentLevel = valueLevel;
        _levelsData = new LevelData[valueNumberLevels];
        powerAwarded = valuePowersAwarded;
    }

    public int GetCurrentLevel()
    {
        return _currentLevel;
    }

    public void ConclusionLevelData(int level,int maxPercentualConclusion,bool[]targetsComplete,bool[] powerAwardedLevel)
    {
        _levelsData[level].SetPercentualConclusion(maxPercentualConclusion);
        _levelsData[level].SetTargetComplete(targetsComplete);
        powerAwarded = powerAwardedLevel;
        _currentLevel = level + 1;
    }

    public bool[] GetPowersAwarded()
    {
        return powerAwarded;
    }

    public void SetPowersAwarded(bool[] newValuePowersAwarded)
    {
        powerAwarded = newValuePowersAwarded;
    }
    [System.Serializable]
    public class LevelData{
        [SerializeField]int _currentMaxPercentualConclusion;
        [SerializeField]bool[] _currentTargetsComplete;
        [SerializeField] bool[] powerAwardedLevel;
        public LevelData(int valuePercentualConclusion,bool[] valueTargetsComplete, bool[] valuePowersAwarded)
        {
            _currentMaxPercentualConclusion = valuePercentualConclusion;
            _currentTargetsComplete = valueTargetsComplete;
            powerAwardedLevel = valuePowersAwarded;
        }
        public int GetPercentualConclusion()
        {
            return _currentMaxPercentualConclusion;
        }
        public void SetPercentualConclusion(int newValue)
        {
            if (newValue > _currentMaxPercentualConclusion)
            {
                _currentMaxPercentualConclusion = newValue;
            }
        }
        public bool[] GetTargetsComplete()
        {
            return _currentTargetsComplete;
        }

        public void SetTargetComplete(bool[] newTargetsObjective)
        {
            _currentTargetsComplete = newTargetsObjective;
        }

        public bool[] GetPowersAwarded()
        {
            return powerAwardedLevel;
        }

        public void SetPowersAwarded(bool[] newValuePowersAwarded)
        {
            powerAwardedLevel = newValuePowersAwarded;
        }
    }
}
[System.Serializable]
public class LevelData
{
    [SerializeField] int _levelGaming;
    [SerializeField] float _playerPositionX, _playerPositionY;
    [SerializeField] int _lifePlayerAmount;
    [SerializeField] bool[] _powers;
    [SerializeField] bool[] coinsLevel;
    //[SerializeField] int progressLevel;
    //[SerializeField] bool[] regionsVisited;
    [SerializeField] Vector2?[] bossLevelPosition;
    public int LevelGaming { get { return _levelGaming; } private set { } }
    public Vector2 CharacterPosition { get { return new Vector2(_playerPositionX, _playerPositionY); } private set { } }
    public bool[] Powers { get { return _powers; }  private set{}}
    public LevelData(int valueLevel, float posPlayerX=0,float posPlayerY=0, int valueLifePlayerAmount = 3, bool[] valuePowerPlayer = null)
    {
        _levelGaming = valueLevel;
        _playerPositionX = posPlayerX;
        _playerPositionY = posPlayerY;
        _lifePlayerAmount = valueLifePlayerAmount;
        _powers = valuePowerPlayer;
    }
}
