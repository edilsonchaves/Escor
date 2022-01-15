using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA_Javali_Tiro : MonoBehaviour
{

    public float cadence, attackDistance, shootForce;

    public Transform ShootPosition;
    public GameObject bulletPrefab;

    public LayerMask groundLayer, playerLayer;
    public Animator anim;

    private float startX, aux, currentDirection;
    private GameObject bullet;
    private bool attacking;
    private Transform target;

    private List<GameObject> bulletsActive = new List<GameObject>(){}, bulletsDisabled = new List<GameObject>(){}; 

    // Start is called before the first frame update
    void Start()
    {
        target           = GameObject.FindGameObjectWithTag("Player").transform;
        startX           = ShootPosition.position.x;
        currentDirection = 1;
    }

    // Update is called once per frame
    void Update()
    {

        if(PlayerIsNear() && !HaveObstacle() && !attacking)
        {
            SetFaceToPlayer();
            aux += Time.deltaTime;
            if(aux > 1/cadence)
            {
                aux = 0;
                Shoot();
            }
        }
        else if(!attacking)
        {
            ChangeAnimation("JavaliParado2", false);
            aux = 0;
        }
    }

    //muda a animação do javali
    protected void ChangeAnimation(string str, bool restart=true)
    {
        if(restart)
        {
            anim.Play(str, -1, 0f);
        }
        else
        {
            anim.Play(str);
        }
    }


    protected void Shoot()
    {
        StartCoroutine(Shoot_());
    }


    protected IEnumerator Shoot_()
    {
        attacking = true;
        ChangeAnimation("JavaliAtacando");
        yield return new WaitForSeconds(0.5f);
        bullet          = GetBullet();
        bullet.GetComponent<BulletScript>().script   = GetComponent<IA_Javali_Tiro>();
        Vector2 force   = CalculateDirectionToAttack();
        bullet.GetComponent<Rigidbody2D>().AddForce(force*shootForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);
        attacking = false;
    }

    protected void InvertDirection()
    {
        currentDirection *= -1;
        FlipJavali();
    }


    // muda a direção do sprite do javali
    protected void FlipJavali()
    {
        transform.Rotate(Vector2.up *180);
    }


    protected Vector2 CalculateDirectionToAttack()
    {
        return (target.position - ShootPosition.position).normalized;
    }


    protected bool PlayerIsNear()
    {
        Vector2 dir = target.position - transform.position;
        float distance = dir.magnitude;
        return distance < attackDistance;
    }


    protected bool HaveObstacle()
    {
        Vector2 dir = target.position - transform.position;
        float distance = dir.magnitude;
        RaycastHit2D[] obstacles = Physics2D.RaycastAll(transform.position, dir.normalized, distance);

        foreach(RaycastHit2D r in obstacles)
        {
            if(r.collider.gameObject.layer == 7)
                return true;
        }

        return false;
    }


    // retorna uma nova bala
    protected GameObject GetBullet()
    {
        GameObject b;

        // verifica se há uma bala já criada
        if(bulletsDisabled.Count == 0) // não
        {
            // instancia uma nova bala
            b = Instantiate(bulletPrefab) as GameObject;
        }
        else // sim 
        {
            b = bulletsDisabled[0]; // pega a bala
            bulletsDisabled.Remove(b); // remove da lista de balas reservas
        }

        b.SetActive(true); // ativa
        bulletsActive.Add(b); // adiciona á lista de balas ativas
        b.transform.position = ShootPosition.position; // define a posição

        return b;
    }


    // remove uma bala específica
    public void DeletBullet(GameObject bullet)
    {
        bulletsActive.Remove(bullet);
        bulletsDisabled.Add(bullet);
        bullet.SetActive(false);
    }


    protected int GetDirectionOfPlayer()
    {
        if(target.position.x < transform.position.x)
        {
            return -1;
        }
        else if(target.position.x > transform.position.x)
        {
            return 1;
        }

        return 0;
    }

    protected void SetFaceToPlayer()
    {
        if(GetDirectionOfPlayer() != currentDirection)
            InvertDirection();
    }


    protected Vector2 CalculateForce()
    {
        Vector2 dir         = target.position - ShootPosition.position;
        Vector2 tgt         = target.position; 
        float mass          = bullet.GetComponent<Rigidbody2D>().mass;
        float gravity       = Physics.gravity.magnitude;
        float velX          = dir.magnitude/mass;
        float time          = (tgt.x - ShootPosition.position.x) / velX;
        float velY          = gravity * (time/2);
        float angle         = (Mathf.Atan2(velY, velX) * Mathf.Rad2Deg) - 15;
        time                = (tgt.x - Mathf.Sin((90 - angle) * Mathf.Deg2Rad) - ShootPosition.position.x) / velX;
        velY                = gravity * (time / 2);
        Vector2 velocity    = new Vector2(velX, velY);
        return velocity * mass;
    }


}
