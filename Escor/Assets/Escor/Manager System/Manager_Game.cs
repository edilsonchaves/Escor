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

    public LevelInfo.LevelStatus levelStatus;
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
        SaveLoadSystem.SaveFile<SectionData>(new SectionData(1, 3));
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
        levelStatus = LevelInfo.LevelStatus.NewLevel;
        levelData = new LevelData(levelSelected);
        SaveLoadSystem.SaveFile<LevelData>(levelData);


    }

    public void LoadLevelGame()
    {
        LoadLevelData();
        if (levelData == null)
            levelStatus = LevelInfo.LevelStatus.NewLevel;    
        else
            levelStatus = LevelInfo.LevelStatus.ContinueLevel;
    }

    public void AdaptLanguageInScene()
    {
        ManagerEvents.GameConfig.ChangedLanguage(saveGameData.LanguageSelect);
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

    public SectionData(int valueLevel, int valueNumberLevels)
    {
        _currentLevel = valueLevel;
        _levelsData = new LevelData[valueNumberLevels];
    }

    public int GetCurrentLevel()
    {
        return _currentLevel;
    }

    public void ConclusionLevelData(int level,int maxPercentualConclusion,bool[]targetsComplete)
    {
        _levelsData[level].SetPercentualConclusion(maxPercentualConclusion);
        _levelsData[level].SetTargetComplete(targetsComplete);
        Debug.Log(_levelsData[level].GetPercentualConclusion()+"/"+ _levelsData[level].GetTargetsComplete().GetValue(0,1,2));
    }
    [System.Serializable]
    public class LevelData{
        [SerializeField]int _currentMaxPercentualConclusion;
        [SerializeField]bool[] _currentTargetsComplete;
        public LevelData(int valuePercentualConclusion,bool[] valueTargetsComplete)
        {
            _currentMaxPercentualConclusion = valuePercentualConclusion;
            _currentTargetsComplete = valueTargetsComplete;
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
            for(int i = 0; i < newTargetsObjective.Length; i++)
            {
                if (newTargetsObjective[i])
                    _currentTargetsComplete[i] = true;
            }
        }
    }
}
[System.Serializable]
public class LevelData
{
    [SerializeField] int _levelGaming;
    public int LevelGaming { get { return _levelGaming; } private set { } }

    public LevelData(int valueLevel)
    {
        _levelGaming = valueLevel;
    }
}
