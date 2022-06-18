using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    private static UIController _instance;
    [SerializeField] Transform MainCanvas;

    public static UIController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject().AddComponent<UIController>();
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }

    }
    public Popup CreatePopup()
    {
        LocalizeMainCanvas();
        GameObject popObj = Instantiate(Resources.Load("UI/Dialog")) as GameObject;
        popObj.transform.SetParent(MainCanvas);
        popObj.transform.localScale = Vector3.one;
        popObj.transform.localPosition = Vector3.zero;
        return popObj.GetComponent<Popup>();
    }


    private void LocalizeMainCanvas()
    {
        MainCanvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Transform>();
    }
}
