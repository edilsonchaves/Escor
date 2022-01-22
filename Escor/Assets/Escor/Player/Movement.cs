using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    
    Animator animator;
    public float speed;
    public float jumpForce;
    public bool noChao = true;
    private bool pulando = false;
    private Rigidbody2D rb;
    private PlayerRopeControll ropeControll;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        ropeControll = GetComponent<PlayerRopeControll>();
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
        
        Move();
        Jump();
        
        if(!ropeControll.attached)
        {
            Move();
            Jump();
        }
        
        if(isGrounded)
        {
            isJumping = false;
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
            animator.SetBool("Defendendo", true);
        }
    }

    void LookDirection(int yAngle)
    {
        transform.eulerAngles = new Vector2(0f, yAngle);
    }
}
