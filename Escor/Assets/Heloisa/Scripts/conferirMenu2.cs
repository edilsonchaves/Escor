using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class conferirMenu2 : MonoBehaviour
{
    [SerializeField] private Sprite[] _vidasSprite;
    [SerializeField] private Sprite[] _poderSprite;
    [SerializeField] private Image _vidasImg;
    [SerializeField] private Image _poderImg;

    //Essa função precisa ser chamada no script do Player da seguinte forma:
    // assim que a nossa vida diminuir, vai ser att a vida
    // _conferirMenu2.UpdateVidas("a forma como foi colocado a vida no script. score, lives.. vida")
    public void UpdateVidas(int vidasCorrentes)
    {
        _vidasImg.sprite = _vidasSprite[vidasCorrentes];
    }

    //Fazer da mesma forma no script do player como foi feito para vida
    public void UpdatePoder(int poderCorrente)
    {
        _poderImg.sprite = _poderSprite[poderCorrente];
    }
}

