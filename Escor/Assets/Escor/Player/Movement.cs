using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour {

    public static int keepingMeStopped;
    public Animator animator;
    public float speed;
    public float jumpForce;
    public bool noChao = true;
    public bool pulando = false;
    public bool atacando = false;
    public bool defendendo = false;
    private bool slowmotion = false;
    private bool caindo = false;
    private float maxJumpForce;
    private Rigidbody2D rb;
    private PlayerRopeControll ropeControll;
    [SerializeField]public static bool canMove = true;
    [SerializeField]int _life;
    [SerializeField] float _fragmentLife;
    SpriteRenderer sprite;
    public bool isInvunerable;
    bool insideCave;
    Coroutine GainLifeUIUpdate;
    public float FragmentLife
    {
        get { return _fragmentLife;}
        set
        {
            if (value == 5)
            {
                GainLife();
                _fragmentLife = 0;
                ManagerEvents.PlayerMovementsEvents.PlayerGetedFragmentLife(_fragmentLife, 5.0f);
                GainLifeUIUpdate=StartCoroutine(UpdateGainLifeUI());
            }
            else
            {
                _fragmentLife = value;
                Debug.Log("Teste");
                ManagerEvents.PlayerMovementsEvents.PlayerGetedFragmentLife(_fragmentLife,5.0f);
            }
        }
    }
    public int Life
    {

        get { return _life; }
        set {

            if (value > _life)
            {
                _life = value;
                SfxManager.PlaySound(SfxManager.Sound.playerGetLife);
            }
            else
            {
                if (isInvunerable == false)
                {
                    _life = value;
                    if (GainLifeUIUpdate != null)
                        StopCoroutine(GainLifeUIUpdate);
                    if (Life == 0)
                    {
                        // [Jessé]
                        if(Manager_Game.Instance.sectionGameData.GetCurrentLevel() == 2)
                            PlayerPrefs.SetInt("SkipConversationOfTurtle", 1); // diz a tartaruga para pular o diálogo

                        animator.SetTrigger("Morrendo");
                        SfxManager.PlaySound(SfxManager.Sound.playerDie);
                        LevelManager.levelstatus = LevelManager.LevelStatus.EndGame;
                        StartCoroutine(DiePersonagem());
                    }
                    else
                    {
                        animator.SetTrigger("TakeDamage");
                        // SfxManager.PlaySound(SfxManager.Sound.playerHurt);
                        SfxManager.PlayRandomHurt(); // [Jessé] toca um som aleátorio toda vez
                        PersonagemMudarEstado();
                    }
                }
                ManagerEvents.PlayerMovementsEvents.LifedPlayer(Life);
            }
        }
    }

    [SerializeField] bool[] _powerHero;
    public bool[] PowerHero { get { return _powerHero; } private set { } }
    [SerializeField] float[] timeAbilityDefense;
    IEnumerator DiePersonagem()
    {
        yield return new WaitForSeconds(1f);
        ManagerEvents.PlayerMovementsEvents.DiedPlayer();

    }
    public void PersonagemMudarEstado()
    {
        StartCoroutine(InvunerablePersonagem());
    }
    IEnumerator InvunerablePersonagem()
    {
        isInvunerable = true;
        float timeAnimation = 3;
        int status = -1;
        while (timeAnimation > 0)
        {
            float timeFrame= Time.deltaTime;
            timeAnimation -= timeFrame;
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, sprite.color.a+(timeFrame * status*2));
            if(sprite.color.a<0|| sprite.color.a > 1)
            {
                status *= -1;

            }
            yield return new WaitForSeconds(timeFrame);
        }
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1);
        isInvunerable = false;
        yield return null;
    }
    void Awake()
    {
        keepingMeStopped        = 0; // [Jessé]
        animator                = GetComponent<Animator>();
        rb                      = GetComponent<Rigidbody2D>();
        ropeControll            = GetComponent<PlayerRopeControll>();
        _life                   = 3;
        ManagerEvents.PlayerMovementsEvents.LifedPlayer(_life);
        sprite                  = GetComponent<SpriteRenderer>();
        _powerHero              = Manager_Game.Instance.sectionGameData.GetPowersAwarded();
        timeAbilityDefense      = new float[2];
        timeAbilityDefense[1]   = 5;
        timeAbilityDefense[0]   = 5;
    }

    private void OnEnable()
    {
        ManagerEvents.PlayerMovementsEvents.onLookDirection += LookDirection;
    }
    private void OnDisable()
    {
        ManagerEvents.PlayerMovementsEvents.onLookDirection -= LookDirection;
        SfxManager.TiposDeSom = new List<GameObject>();
    }

    void FixedUpdate()
    {

        if(noChao)
            rb.velocity = new Vector2(0, rb.velocity.y); // impedir que o player fique deslisando

        if (LevelManager.levelstatus == LevelManager.LevelStatus.Game)
        {
            if (canMove && !ropeControll.attached && !defendendo)
            {
                Move();
            }
            else if(!canMove)
            {
                animator.SetFloat("VelocidadeX", 0);
            }
        }
    }

    void Update()
    {
        // print("_> "+LevelManager.levelstatus);
        canMove = keepingMeStopped == 0; // [Jessé]
        if(!canMove)
        {
            animator.SetBool("Pulando", false);
            animator.SetBool("Caindo", false);
            animator.SetBool("NoChao", true);
            pulando = false;
        }
        animator.SetFloat("VelocidadeY", rb.velocity.y);
        // print("_> keepingMeStopped: "+keepingMeStopped);


        if (LevelManager.levelstatus == LevelManager.LevelStatus.Game)
        {

            animator.SetBool("Balancando", ropeControll.attached);


            if (canMove && !ropeControll.attached)
            {
                if(rb.velocity.y < maxJumpForce)
                {

                    pulando = false;
                    animator.SetBool("Pulando", false);
                    maxJumpForce = 0;
                }

                animator.SetBool("NoChao", noChao);

                // if(noChao)
                // {
                //     caindo = false;
                // }
                if(noChao == false && rb.velocity.y < -0.5f)
                {
                    // animator.SetBool("Caindo", true);
                    caindo = true;
                } else if(noChao == true || rb.velocity.y >=0)
                {
                    caindo = false;
                    // animator.SetBool("Caindo", false);
                }
                animator.SetBool("Caindo", noChao == false && pulando == false && rb.velocity.y < -0.5f);

                bool canJump = noChao && !pulando && _powerHero[0];

                if(Input.GetButtonDown("Jump") && canJump)
                {
                    pulando = true;
                    // noChao = false;
                    SfxManager.PlaySound(SfxManager.Sound.playerJump);
                    animator.SetBool("Pulando", true);
                    animator.Play("pulando normal", -1, 0);
                    // animator.SetBool("Pulando", false);




                }
                else if (noChao && !pulando)
                {
                    // pulando = false;
                    animator.SetBool("Pulando", false);
                    animator.SetBool("Atacando", false);
                }


                if (defendendo)
                {
                    if (timeAbilityDefense[0] > 0)
                        timeAbilityDefense[0] -= Time.deltaTime;
                    else
                    {
                        timeAbilityDefense[0] = 0;
                        animator.SetBool("Defendendo", false);
                        defendendo = false;
                    }
                }
                else
                {
                    if (timeAbilityDefense[0] < timeAbilityDefense[1])
                        timeAbilityDefense[0] += Time.deltaTime;
                    else
                        timeAbilityDefense[0] = timeAbilityDefense[1];
                }
                ManagerEvents.PlayerMovementsEvents.PlayerDefensedPower(timeAbilityDefense[0],timeAbilityDefense[1]);



            }

            Defense();

            SlowMotion();

            Stun();

        }

    }

    void Move()
    {
        Vector3 movement = new Vector2(Input.GetAxis("Horizontal"), 0f);

        if(slowmotion == true)
        {
            transform.position += movement * Time.fixedDeltaTime * (speed * 1.5f);
            if(pulando == true)
            {
                movement.y = 0.10f;

            }
        }
        if(slowmotion == false)
        {
            transform.position += movement * Time.fixedDeltaTime * speed;
        }
        float inputAxis = Input.GetAxis("Horizontal");
        animator.SetFloat("VelocidadeX", Mathf.Abs (inputAxis));

        if(inputAxis > 0)
        {
            LookDirection(0);
            if (noChao)
                SfxManager.PlaySound(SfxManager.Sound.playerMove);

        }

        if (inputAxis < 0)
        {
            LookDirection(180);
            if(noChao)
                SfxManager.PlaySound(SfxManager.Sound.playerMove);

        }

        // [Jessé]
        if(inputAxis != 0)
        {
            LookDirection(inputAxis < 0 ? 180 : 0);
            if(noChao)
                SfxManager.PlaySound(insideCave ? SfxManager.Sound.playerMoveCaverna : SfxManager.Sound.playerMove);
        }
    }

    void Jump()
    {
        if(keepingMeStopped != 0)
        {
            return;
            pulando = false;
        }
        // if(Input.GetButtonDown("Jump") && noChao)
        // {
            pulando = true;
            // noChao = false;
            // animator.SetBool("Pulando", pulando);
            if(slowmotion == true)
            {

                rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            }
            if(slowmotion == false)
            {
                rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                maxJumpForce = rb.velocity.y;

            }

    }

    public static void StopKeepPlayerStopped()
    {
        keepingMeStopped--;

        if(keepingMeStopped < 0)
            keepingMeStopped = 0;
    }


    public static void KeepPlayerStopped()
    {
        keepingMeStopped++;
    }

    void Defense()
    {

        if (_powerHero[1] && pulando == false)
        {
            if (Input.GetButtonDown("Defesa") && timeAbilityDefense[0] >0)
            {
                defendendo = true;
                animator.SetBool("Defendendo", true);
                canMove = false;

            }
            else if ((Input.GetButtonUp("Defesa") || timeAbilityDefense[0] <= 0) && defendendo)
            {
                if(defendendo)
                    animator.Play("defesa");
                defendendo = false;
                canMove = true;
                animator.SetBool("Defendendo", false);
            }
        }

    }
    void SlowMotion()
    {
        if(Input.GetButtonDown("Tempo") && slowmotion == false)
        {
            Debug.Log("slowmotion ativo");
            slowmotion = true;
            Time.timeScale = 0.75f;
            Time.fixedDeltaTime = Time.timeScale * .02f;

            //Efeito slowmotion ativo
        } else if (Input.GetButtonDown("Tempo") && slowmotion == true)
        {
            Debug.Log("slowmotion inativo");
            slowmotion = false;
            Time.timeScale = 1;
            Time.fixedDeltaTime = Time.timeScale * .02f;
            //Efeito slowmotion inativo
        }
    }

    // IEnumerator StunningTime()
    // {

    //     // yield return new WaitUntil(() => (animator.GetCurrentAnimatorStateInfo(0).IsName("pulando ataque"))); // espera a animação mudar para 'PortaoAbrindo'

    // }

    void Stun()
    {

        float execAnimator = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        if(!noChao && Input.GetButtonDown("Stun")) //troca pra vermelho
        {
            atacando = true;
            animator.Play(caindo?"caindo ataque":"pulando ataque", -1, execAnimator);
            // if(!caindo)
            // {
            //     animator.Play("pulando ataque", -1, execAnimator);

            // }
            // else
            // {
            //     animator.Play("caindo ataque", -1, execAnimator);

            // }


            animator.SetBool("Atacando", true);
        }
        else if (!noChao && Input.GetButtonUp("Stun")) // troca pra branco
        {
            atacando = false;
            animator.Play(caindo?"caindo":"pulando normal", -1, execAnimator);
            // if(!caindo)
            // {
            //     animator.Play("pulando normal", -1, execAnimator);

            // }
            // else
            // {
            //     animator.Play("caindo", -1, execAnimator);
            // }

            animator.SetBool("Atacando", false);

        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        // [Jessé]
        if(col.tag == "Cave Interior")
        {
            insideCave = true;
        }

        if (col.gameObject.CompareTag("Vida") && Life<3)
        {
            GainLife();
            Destroy(col.gameObject);
        }

        if (col.gameObject.CompareTag("Power"))
        {
            _powerHero[(int)col.gameObject.GetComponent<PowerScript>().GetPower()]= true;
            ManagerEvents.PlayerMovementsEvents.PlayerGetedPower((int)col.gameObject.GetComponent<PowerScript>().GetPower());
            SfxManager.PlaySound(SfxManager.Sound.playerGetNewPower);

            Destroy(col.gameObject);
        }

        if (col.gameObject.CompareTag("FragmentMemory"))
        {
            ManagerEvents.PlayerMovementsEvents.PlayerObtainedFragmentMemory();
            Destroy(col.gameObject);
        }

        if (col.gameObject.CompareTag("FragmentLife") && (Life<3 || FragmentLife<4))
        {
            FragmentLife++;
            Debug.Log("FragmentLife");
            Destroy(col.gameObject);
        }

    }

    void OnCollisionEnter2D(Collision2D col)
    {

        if(Mathf.Round(col.contacts[0].normal.y) == 1 && col.gameObject.tag == "Javali")
        {
            rb.velocity = new Vector2(Mathf.Abs(transform.eulerAngles.y) > 0 ? -1.5f : 1.5f, 8);
            if (atacando) col.gameObject.GetComponent<IA_Javali>().JavaliStuned();

        }

    }

    // [Jessé]
    void OnTriggerExit2D(Collider2D col)
    {
        if(col.tag == "Cave Interior")
        {
            insideCave = false;
        }
    }

    public void LookDirection(float yAngle)
    {
        transform.eulerAngles = new Vector2(0f, yAngle);
    }

    public void GainLife()
    {
        Life++;
    }
    IEnumerator UpdateGainLifeUI()
    {
        yield return new WaitForSeconds(1f);
        ManagerEvents.PlayerMovementsEvents.LifedPlayer(Life);
        GainLifeUIUpdate = null;
    }


}
