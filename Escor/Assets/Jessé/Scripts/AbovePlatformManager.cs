using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbovePlatformManager : MonoBehaviour
{
    [SerializeField]
    public bool isAbove;
    // private Movement mvt;
    private Rigidbody2D objAboveRigidbody;
    // private MovePlataform movePlatform;

    Vector2 myVelocity;

    void Start()
    {
        // TryGetComponent(out MovePlataform movePlatform);
    }


    void OnCollisionEnter2D(Collision2D collision) {
        
        if (collision.gameObject.tag == "Player" && Mathf.Round(collision.contacts[0].normal.y) == Mathf.Round(-Vector2.up.y)) 
        {
            collision.transform.SetParent(transform);
            isAbove = true;
        }

    }


    void OnCollisionExit2D(Collision2D collision) {
        
        if (collision.gameObject.tag == "Player" && isAbove) 
        {
            if(collision.gameObject.TryGetComponent(out Rigidbody2D objAboveRigidbody))
            {
                objAboveRigidbody.velocity = new Vector2(myVelocity.x, objAboveRigidbody.velocity.y); // a velocidade no eixo y não muda
            }

            collision.transform.SetParent(null);
            isAbove = false;
        }

    }


    public void SetVelocity(Vector2 vel)
    {
        myVelocity = vel;
    }


    // void OnCollisionExit2D(Collision2D collision) {
    
    //     if (collision.gameObject.tag == "Player") 
    //     {
    //         if(collision.transform.parent == transform)
    //         {
    //             collision.transform.SetParent(null);
    //         }
    //         mvt = collision.gameObject.GetComponent<Movement>();
    //         mvt.noChao = false;
    //         isAbove = false;
    //     }

    // }

}
