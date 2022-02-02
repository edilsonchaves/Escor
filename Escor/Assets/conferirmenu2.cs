using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class conferirmenu2 : MonoBehaviour
{
    [SerializeField] GameObject bgUI;
    [SerializeField] private Sprite[] _vidasSprite;
    [SerializeField] private Sprite[] _poderSprite;
    [SerializeField] private Image _vidasImg;
    [SerializeField] private Image _poderImg;

    //É necessário chamar essa função no script do player 
    // no local onde foi definida a logica da vida
    // quando o player sofrer um dano, perde a vida
    // quando ele perder a vida, a vida sera atualizada:
    // _conferirMenu2.UpdateVidas("nome da forma como foi chamada a vida no script");
    public void UpdateVidas(int vidasCorrentes)
    {
        _vidasImg.sprite = _vidasSprite[vidasCorrentes];
    }

    // mesma forma como no updatevidas
    // _conferirMenu2.UpdatePoder("nome da forma como foi chamada o poder no script");

    public void UpdatePoder(int poderCorrentes)
    {
        _poderImg.sprite = _poderSprite[poderCorrentes];
    }
    public void BGDesactiveDead()
    {
        bgUI.SetActive(false);
    }

    private void OnEnable()
    {
        ManagerEvents.PlayerMovementsEvents.onLifePlayer += UpdateVidas;
        ManagerEvents.PlayerMovementsEvents.onDiePlayer += BGDesactiveDead;
    }
    private void OnDisable()
    {
        ManagerEvents.PlayerMovementsEvents.onLifePlayer -= UpdateVidas;
        ManagerEvents.PlayerMovementsEvents.onDiePlayer -= BGDesactiveDead;
    }

}
