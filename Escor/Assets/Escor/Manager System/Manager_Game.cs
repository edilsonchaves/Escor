using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Game : MonoBehaviour
{
    public static Manager_Game manager;
    public static int volume;
    public static int volumeAmbient;
    public static int letterSize;
    public static int resolution;
    private void Awake()
    {
        if (manager == null)
        {
            manager = this;
            DontDestroyOnLoad(this.gameObject);
            //SaveLoadSystem.SaveFile<GameData>(new GameData(0, 0, 0, 0));
            //SaveLoadSystem.SaveFile<PlayerData>(new PlayerData(100));

            var saveData = SaveLoadSystem.LoadFile<GameData>("C:/Users/Edilson Chaves/AppData/LocalLow/DefaultCompany/Escor/GameData.data");
            Debug.Log(saveData._volume+" "+ saveData._volumeAmbient + " "+ saveData._letterSize+" "+ saveData._resolution);
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
public class PlayerData
{
    int _value;
    public PlayerData(int value)
    {
        _value = value;
    }
}
