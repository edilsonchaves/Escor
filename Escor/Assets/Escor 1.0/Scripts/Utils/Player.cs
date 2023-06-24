using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using System.IO;
using System;

public class Player : Singletons<Player>
{
    [SerializeField] private DataPlayer saveData;
    [SerializeField] private string _playerName;
    [SerializeField]private int _playerLife;

    public static bool IsPlayerLoaded;

    private void Start()
    {
    }
    void Update()
    {
    }

    private void OnEnable()
    {
        Game_Manager._saveAction += SavePlayerData;

    }

    private void OnDisable()
    {
        Game_Manager._saveAction -= SavePlayerData;

    }
    private void SavePlayerData()
    {
        Debug.Log("Player Save");
        saveData.ConfigureDataToSave(_playerName, _playerLife, transform.position);
        saveData.SaveMemory();
    }
    public static IEnumerator SetupPlayer()
    {
        IsPlayerLoaded = false;
        Instance.saveData.Load();
        Debug.Log($"Dados Carregados");
        yield return new WaitForSecondsRealtime(2f);
        Instance._playerName = Instance.saveData.name;
        Instance._playerLife = Instance.saveData.vida;
        Instance.transform.position = Instance.saveData.position;
        Debug.Log($"Player Atualizado");
        yield return new WaitForSecondsRealtime(2f);
        IsPlayerLoaded = true;
    }
}

[System.Serializable]
public class DataPlayer{
    public string name;
    public int vida;
    public Vector3 position;
    public void ConfigureDataToSave(string newName,int newVida, Vector3 newPosition)
    {
        name = newName;
        vida = newVida;
        position = newPosition;
    }
    public void ConfigureDataToSave(int newVida, Vector3 newPosition)
    {
        vida = newVida;
        position = newPosition;
    }
    public void SaveMemory()
    {
        string path;
        byte[] data = SerializationUtility.SerializeValue(this, DataFormat.JSON);
        path = "" + Application.persistentDataPath;
        path += "/" + this.GetType().ToString() + ".data";
        Debug.Log(path);
        Debug.Log(data.LongLength);
        File.WriteAllBytes(path, data);
    }
    public void Load()
    {
        var value = LoadMemory();
        name = value.name;
        vida = value.vida;
        position = value.position;
    }
    public DataPlayer LoadMemory()
    {
        string path;
        path = "" + Application.persistentDataPath;
        path += "/" + this.GetType().ToString() + ".data";
        if (File.Exists(path))
        {
            Debug.Log(path);
            byte[] bytes = File.ReadAllBytes(path);
            Debug.Log(bytes.LongLength);
            var teste = SerializationUtility.DeserializeValue<DataPlayer>(bytes, DataFormat.JSON);
            return teste;
        }
        return null;
    }
}
