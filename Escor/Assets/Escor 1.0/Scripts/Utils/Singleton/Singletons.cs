using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singletons<T> : MonoBehaviour where T : MonoBehaviour, new()
{
    // Start is called before the first frame update
    private static T _instance;
    public static T Instance
    {
        get
        {            
            return _instance;            
        }
        private set { }
    }
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = FindObjectOfType<T>();
        }
        else if (_instance != FindObjectOfType<T>())
        {
            Destroy(FindObjectOfType<T>());
        }
        DontDestroyOnLoad(_instance.gameObject);
    }

}
