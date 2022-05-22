using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JavaliTiro : IA_Javali
{

    public float cadence, shootForce;

    public Transform ShootPosition;
    public GameObject bulletPrefab;

    private float startX, aux;
    private GameObject bullet;

    private List<GameObject> bulletsActive   = new List<GameObject>(){},
                             bulletsDisabled = new List<GameObject>(){};

    bool parado;


    // Start is called before the first frame update
    void Start()
    {
        // print("Start from JavaliTiro");
        base.Start();
    }


    // Update is called once per frame
    void Update()
    {
        if (LevelManager.levelstatus == LevelManager.LevelStatus.Game)
        {
            isGrounded = CheckIsGrounded();
            ShowExclamation(true);  // mostra a exclamacao quando o player entra na área de ataque
            Movement(); // faz a movimentação do javali
            // if(!attacking && !parado)
            // {
            //     parado = true;
            //     ChangeAnimation("JavaliParado2", false);
            // }
            Attack();
        }
        else
        {
            JavaliAnimator.Play("JavaliParado2");
        }
    }


    protected override bool CloseToAttack()
    {
        // if(!PlayerInsideArea(true))
        //     return false;

        if(GetDistaceOfPlayer(ShootPosition.position) > AttackDistance || !activateAttack)
            return false;


        return IsFaceToPlayer()
               && !HaveObstacle(playerTrans.position, ShootPosition.position);
    }


    protected override void Attack()
    {
        if(stuned || !Move || !activateAttack)
        {
            aux = 0;
            return;
        }

        if(!CloseToAttack())
        {
            // o player não está perto o suficiente ainda
            aux = 0;
            parado = false;
            // firstContactWithPlayer = true;
        }
        else // encontrou o player
        {
            // parado = true;

            // ShowExclamation();
            FlipFaceToPlayer(); // provavelmente desnecessário

            aux += Time.deltaTime;
            if(aux > 1/cadence)
            {
                aux = 0;
                Shoot();
            }
        }
    }


    protected void Shoot()
    {
        StartCoroutine(Shoot_());
    }


    protected IEnumerator Shoot_()
    {
        attacking = true;
        // FlipFaceToPlayer();
        ChangeAnimation("JavaliAtacando2", true);
        SfxManager.PlaySound(SfxManager.Sound.javaliShoot);
        myRb.velocity = Vector3.zero;

        yield return new WaitForSeconds(0.5f);
        bullet          = GetBullet();
        bullet.transform.localScale = transform.localScale * 0.075f;
        bullet.GetComponent<BulletScript>().emissor = this.gameObject;
        Vector2 force   = CalculateDirectionToShoot();
        bullet.GetComponent<BulletScript>().valueForce = shootForce;
        bullet.GetComponent<Rigidbody2D>().AddForce(force*shootForce, ForceMode2D.Impulse);
        StartCoroutine(AttackFinished());
        // yield return new WaitForSeconds(0.5f); // Espera 0.5 seg para finalizar o ataque
        // attacking = false;
    }


    protected Vector2 CalculateDirectionToShoot()
    {
        return (playerTrans.position - ShootPosition.position).normalized;
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
        if(playerTrans.position.x < transform.position.x)
        {
            return -1;
        }
        else if(playerTrans.position.x > transform.position.x)
        {
            return 1;
        }

        return 0;
    }


    private void OnEnable()
    {
        ManagerEvents.Enemy.onRockDelete += DeletBullet;
    }


    private void OnDisable()
    {
        ManagerEvents.Enemy.onRockDelete += DeletBullet;
    }
}
