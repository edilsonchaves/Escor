using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class MelleAttackState : BossBaseState
{
    [SerializeField] private float distanceFolga;
    public override void EnterState(BossScript boss)
    {
        Debug.Log("Execute Melle Attack");
        boss.bossCoroutineAction = boss.StartCoroutine(MelleAttack(boss));
    }
    public override void UpdateState(BossScript boss) 
    {
    }

    public override void OnCollisionEnter(BossScript boss, Collision2D collsion) 
    {
        if (collsion.gameObject.tag=="Player")
        {
            Debug.Log("Collision Enter Player");
        }
    }

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
        SfxManager.PlaySound(SfxManager.Sound.lobeAttack);
        yield return new WaitForSeconds(0.5f);
        Vector2 targetPosition = new Vector2(boss.GetTargetPosition().x, boss.transform.position.y);
        if (Vector2.Distance(targetPosition, boss.transform.position) > distanceFolga)
        {
            Debug.Log("Dano Player");
            boss.GetTarget().GetComponent<Movement>().Life--;
        }
        else
        {
            Debug.Log("Sem Dano Player");
        }
        yield return new WaitForSeconds(0.5f);
        boss.SwitchState(boss.patrollState);
    }
}
