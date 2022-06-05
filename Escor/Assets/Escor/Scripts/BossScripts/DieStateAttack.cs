using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[Serializable]
public class DieStateAttack : BossBaseState
{
    [SerializeField] private List<Conversa> conversaBossMorte;
    [SerializeField] GameObject fade;

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
        //boss.PlayAnimation("BossDerrotadoLoop");
        fade.SetActive(true);
        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene("FinalVerdadeiro");
    }
    public override void OnCollisionExit(BossScript boss, Collision2D collision)
    {
    }
}
