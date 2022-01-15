using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class toggleScript : MonoBehaviour
{
    [SerializeField] int _value;
    public int Value { get { return _value; }}
    [SerializeField] Text text;


    public void SetText(string newText)
    {
        text.text = newText;
    }
}
