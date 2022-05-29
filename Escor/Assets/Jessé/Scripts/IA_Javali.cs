using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//OBS: se o javali for cair de alguma superficie, é melhor usar 'walkInAllGround' para não acontecer nenhum BUG (resolvido na linha 'BUG1')


public class IA_Javali : MonoBehaviour
{
    [Range(1,2)]
    public int AiLevel;

    // [0] Não se move e ataca quando perto (removi esse nivel)
    // [1] Se move e ataca quando o alvo está logo a frente (não se vira caso o alvo esteja atrás)
    // [2] Se move e é capaz de seguir o alvo (não pula e nem desce de plataformas)

    public bool Move;
    public bool activateAttack = true;
    [Range(-1,1)]
    public int startMovementDirection; // -1 = left, 0 = RandomDirection, 1 = right

    [Range(1,50)]
    public float MovementDistance;
    public bool walkInAllGround; // só funciona com "AiLevel == 1" (OBS: nem sei mais)

    public float MovementSpeed, FollowDistance, AttackDistance;
    private float JumpHeight;
    public Animator JavaliAnimator;
    public int attacksReceived=0;

    [Header("Raycast")]
    public LayerMask groundLayer;
    public LayerMask platformLayer;
    private LayerMask wallLayer;
    public LayerMask playerLayer;
    public LayerMask javaliLayer;
    public Vector3 offSetGround, offSetWall;
    public Animator exclamation;


    // a linha vermelha é usada para mostrar a altura do pulo e para detectar se é possível pular o obstaculo
    // a linha roxa é para detectar obstaculos
    // a linha amarela é para detectar se há colisão com o chão
    // a linha preta é para mostrar onde é o limite da movimentação do javali


    protected Rigidbody2D myRb;
    protected Vector3 myStartPosition;
    protected int currentDirection;
    protected float auxMovement, distanceOfCollision = .3f;
    protected bool isGrounded, following, started, attacking, stuned, bug, canAttack, firstContactWithPlayer; // firstContactWithPlayer reseta quando o player sai da area de visão
    protected Transform playerTrans;



#if UNITY_EDITOR
    // Gizmos - Desenvolvimento
    void OnDrawGizmos()
    {
        GUIStyle style = new GUIStyle();

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay((Vector2)(transform.position) + (Vector2)(offSetGround), -Vector2.up*offSetGround.z);
        Gizmos.DrawRay(new Vector2(transform.position.x - offSetGround.x, transform.position.y + offSetGround.y), -Vector2.up*offSetGround.z);

        style.normal.textColor = Color.yellow;
        Handles.Label((Vector2)(transform.position)+new Vector2(offSetGround.x, offSetGround.y - offSetGround.z) , "Detector do chão", style);
        // Handles.Label((Vector2)(transform.position)+new Vector2(offSetGround.x+offSetWall.z/2, offSetWall.y) , "Detector de obstaculos", style);

        // Gizmos.color = Color.red;
        // Gizmos.DrawRay(new Vector2(transform.position.x+offSetWall.x, transform.position.y + JumpHeight), Vector2.right*offSetWall.z);
        // Gizmos.DrawRay(new Vector2(transform.position.x-offSetWall.x, transform.position.y + JumpHeight), -Vector2.right*offSetWall.z);

        Gizmos.color = Color.magenta;
        Gizmos.DrawRay((Vector2)(transform.position+offSetWall), Vector2.right*offSetWall.z);
        Gizmos.DrawRay(new Vector2(transform.position.x - offSetWall.x, transform.position.y + offSetWall.y), -Vector2.right*offSetWall.z);

        style.normal.textColor = Color.magenta;
        Handles.Label((Vector2)(transform.position)+new Vector2(offSetWall.x+offSetWall.z, offSetWall.y) , "Detector de obstáculos", style);

        if(Move)
        {
            style.normal.textColor = Color.green;
            Gizmos.color = Color.green;
            if(started)
            {
                Gizmos.DrawRay(new Vector2(myStartPosition.x - MovementDistance, myStartPosition.y+0.5f), -Vector2.up);
                Gizmos.DrawRay(new Vector2(myStartPosition.x + MovementDistance, myStartPosition.y+0.5f), -Vector2.up);
                Handles.Label(new Vector2(myStartPosition.x - MovementDistance, myStartPosition.y+0.5f) , " Limite de movimento", style);
            }
            else
            {
                Gizmos.DrawRay(new Vector2(transform.position.x - MovementDistance, transform.position.y+0.5f), -Vector2.up);
                Gizmos.DrawRay(new Vector2(transform.position.x + MovementDistance, transform.position.y+0.5f), -Vector2.up);
                Handles.Label(new Vector2(transform.position.x - MovementDistance, transform.position.y+0.5f), " Limite de movimento", style);
            }
        }

        DrawArea(Color.red);
        DrawArea(Color.white, false);

        style.normal.textColor = Color.red;
        Handles.Label((Vector2) transform.position + Vector2.right * AttackDistance , " Attack Distance", style);
        style.normal.textColor = Color.white;
        Handles.Label((Vector2) transform.position - Vector2.right * FollowDistance , " Follow Distance", style);
    }

#endif
    void DrawArea(Color color, bool ofAttack=true)
    {
        float dis = AttackDistance;

        if(!ofAttack)
            dis = FollowDistance;

        Vector2 startDirection = GetDirectionOfAngle(0, dis);
        Gizmos.color = color;

        for (int angle=1; angle<=360; angle++)
        {
            Vector2 endDirection = GetDirectionOfAngle(angle, dis);
            // Debug.DrawLine((Vector2) transform.position + startDirection, (Vector2) transform.position + endDirection, color);
            Gizmos.DrawLine((Vector2) transform.position + startDirection, (Vector2) transform.position + endDirection);
            startDirection = endDirection;
        }
    }


    float GetAngleOfDirection(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x);
        float degrees = 180*angle/Mathf.PI;
        // return degrees;
        return (360+Mathf.Round(degrees))%360;
    }


    Vector2 GetDirectionOfAngle(float angle, float radius)
    {
        // var radius = 60;
        // var angle  = 140;
        float x = radius * Mathf.Sin(Mathf.PI * 2 * angle / 360);
        float y = radius * Mathf.Cos(Mathf.PI * 2 * angle / 360);
        return new Vector2(x, y);
    }

    // ----------------------

    public void Start()
    {
        // print("Start from IA_Javali");

        // StartCoroutine(AddForceIfBuged());

        wallLayer           = groundLayer;
        started             = true;
        // print(started);

        playerTrans         = GameObject.FindGameObjectWithTag("Player").transform;
        myRb                = GetComponent<Rigidbody2D>();
        myStartPosition     = myRb.position;
        currentDirection    = startMovementDirection != 0 ? startMovementDirection : RandomDirection();
        // currentDirection    = 1;
        if(currentDirection == -1)
            FlipJavali();
        // bug                 = true;
    }


    // aqui é feito todo o calculo para movimentar o javali
    protected void Movement()
    {
        if(!CheckIsGrounded(false))
        {
            walkInAllGround = true; // [BUG1] gambiarra pra resolver BUG rsrs
            myRb.velocity = new Vector2(0, myRb.velocity.y); // [BUG1]
            ChangeAnimation("JavaliParado");
            // print("_> aqui 1");
            return;
        }

        if(!Move)
        {
            if(!CloseToAttack() && !attacking && !stuned)
            {
                // print("_> aqui 2");
                ChangeAnimation("JavaliParado2");
            }

            // print("_> aqui 3");
            return;
        }

        // print("_> aqui 4 :"+CloseToAttack());
        auxMovement = Time.fixedDeltaTime * MovementSpeed * currentDirection * 100; // calcula quanto ele deve se mover
        Vector2 newPosition = myRb.position + new Vector2(auxMovement, 0); // salva a nova posição após o movimento
        Vector2 newPosition2 = myRb.position + new Vector2(auxMovement*0.5f, 0); // nova posição porém com um offset um pouco mais a frente (Não sei pq funciona, mas funciona)

        if (!bug)
        {
            // verifica se é para andar em todo o terreno
            // if(walkInAllGround)
            // {
            //     // verifica se está tocando o chão
            //     if(CheckIsGrounded(true))
            //     {
            //         // verifica se encontrou algum obstaculo e faz o retorno quando estiver próximo
            //         if(HitWall() && GetDistanceOfCollisionWithWall(GetWall()) < distanceOfCollision)
            //         {
            //             FlipFaceToStartPosition(); // vira a face do javali para o ponto inicial
            //         }
            //     }
            //     else
            //     {
            //         FlipFaceToStartPosition(); // vira a face do javali para o ponto inicial
            //     }
            // }
            if (attacking || stuned || CloseToAttack())
            {
                // não faça nada
                // print("_> aqui 4.1");
                // print("_> attacking: "+attacking);
                // print("_> stuned: "+stuned);
            }
            else if (AiLevel==0)
            {
                // print("_> aqui 4.2");
                // O Javali não anda, fica somente no mesmo lugar esperando a hora de atacar
                return;
            }
            // inteligência do javali nível 1
            else if (AiLevel == 1)
            {
                // print("_> aqui 4.3");
                if(Move)
                {
                    // print("_> aqui 5");
                    myRb.velocity = new Vector2(auxMovement, myRb.velocity.y); // movimenta para a nova posição
                }
                // myRb.velocity = new Vector2(auxMovement, myRb.velocity.y); // movimenta para a nova posição


                // verifica se encontrou algum obstaculo ou está fora do limite e faz o retorno
                //                false                                                            || (true                     && true            ) || false
                // print("_> IsOutLimite(newPosition2): "+IsOutLimite(newPosition2));
                // print("_> aqui 6");
                if ((HitWall() && GetDistanceOfCollisionWithWall(GetWall()) < distanceOfCollision) || (IsOutLimite(newPosition2) && !walkInAllGround) || !CheckIsGrounded(true)) // verifica se a nova posição está fora do limite
                {
                    // print("_> aqui 7");
                    // print("_> HitWall(): "+HitWall());
                    // print("_> GetDistanceOfCollisionWithWall(GetWall())< distanceOfCollision: "+(GetDistanceOfCollisionWithWall(GetWall())< distanceOfCollision));
                    // print("_> IsOutLimite(newPosition2):  "+IsOutLimite(newPosition2));
                    // print("_> !walkInAllGround: "+!walkInAllGround);
                    // print("_> !CheckIsGrounded(true): "+!CheckIsGrounded(true));
                    // print("_> CheckIsGrounded(false): "+CheckIsGrounded(false));
                    if(CheckIsGrounded(false))
                    {
                        // print("_> aqui 8");
                        InvertDirection();
                    }
                    // else
                    // {
                        // FlipFaceToStartPosition(); // vira a face do javali para o ponto inicial
                    // }
                }


                // if(GetDistaceOfPlayer() < AttackDistance && !attacking && IsFaceToPlayer())
                // if(CloseToAttack())
                // {
                //     // print("Pode atacar sim");
                //     Attack(); // ataca
                //     // canAttack = true;
                // }
            }
            // inteligência do javali nível 2
            else if (AiLevel == 2)
            {
                // print("_> aqui 4.4");
                if(Move)
                {
                    myRb.velocity = new Vector2(auxMovement, myRb.velocity.y); // movimenta para a nova posição
                }

                following = false;

                if(CheckIsGrounded(true))
                {
                    // verifica se pode seguir ou não o player
                    if(PlayerInsideArea() && !HasObstacleOnWay() && GetXDistaceOfPlayer() != 0f && !CheckIfPlayerIsOnDifferentGround())
                    {
                        following = true;
                        FlipFaceToPlayer(); // vira a face do javali para o lado do player
                    }
                }
                else
                {
                    following = false;
                    FlipFaceToStartPosition();
                }

                // não seguindo
                if(!following)
                {
                    // volta para dentro do limite
                    if((IsOutLimite(newPosition) && !walkInAllGround) || !CheckIsGrounded(true))
                    {
                        FlipFaceToStartPosition(); // vira a face do javali para o ponto inicial
                    }

                    // encontrou uma parede
                    if(HitWall())
                    {
                        // verifica se é possível pular essa parede
                        // if(CanJumpWall())
                        if(false)
                        {
                            Jump(); // pula
                        }
                        // verifica o quão próximo está a parede e faz o retorno
                        else if(GetDistanceOfCollisionWithWall(GetWall()) < .35f)
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
                    // if(CanJumpWall())
                    if(false)
                    {
                        Jump(); // pula

                        if(PlayerIsOnTheEdge())
                        {
                            Attack();
                            // Attack(false);
                            // canAttack = true;
                        }
                        else
                        {
                            //Jump(3f); // pula
                        }
                    }
                    // verifica o quão próximo está a parede e faz o retorno
                    else if(GetDistanceOfCollisionWithWall(GetWall()) < .35f || HaveACliff())
                    {
                        following = false;
                        FlipFaceToStartPosition(); // vira a face do javali para o ponto inicial
                    }
                }


                // verifica se está próximo o bastante para atacar
                // if(GetDistaceOfPlayer() < AttackDistance && !attacking && !HasObstacleOnWay() && !HaveACliff() && !CheckIfPlayerIsOnDifferentGround())
                // {
                //     FlipFaceToPlayer(); // vira a face do javali para o lado do player
                //     // Attack(); // ataca
                //     canAttack = true;
                //
                //
                // }
            }


            if (!attacking && !stuned && isGrounded)
            {
                // print("_> aqui 4.5");
                if(!CloseToAttack())
                {
                    // print("_> aqui 4.6");
                    ChangeAnimation("JavaliAndando");
                }
                else if(myRb.velocity != Vector2.zero)
                {
                    // print("_> aqui 4.7");
                    myRb.velocity = Vector3.zero;
                    ChangeAnimation("JavaliParado2");
                }
                    // SfxManager.PlaySound(SfxManager.Sound.javaliMove);
            }
        }
        else
        {

            // print("_> aqui 4.8");
            if(!isGrounded)
            {
                // print("_> aqui 4.9");
                ChangeAnimation("JavaliTonto");
                SfxManager.PlaySound(SfxManager.Sound.javaliStuned);
                myRb.AddForce(new Vector2(currentDirection*-1, 0)*2, ForceMode2D.Impulse);
            }
            else
            {
                // print("_> aqui 5.5");
                bug = false;
            }
            // myRb.AddForce(new Vector2(currentDirection*-1, 0)*200, ForceMode2D.Impulse);
        }
    }


    protected virtual bool CloseToAttack()
    {
        if(!PlayerInsideArea(true) || !activateAttack)
            return false;

        return IsFaceToPlayer()
               && !HaveObstacle(playerTrans.position, transform.position)
               && !HaveACliff()
               && !CheckIfPlayerIsOnDifferentGround();
    }


    // mostra uma exclamação na cabeça do javali
    protected void ShowExclamation(bool rangeDistaceIsAttack=false)
    {
        // firstContactWithPlayer = false;

        if(!PlayerIsCloseWithoutObstacle(rangeDistaceIsAttack)) // fora do alcance
        {
            firstContactWithPlayer = true;
        }
        else if(!IsFaceToPlayer())
        {
            firstContactWithPlayer = true;
        }
        else if(firstContactWithPlayer)
        {
            firstContactWithPlayer = false;
            exclamation.Play("exclamacao2", -1, 0); // mostra uma exclamação
        }

    }


    //inicia o ataque
    protected virtual void Attack()
    {
        if(!CloseToAttack() || attacking || stuned || !Move || !activateAttack)
            return;

        attacking = true;
        stuned = false;
        // ShowExclamation();
        // StopAllCoroutines();
        StartCoroutine(AttackFinished(2f));
        FlipFaceToPlayer();
        ChangeAnimation("JavaliAtacando", true);
        
        myRb.velocity = Vector3.zero;
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
    protected IEnumerator AttackFinished(float sec=1f)
    {
        // yield return new WaitForSeconds(sec);
        yield return new WaitUntil(() => (JavaliAnimator.GetCurrentAnimatorStateInfo(0).IsName("JavaliAtacando") ||
                                            JavaliAnimator.GetCurrentAnimatorStateInfo(0).IsName("JavaliAtacando2"))); // espera a animação mudar para 'JavaliAtacando' ou 'JavaliAtacando2'
        yield return new WaitUntil(() => (JavaliAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f)); // espera a animação chegar na metade
        if(!stuned)
        {
            SfxManager.PlaySound(SfxManager.Sound.javaliAttack); // som de ataque
            yield return new WaitUntil(() => (JavaliAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)); // espera a animação chegar no final
            
        }

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
        myRb.velocity = Vector2.zero;
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

        if(Physics2D.Raycast(new Vector2(transform.position.x + offSetGround.x*currentDirection, transform.position.y + offSetGround.y), -Vector2.up, offSetGround.z, platformLayer).distance != 0)
            return false;

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
        // print("_> position: "+position);
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

        Debug.DrawRay(new Vector2(javaliHit.point.x, transform.position.y), Vector2.right*currentDirection*javaliHit.distance, Color.red);
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
        RaycastHit2D groundOfJavali = Physics2D.Raycast((Vector2) transform.position, -Vector2.up, 1000f, groundLayer);

        return Mathf.Abs(groundOfPlayer.point.y - groundOfJavali.point.y) > .0001f;

        // ------------------------------


        // RaycastHit2D groundOfPlayer = Physics2D.Raycast((Vector2) playerTrans.position, -Vector2.up, 1000f, groundLayer);
        // RaycastHit2D wallOfPlayer = Physics2D.Raycast((Vector2) playerTrans.position, -Vector2.up, 1000f, wallLayer);

        // if(groundOfPlayer.distance >= wallOfPlayer.distance)
        //     groundOfPlayer = wallOfPlayer;

        // RaycastHit2D groundOfJavali = Physics2D.Raycast((Vector2) transform.position, -Vector2.up, 1000f, groundLayer);
        // RaycastHit2D wallOfJavali = Physics2D.Raycast((Vector2) transform.position, -Vector2.up, 1000f, wallLayer);

        // if(groundOfJavali.distance >= wallOfJavali.distance)
        //     groundOfJavali = wallOfJavali;

        // return groundOfPlayer.point.y != groundOfJavali.point.y;
    }



    // retorna a distacia do player no eixo X
    protected float GetXDistaceOfPlayer()
    {
        RaycastHit2D javaliHit = Physics2D.Raycast(new Vector2(playerTrans.position.x, transform.position.y), Vector2.right*GetDirectionOfPlayer()*-1, Mathf.Infinity, javaliLayer);
        RaycastHit2D playerHit = Physics2D.Raycast(new Vector2(javaliHit.point.x, playerTrans.position.y), Vector2.right*GetDirectionOfPlayer(), Mathf.Infinity, playerLayer);
        // Debug.DrawLine(new Vector2(playerTrans.position.x, transform.position.y), new Vector2(playerTrans.position.x+(Vector2.right*GetDirectionOfPlayer()*-1).x, transform.position.y), Color.red);
        // Debug.DrawLine(new Vector2(javaliHit.point.x, playerTrans.position.y),    new Vector2(javaliHit.point.x+(Vector2.right*GetDirectionOfPlayer()).x, playerTrans.position.y), Color.blue);

        return playerHit.distance;
    }


    // retorna a magnitude da distancia entre o player e o javali
    protected float GetDistaceOfPlayer(Vector2 origin)
    {
        Vector2 dir = (origin - (Vector2) playerTrans.position).normalized; // direção

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

    // se está a direita ou esquerda
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


    public void JavaliStuned()
    {
        if(stuned)
            return;

        attacksReceived++;

        StartCoroutine(_JavaliStuned());
    }


    protected IEnumerator _JavaliStuned()
    {
        // if(attacking)
        // {
        //     yield return new WaitForSeconds(0.3f);
        //     ChangeAnimation("JavaliTonto", true);
        //     stuned = true;
        //     attacking = false;
        //     // yield return new WaitForSeconds(2.2f);
        //     yield return new WaitUntil(() => (JavaliAnimator.GetCurrentAnimatorStateInfo(0).IsName("JavaliTonto"))); // espera a animação mudar para 'PortaoAbrindo'
        //     yield return new WaitUntil(() => (JavaliAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)); // espera a animação chegar no final
        //
        // }
        // else
        // {
            // yield return new WaitForSeconds(2.5f);
        // Move = false;
        stuned = true;
            // yield return new WaitForSeconds(0.3f);
            ChangeAnimation("JavaliTonto", true);
            yield return new WaitUntil(() => (JavaliAnimator.GetCurrentAnimatorStateInfo(0).IsName("JavaliTonto"))); // espera a animação mudar para 'PortaoAbrindo'
            yield return new WaitUntil(() => (JavaliAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)); // espera a animação chegar no final

        // }

        ChangeAnimation("JavaliParado2");
        // Move = true;
        attacking = false;
        stuned = false;
    }


    // verifica se há um precipício na direção do player (ainda vou desenvolver)
    protected bool HaveACliff()
    {
        return !CheckIsGrounded(true);
    }


    // Verifica se existe algum obstáculo (ground, platform) entre dois pontos
    protected bool HaveObstacle(Vector2 point_a, Vector2 point_b)
    {
        Vector2 dir                 = point_a - point_b; // direção
        float distance              = dir.magnitude;
        RaycastHit2D[] obstacles    = Physics2D.RaycastAll(point_b, dir.normalized, distance); // todos os obstáculos entre os dois pontos

        foreach(RaycastHit2D r in obstacles)
        {
            if(groundLayer == (groundLayer | ( 1 << r.collider.gameObject.layer)))
                return true;
        }

        return false;
    }


    // Verificar se o player está detro da distancia do javali, seja de ataque ou não
    protected bool PlayerIsNear(bool useAttackArea=false)
    {
        // OBS: existe dois tipo de área, a de ATAQUE e a que o javali ENXERGA o player

        float rangeDistance = FollowDistance; // é a distancia que o javali consegue ENXERGAR o player

        if(useAttackArea)
            rangeDistance = AttackDistance;  // é a distancia que o javali consegue ATACAR o player

        return GetDistaceOfPlayer(transform.position) < rangeDistance; // verifica se está dentro área ou não
    }


    // Origin = transform do javali
    // End    = primeito ponto de colisão com o player
    protected bool PlayerInsideArea(bool useAttackArea=false)
    {
        float rangeDistance = FollowDistance; // é a distancia que o javali consegue ENXERGAR o player

        if(useAttackArea)
            rangeDistance = AttackDistance;  // é a distancia que o javali consegue ATACAR o player

        Vector2 dir            = (Vector2)(playerTrans.position-transform.position);
        RaycastHit2D playerHit = Physics2D.Raycast((Vector2)transform.position, dir, Mathf.Infinity, playerLayer);

        return (playerHit.distance <= rangeDistance);
    }


    // verifica se o player está perto sem que exista obstáculo
    protected bool PlayerIsCloseWithoutObstacle(bool useAttackArea=false)
    {
        return PlayerInsideArea(useAttackArea) && !HasObstacleOnWay();
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player" && !stuned && attacking)
        {
            Movement playerMovement = col.GetComponent<Movement>();
            if (!playerMovement.defendendo)
            {
                if (!playerMovement.isInvunerable)
                {
                    playerMovement.Life -= 1;
                }

            }
            else
            {
                JavaliStuned();
                // SfxManager.PlaySound(SfxManager.Sound.playerDefense);
                Debug.Log("Jogador não recebeu dano");
            }
        }

    }
}
