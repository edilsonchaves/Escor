using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public class SaveLoadSystem
{

    public static void SaveFile<T>(T data)
    {
        BinaryFormatter bf = new BinaryFormatter();
        string path = "" + Application.persistentDataPath;
        path += "/"+typeof(T).ToString()+".data";
        Debug.Log(path);
        FileStream stream = new FileStream(path,FileMode.Create);
        bf.Serialize(stream,data);
        stream.Close();        
    }

    public static T LoadFile<T>(string path)
    {
        if (!File.Exists(path))
        {
            return default(T);
        }
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = new FileStream(path,FileMode.Open);
        try
        {
            T saveData = (T)bf.Deserialize(file);
            file.Close();
            return saveData;
        }
        catch
        {
            Debug.LogError("Erro in Load File");
            file.Close();
            return default(T);
        }
        
    }
    public static void ResetGameData()
    {

        GameData data = new GameData(50, 50, 18, 0);
        BinaryFormatter bf = new BinaryFormatter();
        string path = "C:/Users/Edilson Chaves/AppData/LocalLow/DefaultCompany/Escor/GameData.data";
        FileStream stream = new FileStream(path, FileMode.Create);
        bf.Serialize(stream, data);
        stream.Close();
    
    }
}
