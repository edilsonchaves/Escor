using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [HideInInspector] public IA_Javali_Tiro script;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Animator myAnimator;
    [SerializeField] private string tagOfPlayer = "Player", tagOfJavali = "Javali";
    [SerializeField] private Rigidbody2D myRb;
    [SerializeField] public GameObject emissor;
    
    bool destroyed;
    bool isBackToJavali;

    void Awake()
    {
        destroyed = false;
        isBackToJavali = false;
        myAnimator.Play("PedraLoop", -1, 0);
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if(destroyed)
            return;

        if(col.tag == tagOfPlayer)
        {
            // dano no player
            Movement mvt = col.gameObject.GetComponent<Movement>();
            print("mvt.defendendo:         "+mvt.defendendo);
            if(mvt.defendendo)
            {
                isBackToJavali = true;
                myRb.velocity = Vector2.zero;
                Vector2 force = CalculateDirectionToAttack();
                GetComponent<Rigidbody2D>().AddForce(force * 6, ForceMode2D.Impulse);
                return;
            }


            if (!mvt.isInvunerable)
            {
                print("Tiro acertou o player");
                mvt.Life -= 1;
            }


        }
        else if(col.tag == tagOfJavali && isBackToJavali)
        {
            script.anim.Play("JavaliTonto", -1, 0);
        }

        else if(!(groundLayer == (groundLayer | ( 1 << col.gameObject.layer))))
        {
            return;
        }

        destroyed = true;
        myRb.velocity = Vector3.zero;
        myAnimator.Play("PedraDestroy", -1, 0);
    }


    // chamado por evento na animação
    void DestroyMe()
    {
        destroyed = false;
        script.DeletBullet(this.gameObject);
    }

    protected Vector2 CalculateDirectionToAttack()
    {
        return (emissor.transform.position - this.transform.position).normalized;
    }

}
