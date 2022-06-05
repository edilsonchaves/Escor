using System.Collections;
using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class RangeAttackState : BossBaseState
{
    float currentRangeAttackTime;
    public GameObject bulletPrefab;
    private GameObject bullet;
    private List<GameObject> bulletsActive = new List<GameObject>() { }, bulletsDisabled = new List<GameObject>() { };
    [SerializeField] float timeToExecuteAttack;
    [SerializeField] Transform shootPosition;
    [SerializeField] float shootForce;
    public override void EnterState(BossScript boss) 
    {
        Debug.Log("Execute Range Attack");
        boss.bossCoroutineAction=boss.StartCoroutine(Shoot_(boss));
    }


    public override void UpdateState(BossScript boss) 
    {
    }

    protected IEnumerator Shoot_(BossScript boss)
    {
        boss.PlayAnimation("BossTacandoPedra(Ataque1)");
        // SfxManager.PlaySound(SfxManager.Sound.javaliShoot);
        yield return new WaitForSeconds(0.5f);
        bullet = GetBullet();
        bullet.GetComponent<BulletScript>().emissor = boss.gameObject;
        bullet.GetComponent<BulletScript>().valueForce = shootForce; ;
        Vector2 force = CalculateDirectionToAttack(boss);
        bullet.GetComponent<Rigidbody2D>().AddForce(force * shootForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.8f);
        boss.SwitchState(boss.patrollState);
    }


    public override void OnCollisionEnter(BossScript boss, Collision2D collsion) { }

    public override void OnCollisionExit(BossScript boss, Collision2D collision)
    {
    }

    public override string GetStateName()
    {
        return "RangeAttackState";
    }


    protected Vector2 CalculateDirectionToAttack(BossScript boss)
    {
        return (boss.TargetPosition() - shootPosition.position).normalized;
    }
    protected GameObject GetBullet()
    {
        GameObject b;

        // verifica se há uma bala já criada
        if (bulletsDisabled.Count == 0) // não
        {
            // instancia uma nova bala
            b = MonoBehaviour.Instantiate(bulletPrefab) as GameObject;
        }
        else // sim 
        {
            b = bulletsDisabled[0]; // pega a bala
            bulletsDisabled.Remove(b); // remove da lista de balas reservas
        }

        b.SetActive(true); // ativa
        bulletsActive.Add(b); // adiciona á lista de balas ativas
        b.transform.position = shootPosition.position; // define a posição

        return b;
    }
    public void DeletBullet(GameObject bullet)
    {
        bulletsActive.Remove(bullet);
        bulletsDisabled.Add(bullet);
        bullet.SetActive(false);
    }

}
