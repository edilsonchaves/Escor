using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class MelleAttackState : BossBaseState
{

    public override void EnterState(BossScript boss)
    {
        Debug.Log("Execute Melle Attack");
        boss.bossCoroutineAction = boss.StartCoroutine(MelleAttack(boss));
    }
    public override void UpdateState(BossScript boss) 
    {
    }

    public override void OnCollisionEnter(BossScript boss, Collision2D collsion) { }

    public override void OnCollisionExit(BossScript boss, Collision2D collision)
    {
    }

    public override string GetStateName()
    {
        return "MelleAttackState";
    }

    protected IEnumerator MelleAttack(BossScript boss)
    {
        boss.PlayAnimation("BossArranhando(Ataque3)");
        SfxManager.PlaySound(SfxManager.Sound.javaliAttack);
        yield return new WaitForSeconds(0.3f);
        yield return new WaitForSeconds(1f);
        boss.SwitchState(boss.patrollState);
    }
}
