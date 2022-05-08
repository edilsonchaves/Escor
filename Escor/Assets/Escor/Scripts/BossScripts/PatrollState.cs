using System;
using UnityEngine;

[Serializable]
public class PatrollState : BossBaseState
{
    
    [SerializeField] private float timeToAttack;
    private float currentTimeToAttack;

    
    public override void EnterState(BossScript boss) 
    {
        Debug.Log("Begin Patroll");
        boss.PlayAnimation("BossParado");

    }

    public override void UpdateState(BossScript boss) 
    { 
        if(LevelManager.levelstatus == LevelManager.LevelStatus.Game)
        {
            currentTimeToAttack += Time.deltaTime;
            if (currentTimeToAttack > timeToAttack)
            {
                currentTimeToAttack = 0;
                Debug.Log(Vector2.Distance(boss.TargetPosition(), boss.transform.position));
                if (Vector2.Distance(boss.TargetPosition(), boss.transform.position) > 3)
                {
                    if(Vector2.Distance(boss.TargetPosition(), boss.transform.position) < 5)
                        boss.SwitchState(boss.waveAttackState);
                    else
                        boss.SwitchState(boss.rangeState);
                }
                else
                {
                    boss.SwitchState(boss.melleState);
                }
            }
        }
    }

    public override void OnCollisionEnter(BossScript boss, Collision2D collsion) { }

    public override string GetStateName()
    {
        return "PatrollState";
    }
}
