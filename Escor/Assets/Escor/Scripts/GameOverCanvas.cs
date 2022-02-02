using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverCanvas : MonoBehaviour
{
    [SerializeField]GameObject bg;

    private void OnEnable()
    {
        ManagerEvents.PlayerMovementsEvents.onDiePlayer += ActiveDieBG;
    }
    private void OnDisable()
    {
        ManagerEvents.PlayerMovementsEvents.onDiePlayer -= ActiveDieBG;
    }

    public void ActiveDieBG()
    {
        Debug.Log("oie2 ");
        bg.SetActive(true);
    }
}
