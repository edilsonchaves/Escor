using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class Movement : MonoBehaviour {

    // [Jessé]
    public static int   keepingMeStopped;
    [Range(-1f, -100f)]
    public float fallingVelocity = -20;
    public        bool  doubleJump        = true;
                  bool  usingDoubleJump   = false;
                  bool  walkingOnSpikes   = false;
    public        bool  usingStun         = false;
    public        float jumpDistanceY     = 2; // quantidade de blocos que o pulo alcança
    [Range(0.1f, 1.0f)]
    public        float PercentageOfSpeedWhenWalkingOnSpikes = 0.5f; 



    public int jumpsLeft = 2;
    public Animator animator;
    public float speed;
    float jumpForce; // [Jessé] quem define a força é o script
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
    public bool insideCave;
    Coroutine GainLifeUIUpdate;
    public float FragmentLife
    {
        get { return _fragmentLife;}
        set
        {
            if (value == 5)
            {
                _fragmentLife = 0;
                ManagerEvents.PlayerMovementsEvents.PlayerGetedFragmentLife(_fragmentLife, 5.0f);
                GainLifeUIUpdate=StartCoroutine(UpdateGainLifeUI());
            }
            else
            {
                _fragmentLife = value;
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
                    else if(animator.GetBool("BulletDamage") == false)
                    {
                        
                        animator.SetTrigger("TakeDamage");
                        // SfxManager.PlaySound(SfxManager.Sound.playerHurt);
                        SfxManager.PlayRandomHurt(); // [Jessé] toca um som aleátorio toda vez
                        PersonagemMudarEstado();
                    }
                }
            }
            ManagerEvents.PlayerMovementsEvents.LifedPlayer(Life);
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
        jumpsLeft = 2;
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
        CalculateJumpLine();

        if(noChao)
            rb.velocity = new Vector2(0, rb.velocity.y); // impedir que o player fique deslisando

        if (LevelManager.levelstatus == LevelManager.LevelStatus.Game)
            Move();
    }

    void Update()
    {
        print("Bullet" + animator.GetBool("BulletDamage"));
        print("Take" + animator.GetBool("TakeDamage"));
        canMove = keepingMeStopped == 0 && !ropeControll.attached && !defendendo; // [Jessé]

        if (LevelManager.levelstatus == LevelManager.LevelStatus.Game)
        {
            // Move();
            Jump();
            Defense();
            // SlowMotion();
            Stun();
        }

        animator.SetFloat("VelocidadeY", rb.velocity.y);


        if (LevelManager.levelstatus == LevelManager.LevelStatus.Game)
        {

            animator.SetBool("Balancando", ropeControll.attached);


            if (!ropeControll.attached)
            {
                if(rb.velocity.y < maxJumpForce)
                {

                    // pulando = false;
                    animator.SetBool("Pulando", false);
                    maxJumpForce = 0;
                }

                animator.SetBool("NoChao", noChao);


                if(noChao == false && rb.velocity.y < 0)
                {
                    caindo = true;
                } else if(noChao == true || rb.velocity.y >=0)
                {
                    caindo = false;
                }

                if(caindo && rb.velocity.y < fallingVelocity)
                {
                    animator.SetBool("Caindo 2", true);
                } else if (!caindo)
                {
                    animator.SetBool("Caindo 2", false);
                }

                animator.SetBool("Caindo", noChao == false && rb.velocity.y < 0);

                if (noChao && !pulando)
                {
                    // pulando = false;
                    usingStun = false;
                    atacando = false;
                    jumpsLeft = 2;
                    usingDoubleJump = false;
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

                ManagerEvents.PlayerMovementsEvents.PlayerDefensedPower(timeAbilityDefense[0],timeAbilityDefense[1]); // o que isso faz?

            }


        }

    }

    void Move()
    {
        if(!canMove) // não pode se mover
        {
            animator.SetFloat("VelocidadeX", 0);
            return;
        }

        Vector3 movement = new Vector2(Input.GetAxis("Horizontal"), 0f);

        if(slowmotion == true)
        {

            // trocar depois por Time.deltaTime (em todo lugar) e vê se funciona bem

            transform.position += movement * Time.fixedDeltaTime * (speed * 1.5f); 
            if(pulando == true)
            {
                movement.y = 0.10f;

            }
        }
        if(slowmotion == false)
        {
            transform.position += movement * Time.fixedDeltaTime *(walkingOnSpikes?PercentageOfSpeedWhenWalkingOnSpikes*speed:speed);
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


    // [Jessé]
    private void CalculateJumpLine()
    {
        // USAR MASSA 1

        float g = rb.gravityScale * Physics2D.gravity.magnitude;
        // print(g);
        jumpForce = Mathf.Sqrt((2*g)*(jumpDistanceY*0.97f)); // o quadrado é 0.97x0.97
        // float v0 = jumpForce / rb.mass; // converts the jumpForce to an initial velocity
        // 1 = (v0 * v0)/(2*g)
        // (2*g)*blocosY = v0^0.5
        // float maxJump_y = transform.position.y + Mathf.Sqrt(jumpForce);
        // float maxJump_y = transform.position.y + (v0 * v0)/(2*g);

        // For Debug.DrawLine in FixedUpdate :
        // Vector3 lineStart = new Vector3(-100, maxJump_y, 0);
        // Vector3 lineEnd = new Vector3(100, maxJump_y, 0);
        // //
        // Debug.DrawLine(lineStart, lineEnd, Color.red);
    }


    // [Jessé]
    public void AddForceUp()
    {
        // é subtraido a velocity.y atual para que o pulo tenha a mesma altura sempre

        // porque por exemplo, se ela tiver caindo a uma velocidade de -20, e a força do pulo for 10
        // ela ainda vai continuar caindo, pois a velocidade resultante será -10 porque 10 + (-20) = -10,
        // mas de se fizer 10 - (-20), a força do pular será 30, então levando em consideração
        // o -20 de queda e 30 se subida, o resultado é uma força de 10 para cima
        rb.AddForce(new Vector2(0f, jumpForce - rb.velocity.y), ForceMode2D.Impulse);

        if(!slowmotion)
            maxJumpForce = rb.velocity.y;
    }


    bool CanJump()
    {
        if(!canMove)                     return false; // não pode se mover
        else if(_powerHero.Length == 0)  return false; // não existe valor no array
        else if(!_powerHero[0])          return false; // não tem o poder do pulo
        else if(!((noChao && !animator.GetBool("Pulando") && !pulando) || jumpsLeft > 0)) return false; // não pode pular 

        return true; // pode pular
    }


    void Jump()
    {
        if(!Input.GetButtonDown("Jump")) return; // não está querendo pular 
        else if (!CanJump()) return; // não pode pular 😥

        // parabéns por chegar até aqui
        // isso significa que você vai pular :)


        // if(keepingMeStopped != 0)
        // {
        //     pulando = false;
        //     return;
        // }

        if(caindo && !pulando) // chegou perto da beirada da plataforma e caiu
        {
            jumpsLeft = 1; // vai direto pro segundo pulo
        }

        switch(jumpsLeft)
        {
            case 2: // primeiro pulo

                animator.Play("pulando normal", -1, 0.33f); // começa a animação já no segundo frame
                AddForceUp();
                usingDoubleJump = false;
                break;

            case 1: // segundo pulo

                // é necessário esperar ocorrer o primeiro pulo antes de pular pela segunda vez
                bool firstJump = animator.GetCurrentAnimatorStateInfo(0).IsName("pulando normal") &&
                                 animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.66f;

                // a primeira condição serve para impedir que o segundo pulo ocorra logo em seguida do primeiro
                if((rb.velocity.y <= jumpForce*0.9f && firstJump) || caindo)
                {
                    animator.Play("pulando ataque", -1, 0.33f); // começa a animação já no segundo frame
                    usingDoubleJump = true;
                    AddForceUp();
                    break;
                }
                else
                {
                    return;
                }

            case 0: // não pode mais pular
                return;
        }

        // AddForceUp();
        SfxManager.PlaySound(SfxManager.Sound.playerJump);
        pulando = true;
        jumpsLeft--;

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

    public void StartPlayer()
    {
        keepingMeStopped--;

        if(keepingMeStopped < 0)
            keepingMeStopped = 0;
    }


    public void StopPlayer()
    {
        keepingMeStopped++;
    }

    void Defense()
    {
        if(!noChao || keepingMeStopped > 0 || ropeControll.attached) // não está no chão ou não pode se mover ou está na corda
            return;

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

        // if(!canMove) // não pode se mover
        //     return;

        if(Input.GetButtonDown("Tempo") && slowmotion == false)
        {
            slowmotion = true;
            Time.timeScale = 0.75f;
            Time.fixedDeltaTime = Time.timeScale * .02f;

            //Efeito slowmotion ativo
        } else if (Input.GetButtonDown("Tempo") && slowmotion == true)
        {
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
        if(usingStun || noChao || !canMove) // está usando o stun ou está no chão ou não pode se mover
            return;


        if(Input.GetButtonDown("Stun"))
        {
            float execAnimator = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            usingStun = true;
            rb.velocity = new Vector2(rb.velocity.x, usingDoubleJump?-12:-10);
            atacando = true;
            animator.Play("caindo ataque", -1, animator.GetCurrentAnimatorStateInfo(0).IsName("caindo")?execAnimator:0);

            animator.SetBool("Atacando", true);
        }



        // else if (!noChao && Input.GetButtonUp("Stun")) // troca pra branco
        // {
        //     atacando = false;
        //     animator.Play(caindo?"caindo":"pulando normal", -1, execAnimator);
        //
        //     animator.SetBool("Atacando", false);
        //
        // }

        // if(!noChao && Input.GetButtonDown("Stun")) //troca pra vermelho
        // {
        //     atacando = true;
        //     animator.Play(caindo?"caindo ataque":"pulando ataque", -1, execAnimator);
        //
        //     animator.SetBool("Atacando", true);
        // }
        // else if (!noChao && Input.GetButtonUp("Stun")) // troca pra branco
        // {
        //     atacando = false;
        //     animator.Play(caindo?"caindo":"pulando normal", -1, execAnimator);
        //
        //     animator.SetBool("Atacando", false);
        //
        // }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        // [Jessé]
        if(col.tag == "Cave Interior")
        {
            insideCave = true;
        }
        else if(col.tag == "Spikes")
        {
            walkingOnSpikes = true;
        }

        if (col.gameObject.CompareTag("Life") && Life<3)
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
            col.GetComponent<GetMemory>().PrefabActive(false);
        }

        if (col.gameObject.CompareTag("FragmentLife") && (Life<3 || FragmentLife<4))
        {
            FragmentLife++;

            Destroy(col.gameObject);
        }
        if(col.tag == "Bullet")
        {
           
            
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
        else if(col.tag == "Spikes")
        {
            walkingOnSpikes = false;
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
        GainLife();
        ManagerEvents.PlayerMovementsEvents.LifedPlayer(Life);
        GainLifeUIUpdate = null;
    }

    public void LoadLifeInitial(int value) 
    {
        _life = value;
        ManagerEvents.PlayerMovementsEvents.LifedPlayer(_life);
    }
    public void BulletDamage()
    {
        animator.SetBool("BulletDamage", false);
    }

}
