using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlataform : MonoBehaviour
{

    public bool MoveOnHorizontal;
    public float speed, maxDistance, waitTimeOnEnd;
    
    public float startDirection; // 1 = cima/direita, -1 = baixo/esquerda

    private float movementAux, currentMovement, waitOnEndAux;
    private bool started, canMove=true;

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
            Gizmos.DrawRay((Vector2)(startPos) + (Vector2)(transform.right*_startDirection*maxDistance) - Vector2.up, Vector2.up*2);
        }
        else
        {
            Gizmos.DrawRay((Vector2)(startPos) + (Vector2)(transform.up*_startDirection*maxDistance) - Vector2.right, Vector2.right*2);
        }

    }


    // Start is called before the first frame update
    void Start()
    {
        started = true;
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

        maxDistance = Mathf.Abs(maxDistance);
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove)
        {
            currentMovement = Time.deltaTime * speed;
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
                canMove = true;
            }
        }
    }
}
