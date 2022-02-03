using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour {
    
    public Animator animator;
    public float speed;
    public float jumpForce;
    public bool noChao = true;
    private bool pulando = false;
    public bool defendendo = false;
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
            if (value > 0)
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
                        animator.SetTrigger("TakeDamage");
                        PersonagemMudarEstado();
                    }
                }
            }

            ManagerEvents.PlayerMovementsEvents.LifedPlayer(Life);
        }
    }
    IEnumerator DiePersonagem()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("OIE");
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


    }

    private void OnEnable()
    {
        ManagerEvents.PlayerMovementsEvents.onLookDirection += LookDirection;
    }
    private void OnDisable()
    {
        ManagerEvents.PlayerMovementsEvents.onLookDirection -= LookDirection;
    }

    void Update()
    {
        Debug.Log("Level status:"+ LevelManager.levelstatus);
        if (LevelManager.levelstatus == LevelManager.LevelStatus.Game) 
        {
            animator.SetBool("NoChao", noChao);

            if (noChao == false && pulando == false && rb.velocity.y < 0)
            {
                animator.SetBool("Caindo", true);
            }
            else
            {
                animator.SetBool("Caindo", false);
            }

            if (canMove)
            {
                Move();
                Jump();
                Defense();
            }

            // if(!ropeControll.attached)
            // {
            //     Move();
            //     Jump();
            // }

            // if(isGrounded)
            // {
            //     isJumping = false;
            // }
        }

    }

    void Move()
    {
        Vector3 movement = new Vector2(Input.GetAxis("Horizontal"), 0f);
        transform.position += movement * Time.deltaTime * speed;
        

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
        if(Input.GetButtonDown("Jump") && noChao)
        {
            pulando = true;
            noChao = false;
            animator.SetBool("Pulando", pulando);
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        
        } else if (noChao == true)
        {
            pulando = false;
            animator.SetBool("Pulando", false);
        }
    }

    void Defense()
    {
        if(Input.GetButtonDown("Defesa"))
        {
            defendendo = true;
            animator.SetBool("Defendendo", true);
        }
        if (Input.GetButtonUp("Defesa"))
        {
            defendendo = false;
            animator.SetBool("Defendendo", false);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Vida"))
        {
            GainLife();
        }
    }

    void LookDirection(float yAngle)
    {
        transform.eulerAngles = new Vector2(0f, yAngle);
    }

    public void GainLife()
    {
        Life++;
    }
    
}
