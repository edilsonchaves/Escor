using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DieStateAttack : BossBaseState
{
    [SerializeField] private List<Conversa> conversaBossMorte;
    public override void EnterState(BossScript boss)
    {
        Debug.Log("Execute Die State");
        if(boss.bossCoroutineAction!=null)
            boss.StopCoroutine(boss.bossCoroutineAction);
        boss.bossCoroutineAction = boss.StartCoroutine(DieStatusBossBattle(boss));
    }

    public override void UpdateState(BossScript boss) { }

    public override void OnCollisionEnter(BossScript boss, Collision2D collsion) { }
        public override string GetStateName()
    {
        return "DieStateAtttack";
    }


    protected IEnumerator DieStatusBossBattle(BossScript boss)
    {
        boss.Conversa(conversaBossMorte);
        boss.PlayAnimation("BossDerrotadoStart");
        yield return new WaitUntil(() => boss.GetStatusConversa());


        // _animatorBoss.Play("BossDerrotadoLoop");
        // fade.SetActive(true);
        yield return new WaitForSeconds(1f);

        //SceneManager.LoadScene("Final");
    }
}
