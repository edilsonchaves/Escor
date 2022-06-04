using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TalkState : BossBaseState
{
    [SerializeField] private List<Conversa> conversaBoss;
    public override void EnterState(BossScript boss)
    {
        Debug.Log("Execute Talk State");
        if (boss.bossCoroutineAction != null)
            boss.StopCoroutine(boss.bossCoroutineAction);
        boss.bossCoroutineAction = boss.StartCoroutine(InitialStatusBoss(boss));
    }

    public override void UpdateState(BossScript boss) { }

    public override void OnCollisionEnter(BossScript boss, Collision2D collsion) 
    {
        if (collsion.gameObject.tag == "Player")
        {
            boss.GetComponent<BoxCollider2D>().enabled = false;
            boss.StartCoroutine(InitialStatusBoss(boss));
        }
    }
    public override string GetStateName()
    {
        return "TalkState";
    }


    protected IEnumerator InitialStatusBoss(BossScript boss)
    {
        boss.Conversa(conversaBoss);
        boss.PlayAnimation("BossParado(Sentado)");
        yield return new WaitUntil(() => boss.GetStatusConversa());
        boss.PlayAnimation("BossLevantando");
        ManagerEvents.Boss.InitializedBattle();
        boss.SetTarget( GameObject.FindGameObjectWithTag("Player").transform);        
        // _animatorBoss.Play("BossDerrotadoLoop");
        // fade.SetActive(true);
        yield return new WaitForSeconds(1f);
        boss.SwitchState(boss.patrollState);
        //SceneManager.LoadScene("Final");
    }

    public override void OnCollisionExit(BossScript boss, Collision2D collision)
    {
    }
}
