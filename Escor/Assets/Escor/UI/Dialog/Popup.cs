using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Popup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textTitle;
    [SerializeField] TextMeshProUGUI _textButton1;
    [SerializeField] Button button1;
    [SerializeField] TextMeshProUGUI _textButton2;
    [SerializeField] Button button2;
    public void InitPopup(string textTitle, string textButton1, Action actionButton1,string textButton2="",Action actionButton2=null)
    {
        button1.onClick.RemoveAllListeners();
        button2.onClick.RemoveAllListeners();
        _textTitle.text = textTitle;
        button1.onClick.AddListener(()=> { actionButton1(); this.gameObject.SetActive(false); });
        _textButton1.text = textButton1;

        if (actionButton2 == null)
        {
            button2.gameObject.SetActive(false);
            return;
        }
        button2.gameObject.SetActive(true);
        button2.onClick.AddListener(() => { actionButton2(); this.gameObject.SetActive(false); });
        _textButton2.text = textButton2;
        this.gameObject.SetActive(true);
    }
}
