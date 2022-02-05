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
    SpriteRenderer sprite;
    public bool isInvunerable;
    public int Life
    {
        
        get { return _life; }
        set {
            
            if (value > _life)
            {
                _life = value;
            }
            else
            {
                if (isInvunerable == false)
                {
                    _life = value;
                    if (Life == 0)
                    {
                        animator.SetTrigger("Morrendo");
                        LevelManager.levelstatus = LevelManager.LevelStatus.EndGame;
                        StartCoroutine(DiePersonagem());
                    }
                    else
                    {
                        Debug.Log("Teste");
                        animator.SetTrigger("TakeDamage");
                        PersonagemMudarEstado();
                    }
                }
            }

            ManagerEvents.PlayerMovementsEvents.LifedPlayer(Life);
        }
    }

    [SerializeField] bool[] powerHero;
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
        powerHero = new bool[3];

    }

    private void OnEnable()
    {
        ManagerEvents.PlayerMovementsEvents.onLookDirection += LookDirection;
    }
    private void OnDisable()
    {
        ManagerEvents.PlayerMovementsEvents.onLookDirection -= LookDirection;
    }

    void FixedUpdate()
    {
        if (LevelManager.levelstatus == LevelManager.LevelStatus.Game) 
        {
            if (canMove && !ropeControll.attached)
            {
                Move();
            }    
        }
    }

    void Update()
    {
        Debug.Log("Level status:"+ LevelManager.levelstatus);
        if (LevelManager.levelstatus == LevelManager.LevelStatus.Game) 
        {
            
            animator.SetBool("Balancando", ropeControll.attached);


            if (canMove && !ropeControll.attached)
            {
    
                animator.SetBool("NoChao", noChao);
               
                animator.SetBool("Caindo", noChao == false && pulando == false && rb.velocity.y < 0);
                
                // Jump();
                if(Input.GetButtonDown("Jump") && noChao && powerHero[0])
                    {
                        pulando = true;
                        noChao = false;
                        animator.SetBool("Pulando", true);
                        animator.Play("pulando", -1, 0);
                        
                    } else if (noChao == true)
                    {
                        pulando = false;
                        animator.SetBool("Pulando", false);
                    }
                Defense();
            }
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
            
        }

        if(inputAxis < 0)
        {
            LookDirection(180);
            
        }
    }

    void Jump()
    {
        // if(Input.GetButtonDown("Jump") && noChao)
        // {
            pulando = true;
            noChao = false;
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
        if (powerHero[1] && pulando == false) 
        {
            if (Input.GetButtonDown("Defesa"))
            {
                defendendo = true;
                animator.SetBool("Defendendo", true);
            }
            if (Input.GetButtonUp("Defesa"))
            {
                animator.Play("defesa");
                defendendo = false;
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
            powerHero[(int)col.gameObject.GetComponent<PowerScript>().power]= true;
            ManagerEvents.PlayerMovementsEvents.PlayerGetedPower((int)col.gameObject.GetComponent<PowerScript>().power);
            Destroy(col.gameObject);
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
