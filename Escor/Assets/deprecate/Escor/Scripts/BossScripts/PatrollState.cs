using System;
using UnityEngine;

[Serializable]
public class PatrollState : BossBaseState
{
    
    [SerializeField] private float timeToAttack;
    private float currentTimeToAttack;
    [SerializeField] private float speedWalk;
    [SerializeField] private float distanceFolga;
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
                    boss.SwitchState(boss.rangeState);
                }
                else
                {
                    boss.SwitchState(boss.melleState);
                }
            }
            else
            {
                Vector2 targetPosition = new Vector2(boss.GetTargetPosition().x, boss.transform.position.y);
                if (boss.GetTargetPosition().x < boss.transform.position.x)
                {
                    boss.transform.localScale = new Vector3(0.4708609f, 0.4708609f, 0.4708609f);
                }
                else
                {
                    boss.transform.localScale = new Vector3(-0.4708609f, 0.4708609f, 0.4708609f);

                }
                if (Vector2.Distance(targetPosition, boss.transform.position) > distanceFolga)
                {

                    boss.gameObject.transform.position = Vector2.MoveTowards(boss.transform.position, targetPosition, speedWalk * Time.deltaTime);
                    boss.PlayAnimation("BossAndandoLoop");
                }
                else 
                {
                    boss.PlayAnimation("BossParado");
                }

            }
        }
    }

    public override void OnCollisionEnter(BossScript boss, Collision2D collision) 
    {
        if(collision.gameObject.tag == "Bullet")
        {

        }
    }

    public override void OnCollisionExit(BossScript boss, Collision2D collision)
    {
    }

    public override string GetStateName()
    {
        return "PatrollState";
    }
}
