using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovement : MonoBehaviour
{
    public LayerMask groundLayer;
    public LayerMask platformLayer;
    public float jumpForce, speed;
    
    private bool isGrounded, isJumping;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // rb.velocity = Vector2.up * 12;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = CheckIsGrounded();
        Move();

    }


    void Move()
    {
        float inputAxis = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector2(inputAxis, 0f);
        transform.position += movement * Time.deltaTime * speed;
        
        // print(Input.GetAxis("Vertical"));
        bool jump = (Input.GetAxis("Vertical") > 0);

        if(inputAxis > 0)
        {
            LookDirection(0);
        }

        if(inputAxis < 0)
        {
            LookDirection(180);
        }

        if(jump && isGrounded && !isJumping)
        {
            Jump();
        }
    }


    void Jump()
    {
        isJumping = true;
        rb.AddForce(Vector2.up*jumpForce);
        StartCoroutine("StopJump");
    }

    void LookDirection(float yAngle)
    {
        transform.eulerAngles = new Vector2(0f, yAngle);
    }    


    protected bool CheckIsGrounded()
    {
        return Physics2D.Raycast(transform.position, -Vector2.up, 1.28f, groundLayer).distance != 0 ||
                Physics2D.Raycast(transform.position, -Vector2.up, 1.28f, platformLayer).distance != 0;
    }

    IEnumerator StopJump()
    {
        yield return new WaitForSeconds(1);
        isJumping = false;
    }
}
