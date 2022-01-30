using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA_Javali : MonoBehaviour
{
    [Range(1,2)]
    public int AiLevel;

    [Range(1,50)]
    public float MovementDistance;
    public bool walkInAllGround; // só funciona com "AiLevel == 1"
    
    public float MovementSpeed, JumpHeight, FollowDistance, AttackDistance;
    public Animator JavaliAnimator;

    [Header("Raycast")]
    public LayerMask groundLayer;
    private LayerMask wallLayer;
    public LayerMask playerLayer;
    public LayerMask javaliLayer;
    public Vector3 offSetGround, offSetWall;


    // a linha vermelha é usada para mostrar a altura do pulo e para detectar se é possível pular o obstaculo
    // a linha roxa é para detectar obstaculos
    // a linha amarela é para detectar se há colisão com o chão
    // a linha preta é para mostrar onde é o limite da movimentação do javali


    protected Rigidbody2D myRb;
    protected Vector3 myStartPosition;
    protected int currentDirection;
    protected float auxMovement, distanceOfCollision = .3f;
    protected bool isGrounded, following, started, attacking, stuned, bug;
    protected Transform playerTrans;


    // Gizmos - Desenvolvimento
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
        StartCoroutine(AddForceIfBuged());


        wallLayer           = groundLayer;
        started             = true;
        playerTrans         = GameObject.FindGameObjectWithTag("Player").transform;
        myRb                = GetComponent<Rigidbody2D>();
        myStartPosition     = myRb.position;
        currentDirection    = RandomDirection();
        // currentDirection    = 1;
        if(currentDirection == -1)
            FlipJavali();
        // bug                 = true;
    }



    void FixedUpdate()
    {
        if (LevelManager.levelstatus == LevelManager.LevelStatus.Game) 
        {
            isGrounded = CheckIsGrounded();
            Movement(); // faz a movimentação do javali
        }
    }


    // aqui é feito todo o calculo para movimentar o javali
    private void Movement()
    {
        auxMovement = Time.fixedDeltaTime * MovementSpeed * currentDirection * 100; // calcula quanto ele deve se mover
        Vector2 newPosition = myRb.position + new Vector2(auxMovement*Time.fixedDeltaTime, 0); // salva a nova posição após o movimento

        if(!bug)
        {
            // inteligência do javali nível 1
            if(AiLevel == 1 && !attacking && !stuned)
            {
                myRb.velocity = new Vector2(auxMovement, myRb.velocity.y); // movimenta para a nova posição

                // verifica se é para andar em todo o terreno
                if(walkInAllGround)
                {
                    // verifica se está tocando o chão
                    if(CheckIsGrounded(true))
                    {
                        // verifica se encontrou algum obstaculo e faz o retorno quando estiver próximo
                        if(HitWall() && GetDistanceOfCollisionWithWall(GetWall()) < distanceOfCollision) 
                        {
                            FlipFaceToStartPosition(); // vira a face do javali para o ponto inicial
                        }
                    }
                    else
                    {
                        FlipFaceToStartPosition(); // vira a face do javali para o ponto inicial
                    }
                }
                // verifica se encontrou algum obstaculo ou está fora do limite e faz o retorno 
                else if(HitWall() && GetDistanceOfCollisionWithWall(GetWall()) < distanceOfCollision || IsOutLimite(newPosition) || !CheckIsGrounded(true)) // verifica se a nova posição está fora do limite
                {
                    FlipFaceToStartPosition(); // vira a face do javali para o ponto inicial
                }

       
                if(GetDistaceOfPlayer() < AttackDistance && !attacking && IsFaceToPlayer())
                {
                    Attack(); // ataca
                }
            }
            // inteligência do javali nível 2
            else if(AiLevel == 2 && !attacking && !stuned)
            {
                myRb.velocity = new Vector2(auxMovement, myRb.velocity.y); // movimenta para a nova posição

                // verifica se o javali consegue pular o primeiro obstaculo na direção do player
                if(CanJumpWallInDirectionOfPlayer())
                {
                    following = false; // else

                    // verifica se pode seguir ou não o player
                    if(GetDistaceOfPlayer() < FollowDistance && !HasObstacleOnWay() && GetXDistaceOfPlayer() != 0f)
                    {
                        following = true;
                        FlipFaceToPlayer(); // vira a face do javali para o lado do player
                    }
                }

                // print("following   "+following);
                // não seguindo
                if(!following)
                {
                    // volta para dentro do limite
                    if(IsOutLimite(newPosition))
                    {
                        FlipFaceToStartPosition(); // vira a face do javali para o ponto inicial
                    }

                    // encontrou uma parede
                    if(HitWall())
                    {
                        // verifica se é possível pular essa parede
                        if(CanJumpWall())
                        {
                            Jump(); // pula
                        }
                        // verifica o quão próximo está a parede e faz o retorno
                        else if(GetDistanceOfCollisionWithWall(GetWall()) < .3f)
                        {
                            FlipFaceToStartPosition(); // vira a face do javali para o ponto inicial
                        }
                    }
                }
                // está seguindo
                // encontrou uma parede
                else if(HitWall())
                {
                    // verifica se é possível pular essa parede
                    if(CanJumpWall())
                    {
                        Jump(); // pula
                        
                        if(PlayerIsOnTheEdge())
                        {
                            Attack(false);
                        }
                        else
                        {
                            //Jump(3f); // pula
                        }
                    }
                    // verifica o quão próximo está a parede e faz o retorno
                    else if(GetDistanceOfCollisionWithWall(GetWall()) < .3f)
                    {
                        following = false;
                        FlipFaceToStartPosition(); // vira a face do javali para o ponto inicial
                    }
                }


                // verifica se está próximo o bastante para atacar
                if(GetDistaceOfPlayer() < AttackDistance && !attacking && !HasObstacleOnWay())
                {
                    FlipFaceToPlayer(); // vira a face do javali para o lado do player
                    Attack(); // ataca

                }

            }
            
            if(!attacking && !stuned && isGrounded)
            {
                // print("JavaliAndando");
                ChangeAnimation("JavaliAndando");
            }
        }
        else
        {

            if(!isGrounded)
            {
                ChangeAnimation("JavaliTonto");
                myRb.AddForce(new Vector2(currentDirection*-1, 0)*2, ForceMode2D.Impulse);
            }
            else
            {
                bug = false;
            }
            // myRb.AddForce(new Vector2(currentDirection*-1, 0)*200, ForceMode2D.Impulse);
        }
    }

    protected IEnumerator AddForceIfBuged()
    {
        while (true)
        {
            Vector2 startPos = transform.position;
            float time=0;
            bool stop = false;
            while (startPos == (Vector2)transform.position && !stop && !isGrounded)
            {
                time += Time.fixedDeltaTime;
                if (time > 2)
                {
                    bug = true;
                    stop = true;
                }
                yield return null;
            }
            time = 0;
            yield return null;
        }
    }

    //muda a animação do javali
    protected void ChangeAnimation(string str, bool restart=false)
    {
        if(!restart)
        {
            JavaliAnimator.Play(str);
        }
        else
        {
            JavaliAnimator.Play(str, -1, 0);
        }

    }


    // inicia a contagem para finalizar o estado de ataque
    private IEnumerator AttackFinished(float sec=1f)
    {
        yield return new WaitForSeconds(sec);
        // Debug.Log("Attack Finished");
        attacking = false;
    }


    protected void InvertDirection()
    {
        currentDirection *= -1; // muda a direção
        FlipJavali();
    }


    // muda a direção do sprite do javali
    protected void FlipJavali()
    {
        transform.Rotate(Vector2.up *180);
    }


    // verifica se está tocando o chão
    protected bool CheckIsGrounded(bool onlyFace=false)
    {
        if(!onlyFace)
        {
            return Physics2D.Raycast(new Vector2(transform.position.x + offSetGround.x*currentDirection, transform.position.y + offSetGround.y), -Vector2.up, offSetGround.z, groundLayer).distance != 0 ||
                    Physics2D.Raycast(new Vector2(transform.position.x - offSetGround.x*currentDirection, transform.position.y + offSetGround.y), -Vector2.up, offSetGround.z, groundLayer).distance != 0 ||
                     Physics2D.Raycast(new Vector2(transform.position.x + offSetGround.x*currentDirection, transform.position.y + offSetGround.y), -Vector2.up, offSetGround.z, wallLayer).distance != 0 ||
                      Physics2D.Raycast(new Vector2(transform.position.x - offSetGround.x*currentDirection, transform.position.y + offSetGround.y), -Vector2.up, offSetGround.z, wallLayer).distance != 0;
        }
 
        return Physics2D.Raycast(new Vector2(transform.position.x + offSetGround.x*currentDirection, transform.position.y + offSetGround.y), -Vector2.up, offSetGround.z, groundLayer).distance != 0 ||
                 Physics2D.Raycast(new Vector2(transform.position.x + offSetGround.x*currentDirection, transform.position.y + offSetGround.y), -Vector2.up, offSetGround.z, wallLayer).distance != 0;
    }

    // retorna 1 ou -1 aleatoriamente
    protected int RandomDirection()
    {
        return new List<int>(){-1,1}[UnityEngine.Random.Range(0,2)];
    }


    // verifica se a posição passada está fora dos limites de movimentação
    protected bool IsOutLimite(Vector3 position)
    {
        float distance = Mathf.Abs(myStartPosition.x - position.x); // guarda a distancia a partir do ponto inicial

        if(distance >= MovementDistance)
        {
            return true;
        }
        
        return false;
    }


    // retorna a distancia de uma parede até o javali
    protected float GetDistanceOfCollisionWithWall(RaycastHit2D _wall)
    {
        RaycastHit2D javaliHit = Physics2D.Raycast(new Vector2(_wall.point.x, transform.position.y), Vector2.right*currentDirection*-1, 100f, javaliLayer);

        Debug.DrawRay(new Vector2(javaliHit.point.x, transform.position.y), Vector2.right*currentDirection*javaliHit.distance, Color.white);
        return Mathf.Abs(javaliHit.point.x - _wall.point.x);
    }


    // INÚTIL
    // retorna a altura da parede
    protected float GetHeightOfWall(RaycastHit2D _wall)
    {
        float width = _wall.transform.lossyScale.x;
        float height = _wall.transform.lossyScale.y;

        return height;
    }


    // retorna se há uma parede na frente do javali na distacia definida
    protected bool HitWall()
    {
        RaycastHit2D wall = GetWall(); // pega a parede
        
        // checa se encontrou uma parede
        if(wall)
            return true; // encontrou

        return false; // não encontrou
    }


    // retorna a parede na frente do javali dentro da distancia definida    
    protected RaycastHit2D GetWall()
    {
        return Physics2D.Raycast(new Vector2(transform.position.x+offSetWall.x*currentDirection, transform.position.y+offSetWall.y), Vector2.right*currentDirection, offSetWall.z, wallLayer);
    }


    // verifica se o javali é capaz de pular o obstáculo na sua frente
    protected bool CanJumpWall()
    {
        Vector2 origin = new Vector2(transform.position.x+offSetWall.x*currentDirection, transform.position.y + JumpHeight); // ponto de partida do Raycast2D
        RaycastHit2D wall = Physics2D.Raycast(origin, Vector2.right*currentDirection, offSetWall.z, wallLayer); // lança o Raycast2D

        return wall.distance == 0;
    }


    // verifica se o javali é capaz de pular o obstáculo na direção do player
    protected bool CanJumpWallInDirectionOfPlayer()
    {
        Vector2 origin = new Vector2(transform.position.x+offSetWall.x*currentDirection, transform.position.y + offSetWall.y); // ponto de partida do Raycast2D
        RaycastHit2D wall = Physics2D.Raycast(origin, Vector2.right*GetDirectionOfPlayer(), GetXDistaceOfPlayer(), wallLayer); // lança o Raycast2D
        origin = new Vector2(transform.position.x+offSetWall.x*currentDirection, transform.position.y + JumpHeight); // ponto de partida do Raycast2D
        wall = Physics2D.Raycast(origin, Vector2.right*GetDirectionOfPlayer(), wall.distance+.2f, wallLayer); // lança o Raycast2D

        if(wall)// verifica se encontrou um obstáculo e retorna falso
            return false;
        
        return true; // retorna verdadeiro
    }


    // faz o javali pular
    protected void Jump(float forceMultiply=1f)
    {
        // verifica se o javali está no chão
        if(isGrounded)
        {
            // print("Jump");
            ChangeAnimation("JavaliParado", true);
            float g = myRb.gravityScale * Physics2D.gravity.magnitude;
            // float v0 = jumpForce / myRb.mass; // converts the jumpForce to an initial velocity
            // float maxJump_y = GroundCheck1.position.y + (v0 * v0)/(2*g);
            isGrounded = false;

            RaycastHit2D GroundCheck = Physics2D.Raycast((Vector2)(transform.position), -Vector2.up, 10f, groundLayer);
            RaycastHit2D GroundCheck2 = Physics2D.Raycast((Vector2)(transform.position), -Vector2.up, 10f, wallLayer);

            if(GroundCheck.distance >= GroundCheck2.distance)
                GroundCheck = GroundCheck2;

            // calcula a força necessária para pular na altura definida
            float forceY = Mathf.Sqrt(((JumpHeight*forceMultiply)+(transform.position.y-GroundCheck.point.y)*1.25f)*2*g); 

            myRb.velocity = new Vector2(myRb.velocity.x, forceY); // pula
            // myRb.AddForce(transform.up*forceY);
        }
    }


    // protected RaycastHit2D GetWallInDirectionOfPlayer()
    // {
    //     return Physics2D.Raycast((Vector2)(transform.position), Vector2.right*GetDirectionOfPlayer(), GetXDistaceOfPlayer()+1, wallLayer);
    // }


    // verifica se o javali e o player estão na mesma plataforma (talvez nem use)
    protected bool CheckIfPlayerIsOnDifferentGround()
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


    //inicia o ataque
    protected void Attack(bool stopToAttack=true)
    {
        if(!stuned)
        {
            attacking = true;
            // print("Attack");
            ChangeAnimation("JavaliAtacando", true);
            StartCoroutine(AttackFinished());
            if(stopToAttack)
            {
                myRb.velocity = Vector3.zero;
            }
        }
    }


    // retorna a distacia do player no eixo X
    protected float GetXDistaceOfPlayer()
    {
        RaycastHit2D javaliHit = Physics2D.Raycast(new Vector2(playerTrans.position.x, transform.position.y), Vector2.right*GetDirectionOfPlayer()*-1, Mathf.Infinity, javaliLayer);
        RaycastHit2D playerHit = Physics2D.Raycast(new Vector2(javaliHit.point.x, playerTrans.position.y), Vector2.right*GetDirectionOfPlayer(), Mathf.Infinity, playerLayer);

        return playerHit.distance;
    }


    // retorna a magnitude da distancia entre o player e o javali
    protected float GetDistaceOfPlayer()
    {
        Vector2 dir = (transform.position - playerTrans.position).normalized; // direção

        RaycastHit2D[] javaliHits = Physics2D.RaycastAll((Vector2)playerTrans.position, dir, Mathf.Infinity, javaliLayer);
        RaycastHit2D javaliHit = default(RaycastHit2D);

        foreach(RaycastHit2D hit in javaliHits)
        {
            if(hit.transform == this.transform)
            {
                javaliHit = hit;
            }
        }
        
        RaycastHit2D playerHit = Physics2D.Raycast((Vector2)javaliHit.point, dir*-1, Mathf.Infinity, playerLayer);

        return Vector2.Distance(javaliHit.point, playerHit.point);
    }


    // retorna se o javali está virado para o lado do player    
    protected bool IsFaceToPlayer()
    {
        return GetDirectionOfPlayer() == currentDirection;
    }


    protected void FlipFaceToPlayer(bool onlyIfGrounded=true)
    {
        if(GetDirectionOfPlayer() != currentDirection)
        {
            if(onlyIfGrounded)
            {
                if(CheckIsGrounded())
                {
                    InvertDirection();
                }
            }
            else
            {
                InvertDirection();
            }
        }
    }


    protected void FlipFaceToStartPosition(bool onlyIfGrounded=true)
    {
        if((myStartPosition.x < transform.position.x && currentDirection == 1) ||
           (myStartPosition.x > transform.position.x && currentDirection == -1))
        {
            if(onlyIfGrounded)
            {
                if(CheckIsGrounded())
                {
                    InvertDirection();
                }
            }
            else
            {
                InvertDirection();
            }
        }
    }

    protected int GetDirectionOfPlayer()
    {
        if(playerTrans.position.x < transform.position.x)
        {
            return -1;
        }
        else if(playerTrans.position.x > transform.position.x)
        {
            return 1;
        }

        return 0;
    }


    // verifica se há alguma plataforma (ground) entre o player e o javali
    protected bool HasObstacleOnWay()
    {
        float distance  = Vector2.Distance(playerTrans.position, transform.position);
        Vector2 dir     = (playerTrans.position - transform.position); 
        RaycastHit2D obstacle = Physics2D.Raycast(transform.position, dir.normalized, distance, groundLayer);
        Debug.DrawRay((Vector2)transform.position, dir, Color.white);

        return obstacle.distance != 0;
    }


    // verifica se o player está a beirada de uma plataforma
    protected bool PlayerIsOnTheEdge()
    {
        Vector2 origin = new Vector2(transform.position.x+offSetWall.x*currentDirection, transform.position.y + JumpHeight); // ponto de partida do Raycast2D
        RaycastHit2D player = Physics2D.Raycast(origin, Vector2.right*currentDirection, offSetWall.z*1.5f, playerLayer); // lança o Raycast2D
        return player.distance != 0;        
    }

    protected void JavaliStuned()
    {
        if(!stuned)
        {
            StartCoroutine(_JavaliStuned());
        }
    }

    protected IEnumerator _JavaliStuned()
    {
        if(attacking)
        {
            yield return new WaitForSeconds(0.3f);
            ChangeAnimation("JavaliTonto", true);
            stuned = true;
            attacking = false;
            yield return new WaitForSeconds(2.2f);
        }
        else
        {
            yield return new WaitForSeconds(2.5f);
        }
        stuned = false;
    }

    // verifica se há um precipício na direção do player (ainda vou desenvolver)
    protected bool HaveACliff()
    {
        return false;
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player" && !stuned && attacking)
        {
            Movement playerMovement = col.GetComponent<Movement>();
            if (!playerMovement.defendendo)
            {
                playerMovement.Life -= 1;
            }
            else
            {
                JavaliStuned();
                Debug.Log("Jogador não recebeu dano");
            }
        }

    }
}
