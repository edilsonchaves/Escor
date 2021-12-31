using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public GameData saveGameData;
    public SectionData sectionGameData;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
            SaveLoadSystem.SaveFile<SectionData>(new SectionData(1,3));
            saveGameData = SaveLoadSystem.LoadFile<GameData>("C:/Users/Edilson Chaves/AppData/LocalLow/DefaultCompany/Escor/GameData.data");
            sectionGameData = SaveLoadSystem.LoadFile<SectionData>("C:/Users/Edilson Chaves/AppData/LocalLow/DefaultCompany/Escor/SectionData.data");
            Debug.Log(saveGameData._volume+" "+ saveGameData._volumeAmbient + " "+ saveGameData._letterSize+" "+ saveGameData._resolution);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}

[System.Serializable]
public class GameData
{
    public int _volume;
    public int _volumeAmbient;
    public int _letterSize;
    public int _resolution;
    public GameData(int valueVolume,int valueVolumeAmbient,int valueLettrSize, int valueResolution)
    {
        _volume = valueVolume;
        _volumeAmbient = valueVolumeAmbient;
        _letterSize = valueLettrSize;
        _resolution = valueResolution;
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
public class PlayerData
{
    int _value;
    public PlayerData(int value)
    {
        _value = value;
    }
}
