using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusUI : MonoBehaviour
{
    [SerializeField] GameObject bgUI;
    [SerializeField] private Sprite[] _vidasSprite;
    [SerializeField] private GameObject[] _poderSprite;
    [SerializeField] private Image fragmentLifeUI;
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
        _poderSprite[poderCorrentes+1].SetActive(true);
    }
    public void BGDesactiveDead()
    {
        bgUI.SetActive(false);
    }

    private void OnEnable()
    {
        ManagerEvents.PlayerMovementsEvents.onLifePlayer += UpdateVidas;
        ManagerEvents.PlayerMovementsEvents.onDiePlayer += BGDesactiveDead;
        ManagerEvents.PlayerMovementsEvents.onPlayerGetPower += UpdatePoder;
        ManagerEvents.PlayerMovementsEvents.onPlayerDefenseTime += UpdateFillPowerDefense;
        ManagerEvents.PlayerMovementsEvents.onPlayerGetFragmentLife += ObtainFragmented;
    }
    private void OnDisable()
    {
        ManagerEvents.PlayerMovementsEvents.onLifePlayer -= UpdateVidas;
        ManagerEvents.PlayerMovementsEvents.onDiePlayer -= BGDesactiveDead;
        ManagerEvents.PlayerMovementsEvents.onPlayerGetPower -= UpdatePoder;
        ManagerEvents.PlayerMovementsEvents.onPlayerDefenseTime -= UpdateFillPowerDefense;
        ManagerEvents.PlayerMovementsEvents.onPlayerGetFragmentLife -= ObtainFragmented;

    }


    private void UpdateFillPowerDefense(float defenseAmount,float defenseMax)
    {
        float valuePercentual = defenseAmount / defenseMax;
        _poderSprite[2].GetComponent<Image>().fillAmount = valuePercentual;
    }

    private void ObtainFragmented(float currentFragment,float totalFragment)
    {
        Debug.Log("Teste");
        StartCoroutine(ObtainFragmentLogic(currentFragment,totalFragment));
    }

    IEnumerator ObtainFragmentLogic(float currentFragment, float totalFragment)
    {
        float initialFillAmount = fragmentLifeUI.fillAmount;
        Debug.Log(currentFragment / totalFragment);
        float fillAmountDestination = (currentFragment / totalFragment);
        Debug.Log(fillAmountDestination);
        float timer=0;
        while (timer<=1)
        {
            fragmentLifeUI.fillAmount=Mathf.Lerp(initialFillAmount, fillAmountDestination, timer);
            yield return new WaitForSeconds(0.01f);
            timer += 0.01f;
            Debug.Log(timer);
        }
    }

}
