using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Animator myAnimator;
                     public TrailRenderer trail;
    [SerializeField] private string tagOfPlayer = "Player", tagOfJavali = "Javali", tagOfBoss = "Boss";
    [SerializeField] private Rigidbody2D myRb;
    public GameObject emissor;
    public float valueForce;

    bool destroyed;
    bool isBackToJavali;

    void OnEnable()
    {
        // StartCoroutine(ResetTrailRenderer(trail)); // serve para desativar o rastro e impedir que ele apareca se teletransportando
        trail.emitting = true;
        trail.enabled   = true;
        destroyed       = false;
        isBackToJavali  = false;
        myAnimator.Play("PedraLoop", -1, 0);
    }

    IEnumerator ResetTrailRenderer(TrailRenderer tr) {
        float trailTime = tr.time;
        tr.time = 0;
        yield return new WaitForSeconds(0.1f);
        tr.time = trailTime;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (destroyed)
            return;

        if (col.tag == tagOfPlayer)
        {
            // dano no player
            Movement mvt = col.gameObject.GetComponent<Movement>();
            // print("mvt.defendendo:         " + mvt.defendendo);
            if (mvt.defendendo)
            {
                isBackToJavali = true;
                myRb.velocity = -myRb.velocity*1.5f;
                // myRb.velocity = Vector2.zero;
                // Vector2 force = CalculateDirectionToAttack();
                // GetComponent<Rigidbody2D>().AddForce(force * valueForce/2, ForceMode2D.Impulse);
                return;
            }


            if (!mvt.isInvunerable)
            {
                // print("Tiro acertou o player");
                mvt.animator.SetBool("BulletDamage", true);
                mvt.Life -= 1;
                
            }


        }
        else if ((col.tag == tagOfJavali || col.tag == "Javalizao") && isBackToJavali) // estava voltando e colidiu com um jalavi
        {
            // emissor.GetComponent<IA_Javali>().JavaliAnimator.Play("JavaliTonto", -1, 0);
            col.GetComponent<IA_Javali>().JavaliStuned();
            // emissor.GetComponent<IA_Javali>().JavaliStuned();
        }
        else if (col.tag == tagOfBoss)
        {
            emissor.GetComponent<Boss_Script>()._animatorBoss.Play("BossLevandoDano", -1, 0);
            ManagerEvents.Boss.TakedDamage();
        }
        else if (!(groundLayer == (groundLayer | (1 << col.gameObject.layer))))
        {
            return;
        }

        destroyed = true;
        myRb.velocity = Vector3.zero;
        // StartCoroutine(ResetTrailRenderer(trail)); // serve para desativar o rastro e impedir que ele apareca se teletransportando
        myAnimator.Play("PedraDestroy", -1, 0);
    }



    protected Vector2 CalculateDirectionToAttack()
    {
        return (emissor.transform.position - this.transform.position).normalized;
    }

    // chamado por evento na animação
    void DestroyMe()
    {
        StartCoroutine("DestroyMe_");
        // trail.enabled = false;
    }

    IEnumerator DestroyMe_()
    {
        trail.emitting = false;
        yield return new WaitForSeconds(1);
        ManagerEvents.Enemy.RockDeleted(this.gameObject);
        destroyed = false;
    }
}
