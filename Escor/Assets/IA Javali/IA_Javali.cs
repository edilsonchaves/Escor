using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA_Javali : MonoBehaviour
{
    [Range(1,2)]
    public int AiLevel;

    [Range(1,50)]
    public float MovementDistance;
    public bool walkInAllGround; // precisa ser melhorado
    
    public float MovementSpeed, JumpHeight, FollowDistance;
    public SpriteRenderer JavaliSprite;
    public Animator JavaliAnimator;

    [Header("Raycast")]
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public LayerMask playerLayer;
    public LayerMask javaliLayer;
    public Vector3 offSetGround, offSetWall;



    // a linha vermelha é usada para mostrar a altura do pulo e para detectar se é possível pular o obstaculo
    // a linha roxa é para detectar obstaculos
    // a linha amarela é para detectar se há colisão com o chão
    // a linha preta é para mostrar onde é o limite da movimentação do javali

    private Rigidbody2D myRb;
    private Vector3 myStartPosition;
    private int currentDirection;
    private float auxMovement;
    private bool isGrounded, following, started;
    private Transform playerTrans;


    // Gizmos - desenha o caminho - Desenvolvimento
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;     
        Gizmos.DrawRay((Vector2)(transform.position) + (Vector2)(offSetGround), -Vector2.up*offSetGround.z);
        Gizmos.DrawRay(new Vector2(transform.position.x - offSetGround.x, transform.position.y + offSetGround.y), -Vector2.up*offSetGround.z);

        Gizmos.color = Color.red;     
        Gizmos.DrawRay(new Vector2(transform.position.x+offSetWall.x, transform.position.y + JumpHeight), Vector2.right*offSetWall.z);
        Gizmos.DrawRay(new Vector2(transform.position.x-offSetWall.x, transform.position.y + JumpHeight), -Vector2.right*offSetWall.z);

        Gizmos.color = Color.magenta;     
        Gizmos.DrawRay((Vector2)(transform.position+offSetWall), Vector2.right*offSetWall.z);
        Gizmos.DrawRay(new Vector2(transform.position.x - offSetWall.x, transform.position.y + offSetWall.y), -Vector2.right*offSetWall.z);


        Gizmos.color = Color.black;     
        if(started)
        {
            Gizmos.DrawRay(new Vector2(myStartPosition.x - MovementDistance, myStartPosition.y+0.5f), -Vector2.up);
            Gizmos.DrawRay(new Vector2(myStartPosition.x + MovementDistance, myStartPosition.y+0.5f), -Vector2.up);
        }
        else
        {
            Gizmos.DrawRay(new Vector2(transform.position.x - MovementDistance, transform.position.y+0.5f), -Vector2.up);
            Gizmos.DrawRay(new Vector2(transform.position.x + MovementDistance, transform.position.y+0.5f), -Vector2.up);
        }
    }


    void Start()
    {
        started = true;
        playerTrans = GameObject.FindGameObjectWithTag("Player").transform;
        myRb = GetComponent<Rigidbody2D>();
        myStartPosition = myRb.position;
        currentDirection = RandomDirection();
        // currentDirection = -1;
        if(currentDirection == -1)
            FlipJavali();

    }


    void FixedUpdate()
    {
        isGrounded = CheckIsGrounded();
        // print("isGrounded:  "+isGrounded);
        // print("CheckIfPlayerIsOnDifferentGround: "+CheckIfPlayerIsOnDifferentGround());
        Movement(); // faz a movimentação do javali
        // if(HitWall())
        // {
        //     RaycastHit2D w = GetWall();
        //     GetHeightOfWall(w);
        // }
    
        CanJumpWall();

    }


    // aqui é feito todo o calculo para movimentar o javali (IA Level 1)
    void Movement()
    {
        auxMovement = Time.fixedDeltaTime * MovementSpeed * currentDirection * 100; // calcula quanto ele deve se mover
        Vector2 newPosition = myRb.position + new Vector2(auxMovement*Time.fixedDeltaTime, 0); // salva a nova posição após o movimento

        if(AiLevel == 1)
        {
            myRb.velocity = new Vector2(auxMovement, myRb.velocity.y); // movimenta para a nova posição

            if(walkInAllGround)
            {
                if(CheckIsGrounded())
                {
                    if(HitWall() && GetDistanceOfCollisionWithWall(GetWall()) < 0.2f) 
                    {
                        InvertDirection();
                    }
                }
                else
                {
                    InvertDirection(); // muda a direção
                }
            }
            else if(HitWall() && GetDistanceOfCollisionWithWall(GetWall()) < 0.2f || IsOutLimite(newPosition)) // verifica se a nova posição está fora do limite
            {
                InvertDirection();
            }
            // else if(IsOutLimite(newPosition)) 
            // {
            //     InvertDirection();
            // }
        }
        else if(AiLevel == 2)
        {
            //ainda tenho que terminar
        }






        // if(walkInAllGround)
        // {
        //     bool groundCollision = CheckIsGrounded();

        //     if(groundCollision)
        //     {
        //         myRb.MovePosition(newPosition); // movimenta para a nova posição
        //     }
        //     else
        //     {
        //         InvertDirection(); // muda a direção
        //     }

        // }
        // else if(GetXDistaceOfPlayer() < FollowDistance && CanJumpWall() )
        // {
        //     FlipFaceToPlayer();
        //     following = true;
        // }
        // else if(!IsOutLimite(newPosition) || following) // verifica se a nova posição está fora do limite
        // {
        //     myRb.velocity = new Vector2(auxMovement, myRb.velocity.y);
            
        //     if(HitWall())
        //     {
        //         if(CanJumpWall())
        //         {
        //             Jump();
        //         }
        //         else if(GetDistanceOfCollisionWithWall(GetWall()) < 0.2f) 
        //         {
        //             following = false;
        //             InvertDirection();
        //         }
        //     }
         
        //     // myRb.AddForce(new Vector2(auxMovement, 0), ForceMode2D.Force); // movimenta para a nova posição
        // }
        // else if(following)
        // {
        //     FlipFaceToPlayer();
        //     myRb.velocity = new Vector2(auxMovement, myRb.velocity.y);
        // }
        // else
        // {
        //     InvertDirection();
        // }
    }


    private void InvertDirection()
    {
        currentDirection *= -1; // muda a direção
        FlipJavali();
    }

    private void FlipJavali()
    {
        // JavaliSprite.flipX = !JavaliSprite.flipX;
        transform.Rotate(Vector2.up *180);
    }


    // verifica se está tocando o chão
    private bool CheckIsGrounded()
    {
        return Physics2D.Raycast((Vector2)(transform.position) + (Vector2)(offSetGround)*currentDirection, -Vector2.up, offSetGround.z, groundLayer) ||
                Physics2D.Raycast(new Vector2(transform.position.x - offSetGround.x, transform.position.y + offSetGround.y), -Vector2.up, offSetGround.z, groundLayer) ||
                 Physics2D.Raycast((Vector2)(transform.position) + (Vector2)(offSetGround)*currentDirection, -Vector2.up, offSetGround.z, wallLayer) ||
                  Physics2D.Raycast(new Vector2(transform.position.x - offSetGround.x, transform.position.y + offSetGround.y), -Vector2.up, offSetGround.z, wallLayer);
    }

    // retorna 1 ou -1 aleatoriamente
    private int RandomDirection()
    {
        return new List<int>(){-1,1}[UnityEngine.Random.Range(0,2)];
    }


    // verifica se a posição passada está fora dos limites de movimentação
    private bool IsOutLimite(Vector3 position)
    {
        float distance = Mathf.Abs(myStartPosition.x - position.x); // guarda a distancia a partir do ponto inicial
        // print("distance: "+distance);

        if(distance >= MovementDistance)
        {
            return true;
        }
        
        return false;
    }


    // retorna a distancia de uma parede até o javali
    private float GetDistanceOfCollisionWithWall(RaycastHit2D _wall)
    {
        RaycastHit2D javaliHit = Physics2D.Raycast(new Vector2(_wall.point.x, transform.position.y), Vector2.right*currentDirection*-1, 100f, javaliLayer);

        Debug.DrawRay(new Vector2(javaliHit.point.x, transform.position.y), Vector2.right*currentDirection*javaliHit.distance, Color.white);
        return Mathf.Abs(javaliHit.point.x - _wall.point.x);
    }


    // INÚTIL
    // retorna a altura da parede
    private float GetHeightOfWall(RaycastHit2D _wall)
    {
        float width = _wall.transform.lossyScale.x;
        float height = _wall.transform.lossyScale.y;

        return height;
    }


    // retorna se há uma parede na frente do javali na distacia definida
    private bool HitWall()
    {
        RaycastHit2D wall = GetWall(); // pega a parede
        
        // checa se encontrou uma parede
        if(wall)
            return true; // encontrou

        return false; // não encontrou
    }


    // retorna a parede na frente do javali dentro da distancia definida    
    private RaycastHit2D GetWall()
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x+offSetWall.x*currentDirection, transform.position.y+offSetWall.y), Vector2.right*currentDirection, offSetWall.z, wallLayer);
        // Debug.DrawRay(new Vector2(transform.position.x+offSetWall.x*currentDirection, transform.position.y+offSetWall.y), Vector2.right*currentDirection*hit.distance, Color.black);
        return hit;
    }


    // verifica se o javali é capaz de pular o obstáculo
    private bool CanJumpWall()
    {
        //new Vector2(transform.position.x+offSetWall.x, transform.position.y + JumpHeight), Vector2.right*offSetWall.z
        Vector2 origin = new Vector2(transform.position.x+offSetWall.x*currentDirection, transform.position.y + JumpHeight); // ponto de partida do Raycast2D
        RaycastHit2D wall = Physics2D.Raycast(origin, Vector2.right*currentDirection, offSetWall.z, wallLayer); // lança o Raycast2D
        // Debug.DrawRay(origin, Vector2.right*currentDirection*distance, Color.red); // desenha o Raycast2D

        if(wall)// verifica se encontrou um obstáculo e retorna falso
            return false;
        
        return true; // retorna verdadeiro
    }


    // faz o javali pular
    private void Jump()
    {
        // verifica se o javali está no chão
        if(isGrounded)
        {
            float g = myRb.gravityScale * Physics2D.gravity.magnitude;
            // float v0 = jumpForce / myRb.mass; // converts the jumpForce to an initial velocity
            // float maxJump_y = GroundCheck1.position.y + (v0 * v0)/(2*g);
            isGrounded = false;

            RaycastHit2D GroundCheck = Physics2D.Raycast((Vector2)(transform.position), -Vector2.up, 10f, groundLayer);
            RaycastHit2D GroundCheck2 = Physics2D.Raycast((Vector2)(transform.position), -Vector2.up, 10f, wallLayer);

            if(GroundCheck.distance >= GroundCheck2.distance)
                GroundCheck = GroundCheck2;

            // calcula a força necessária para pular na altura definida
            float forceY = Mathf.Sqrt((JumpHeight+(transform.position.y-GroundCheck.point.y)*1.25f)*2*g); 

            myRb.velocity = new Vector2(myRb.velocity.x, forceY); // pula
            // myRb.AddForce(transform.up*forceY);
        }
    }


    // verifica se o javali e o player estão na mesma plataforma (talvez nem use)
    private bool CheckIfPlayerIsOnDifferentGround()
    {
        RaycastHit2D groundOfPlayer = Physics2D.Raycast((Vector2) playerTrans.position, -Vector2.up, 1000f, groundLayer);
        RaycastHit2D wallOfPlayer = Physics2D.Raycast((Vector2) playerTrans.position, -Vector2.up, 1000f, wallLayer);

        if(groundOfPlayer.distance >= wallOfPlayer.distance)
            groundOfPlayer = wallOfPlayer;

        RaycastHit2D groundOfJavali = Physics2D.Raycast((Vector2) transform.position, -Vector2.up, 1000f, groundLayer);
        RaycastHit2D wallOfJavali = Physics2D.Raycast((Vector2) transform.position, -Vector2.up, 1000f, wallLayer);

        if(groundOfJavali.distance >= wallOfJavali.distance)
            groundOfJavali = wallOfJavali;

        return groundOfPlayer.point.y != groundOfJavali.point.y;
    }


    private void Attack()
    {

    }


    private float GetXDistaceOfPlayer()
    {
        RaycastHit2D javaliHit = Physics2D.Raycast(new Vector2(playerTrans.position.x, transform.position.y), Vector2.right*currentDirection*-1, 1000f, javaliLayer);
        RaycastHit2D playerHit = Physics2D.Raycast(new Vector2(javaliHit.point.x, playerTrans.position.y), Vector2.right*currentDirection, 1000f, playerLayer);

        return playerHit.distance;
    }


    private void FlipFaceToPlayer()
    {
        if((playerTrans.position.x < transform.position.x && currentDirection == 1) ||
           (playerTrans.position.x > transform.position.x && currentDirection == -1))
        {
            InvertDirection();
        }

    }
}
