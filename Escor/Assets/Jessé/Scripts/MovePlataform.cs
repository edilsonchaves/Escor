using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlataform : MonoBehaviour
{
    public AbovePlatformManager abovePlatformManager;

    public bool waitPlayerToStartMovement=false, stopMovementOnTheEnd=false, MoveOnHorizontal;
    public float movementTime, maxDistance, waitTimeOnEnd;
    
    public float startDirection; // 1 = cima/direita, -1 = baixo/esquerda

    private float movementAux, currentMovement, waitOnEndAux;
    private bool started, canMove=true, startMovement;

    Vector3 newPos, startPos;
    float _startDirection;


    // Gizmos - Desenvolvimento
    void OnDrawGizmos()
    {

        Gizmos.color = Color.green;   
        
        if(!started)
        {
            startPos = transform.position;  
            _startDirection = startDirection;
        }
        
        if(MoveOnHorizontal)
        {
            Gizmos.DrawRay((Vector2)(startPos) + (Vector2)(transform.right*(_startDirection/Mathf.Abs(startDirection))*maxDistance) - Vector2.up, Vector2.up*2);
        }
        else
        {
            Gizmos.DrawRay((Vector2)(startPos) + (Vector2)(transform.up*(_startDirection/Mathf.Abs(startDirection))*maxDistance) - Vector2.right, Vector2.right*2);
        }

    }


    // Start is called before the first frame update
    void Start()
    {
        started = true;
        startMovement = !waitPlayerToStartMovement;
        startPos = transform.position;
        _startDirection = startDirection;
        if(startDirection == 0)
        {
            startDirection = 1;
        }
        else
        {
            startDirection /= Mathf.Abs(startDirection);
        }

        if(movementTime <= 0)
        {
            movementTime = 1;
        }

        maxDistance = Mathf.Abs(maxDistance);
    }

    // Update is called once per frame
    void Update()
    {
        if(!startMovement && abovePlatformManager.isAbove)
        {
            startMovement = true;
        }
        
        if(startMovement)
        {

            if(canMove)
            {
                currentMovement = maxDistance * Time.deltaTime / movementTime;
                movementAux += currentMovement;

                if(movementAux > maxDistance)
                    currentMovement = maxDistance-(movementAux-currentMovement);

                newPos = (MoveOnHorizontal?transform.right:transform.up)*currentMovement*startDirection;
                transform.position += newPos;

                if(movementAux >= maxDistance)
                {
                    canMove = false;
                    startDirection *= -1;
                    movementAux = 0;
                }

            }
            else
            {
                waitOnEndAux += Time.deltaTime;
                if(waitOnEndAux >= waitTimeOnEnd)
                {
                    waitOnEndAux = 0;
                    canMove = !stopMovementOnTheEnd;
                    startMovement = canMove;
                }

            }

        }
    
    }


    // void OnCollisionEnter2D (Collision2D col)
    // {
    //     if(col.tag == "Player" && col.contactPoints[0].normal == Vector3.up)
    //     {
    //         col.transform.SetParent(transform);
    //         print("startMovement");
    //         startMovement = true; 
    //     }
    // }

 // void OnCollisionEnter2D(Collision2D collision) {
 //        // print(collision.contacts[0].normal);
 //     if (collision.gameObject.tag == "Player" && collision.contacts[0].normal == -Vector2.up) {
 //         // For a box collider aligned with the world axes, this was hit on top
 //        // print("Enter");
 //     }
 // }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            col.transform.SetParent(null);
        }
    }
}
