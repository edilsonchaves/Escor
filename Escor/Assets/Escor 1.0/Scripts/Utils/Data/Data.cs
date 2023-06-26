using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Reflection;
using Sirenix.Serialization;

public class Data<T> : MonoBehaviour where T : MonoBehaviour, new()
{
    T auxValue;
    string path;
    protected void SaveData() {
        Debug.Log(auxValue);
        FieldInfo[] infos = typeof(T).GetFields();
        foreach (var fieldInfo in infos)
        {
            
            Debug.Log($"{fieldInfo.Name} {fieldInfo.GetValue(auxValue)}");
        }

        SaveInMemory();
    } 
    protected void LoadData()
    {
        LoadInMemory();
    }
    protected void InitiateInstance(T value)
    {
        auxValue = value;
    }
    private void SaveInMemory()
    {
        byte[] data = SerializationUtility.SerializeValue(auxValue, DataFormat.JSON);
        path = "" + Application.persistentDataPath;
        path += "/" + typeof(T).ToString() + ".data";
        Debug.Log(path);
        Debug.Log(data.LongLength);
        File.WriteAllBytes(path, data);
    }

    private T LoadInMemory()
    {
        path = "" + Application.persistentDataPath;
        path += "/" + typeof(T).ToString() + ".data";
        if (File.Exists(path))
        {
            Debug.Log(path);
            byte[] bytes = File.ReadAllBytes(path);
            Debug.Log(bytes.LongLength);
            var teste = SerializationUtility.DeserializeValue<DataLevelInfo>(bytes, DataFormat.JSON);
            Debug.Log($"teste: {teste}");
            return null ;
        }

        return null;
    }
}
