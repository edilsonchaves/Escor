using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialState : BossBaseState
{
    public override void EnterState(BossScript boss)
    {
        Debug.Log("Execute Initial State");
    }

    public override void OnCollisionEnter(BossScript boss, Collision2D collsion)
    {
        Debug.Log("Teste");
        if (collsion.gameObject.tag == "Player")
        {
            Debug.Log("Teste2");
            boss.GetComponent<BoxCollider2D>().enabled = false;
            boss.SwitchState(boss.talkState);
        }
    }



    public override void UpdateState(BossScript boss)
    {

    }

    public override string GetStateName()
    {
        return "InitialState";
    }
}
