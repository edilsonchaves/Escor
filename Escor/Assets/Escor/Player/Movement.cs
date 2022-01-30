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



    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        ropeControll = GetComponent<PlayerRopeControll>();
        
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
        animator.SetBool("NoChao", noChao);

        if(noChao == false && pulando == false && rb.velocity.y < 0)
        {
            animator.SetBool("Caindo", true);
        } else
        {
            animator.SetBool("Caindo", false);
        }
        
        if(canMove)
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

    void LookDirection(float yAngle)
    {
        transform.eulerAngles = new Vector2(0f, yAngle);
    }


    
}
