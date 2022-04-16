using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour {
    
    public Animator animator;
    public float speed;
    public float jumpForce;
    public bool noChao = true;
    public bool pulando = false;
    public bool defendendo = false;
    private bool slowmotion = false;
    private Rigidbody2D rb;
    private PlayerRopeControll ropeControll;
    public static bool canMove = true;
    [SerializeField]int _life;
    [SerializeField] int _fragmentLife;
    SpriteRenderer sprite;
    public bool isInvunerable;

    public int FragmentLife
    {
        get { return _fragmentLife;}
        set 
        { 
            if (value == 5)
            {
                GainLife();
                _fragmentLife = 0;
            }
            else
            {
                _fragmentLife = value;
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
                    if (Life == 0)
                    {
                        animator.SetTrigger("Morrendo");
                        SfxManager.PlaySound(SfxManager.Sound.playerDie);
                        LevelManager.levelstatus = LevelManager.LevelStatus.EndGame;
                        StartCoroutine(DiePersonagem());
                    }
                    else
                    {
                        Debug.Log("Teste");
                        animator.SetTrigger("TakeDamage");
                        SfxManager.PlaySound(SfxManager.Sound.playerHurt);
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
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        ropeControll = GetComponent<PlayerRopeControll>();
        _life = 3;
        ManagerEvents.PlayerMovementsEvents.LifedPlayer(_life);
        sprite = GetComponent<SpriteRenderer>();
        _powerHero = Manager_Game.Instance.sectionGameData.GetPowersAwarded();
        timeAbilityDefense = new float[2];
        timeAbilityDefense[1] = 5;
        timeAbilityDefense[0] = 5;
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
        if (LevelManager.levelstatus == LevelManager.LevelStatus.Game) 
        {
            
            animator.SetBool("Balancando", ropeControll.attached);


            if (canMove && !ropeControll.attached)
            {
    
                animator.SetBool("NoChao", noChao);
               
                animator.SetBool("Caindo", noChao == false && pulando == false && rb.velocity.y < -0.5f);
                
                bool canJump = noChao && !pulando && _powerHero[0];

                if(Input.GetButtonDown("Jump") && canJump)
                {
                    pulando = true;
                    // noChao = false;
                    SfxManager.PlaySound(SfxManager.Sound.playerJump);
                    animator.SetBool("Pulando", true);
                    animator.Play("pulando", -1, 0);
                    
                } 
                else if (noChao && !pulando)
                {
                    // pulando = false;
                    animator.SetBool("Pulando", false);
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
    }

    void Jump()
    {
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
            } 
        
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
    void OnTriggerEnter2D(Collider2D col)
    {
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
        }

        if (col.gameObject.CompareTag("FragmentLife"))
        {
            _fragmentLife++;
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


    
}
