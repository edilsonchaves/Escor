using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class WaveAttackState : BossBaseState
{

    public override void EnterState(BossScript boss)
    {
        Debug.Log("Execute Wave Attack");
        boss.bossCoroutineAction = boss.StartCoroutine(WaveAttack(boss));
    }
    public override void UpdateState(BossScript boss)
    {
    }

    public override void OnCollisionEnter(BossScript boss, Collision2D collsion) { }
    public override string GetStateName()
    {
        return "WaveAttackState";
    }

    protected IEnumerator WaveAttack(BossScript boss)
    {
        boss.PlayAnimation("BossBatendoNaParede");
        SfxManager.PlaySound(SfxManager.Sound.javaliMove);
        yield return new WaitForSeconds(1.45f);
        boss.SwitchState(boss.patrollState);
    }
}
