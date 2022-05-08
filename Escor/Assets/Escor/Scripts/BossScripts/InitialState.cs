using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InitialState : BossBaseState
{
    [SerializeField] private List<Conversa> conversaBoss;
    public override void EnterState(BossScript boss)
    {
        Debug.Log("Execute Initial State");
        if (boss.bossCoroutineAction != null)
            boss.StopCoroutine(boss.bossCoroutineAction);
        boss.bossCoroutineAction = boss.StartCoroutine(InitialStatusBoss(boss));
    }

    public override void UpdateState(BossScript boss) { }

    public override void OnCollisionEnter(BossScript boss, Collision2D collsion) 
    {
       // if (collsion.tag == "Player")
        //{
            //boss.GetComponent<BoxCollider2D>().enabled = false;
            //boss.StartCoroutine(InitialStatusBoss(boss));
        //}
    }
    public override string GetStateName()
    {
        return "DieStateAtttack";
    }


    protected IEnumerator InitialStatusBoss(BossScript boss)
    {
        boss.Conversa(conversaBoss);
        boss.PlayAnimation("BossParado(Sentado)");
        yield return new WaitUntil(() => boss.GetStatusConversa());


        // _animatorBoss.Play("BossDerrotadoLoop");
        // fade.SetActive(true);
        yield return new WaitForSeconds(1f);

        //SceneManager.LoadScene("Final");
    }
}
