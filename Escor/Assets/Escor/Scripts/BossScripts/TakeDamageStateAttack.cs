using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class TakeDamageStateAttack : BossBaseState
{
    public override void EnterState(BossScript boss)
    {
        Debug.Log("Execute Take Damage State");
        boss.StopCoroutine(boss.bossCoroutineAction);
        boss.bossCoroutineAction = boss.StartCoroutine(TakeDamage(boss));

    }

    public override void UpdateState(BossScript boss) { }

    public override void OnCollisionEnter(BossScript boss, Collision2D collsion) { }
    public override string GetStateName()
    {
        return "TakeDamageStateAttack";
    }

    public override void OnCollisionExit(BossScript boss, Collision2D collision)
    {
    }

    protected IEnumerator TakeDamage(BossScript boss)
    {
        boss.PlayAnimation("BossLevandoDano");
        SfxManager.PlaySound(SfxManager.Sound.playerHurt_3);
        yield return new WaitForSeconds(1f);
        boss.bossCoroutineAction = null;
        boss.SwitchState(boss.patrollState);
    }
}
