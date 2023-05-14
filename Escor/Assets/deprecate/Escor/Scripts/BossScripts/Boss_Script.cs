using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss_Script : MonoBehaviour
{
    public Animator _animatorBoss;
    [SerializeField] private List<Conversa> conversaBoss;
    [SerializeField] private List<Conversa> conversaBossMorte;
    [SerializeField] ConversaPersonagem conversa;
    [SerializeField] int bossLife;
    [SerializeField] Transform target;
    [SerializeField]float shootTime;
    [SerializeField] Transform ShootPosition;
    float shootTimeCurrent;
    bool attacking = false;
    public GameObject bulletPrefab;
    private GameObject bullet;
    private List<GameObject> bulletsActive = new List<GameObject>() { }, bulletsDisabled = new List<GameObject>() { };
    [SerializeField] float shootForce;
    enum BossStatus {Parado,Game,Die }
    BossStatus bossStatus;
    public GameObject fade;
    void Start()
    {
        conversa = GameObject.FindGameObjectWithTag("ConversaPersonagem").GetComponent<ConversaPersonagem>();
        shootTimeCurrent = 0;
    }
    void Update()
    {
        if (bossStatus==BossStatus.Game && !attacking && LevelManager.levelstatus==LevelManager.LevelStatus.Game)
        {
            shootTimeCurrent += Time.deltaTime;
            if(shootTimeCurrent> shootTime)
            {
                shootTimeCurrent -= shootTime;
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
        //ChangeAnimation("JavaliAtacando");

        SfxManager.PlaySound(SfxManager.Sound.javaliShoot);

        yield return new WaitForSeconds(0.5f);
        bullet = GetBullet();
        bullet.GetComponent<BulletScript>().emissor = this.gameObject;
        bullet.GetComponent<BulletScript>().valueForce = shootForce; ;
        Vector2 force = CalculateDirectionToAttack();
        bullet.GetComponent<Rigidbody2D>().AddForce(force * shootForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.5f);
        attacking = false;
    }
    protected Vector2 CalculateDirectionToAttack()
    {
        return (target.position - ShootPosition.position).normalized;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            GetComponent<BoxCollider2D>().enabled = false;
            StartCoroutine(InitialStatusBossBattle());
        }
    }

    IEnumerator InitialStatusBossBattle()
    {
        StartCoroutine(conversa.ConversaFase(conversaBoss));
        yield return new WaitUntil(() => conversa.StatusConversa);
        _animatorBoss.Play("BossLevantando");
        ManagerEvents.Boss.InitializedBattle();
        bossStatus = BossStatus.Game;
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    IEnumerator DieStatusBossBattle()
    {
        StartCoroutine(conversa.ConversaFase(conversaBossMorte));
        _animatorBoss.Play("BossDerrotadoStart");
        yield return new WaitUntil(() => conversa.StatusConversa);
        bossStatus = BossStatus.Die;

        // [Jessé] ----------------

            // _animatorBoss.Play("BossDerrotadoLoop");
            fade.SetActive(true);
            yield return new WaitForSeconds(1f);

            SceneManager.LoadScene("Final");

        // ------------------------

    }

    protected GameObject GetBullet()
    {
        GameObject b;

        // verifica se há uma bala já criada
        if (bulletsDisabled.Count == 0) // não
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
    public void DeletBullet(GameObject bullet)
    {
        bulletsActive.Remove(bullet);
        bulletsDisabled.Add(bullet);
        bullet.SetActive(false);
    }

    private void OnEnable()
    {
        ManagerEvents.Enemy.onRockDelete += DeletBullet;
        ManagerEvents.Boss.onTakeDamage += TakeDamage;
    }
    private void OnDisable()
    {
        ManagerEvents.Enemy.onRockDelete += DeletBullet;
        ManagerEvents.Boss.onTakeDamage += TakeDamage;
    }

    public void TakeDamage()
    {
        bossLife--;
        ManagerEvents.Boss.UpdatedLife(bossLife);
        if (bossLife == 0)
        {
            bossStatus = BossStatus.Die;
            StartCoroutine(DieStatusBossBattle());
        }
    }
}
