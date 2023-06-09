using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using System.IO;
using System;

public class Player : MonoBehaviour
{
    [SerializeField] private DataPlayer data;

    private void Start()
    {
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            data.SaveMemory();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            data.Load();
        }
    }
}

[System.Serializable]
public class DataPlayer{
    public string name;
    public int vida;
    public Vector3 position;
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
