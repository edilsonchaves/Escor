using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbovePlatformManager : MonoBehaviour
{
    [SerializeField]
    public bool isAbove;
    private Movement mvt;

    void Update()
    {
        // if(mvt)
        // {
        //     mvt.noChao = isAbove;
        // }
            
    }

    void OnCollisionEnter2D(Collision2D collision) {
        
        if (collision.gameObject.tag == "Player" && Mathf.Round(collision.contacts[0].normal.y) == Mathf.Round(-Vector2.up.y)) 
        {
            mvt = collision.gameObject.GetComponent<Movement>();
            mvt.noChao = true;
            collision.transform.SetParent(transform);
            isAbove = true;
        }

    }


    void OnCollisionExit2D(Collision2D collision) {
    
        if (collision.gameObject.tag == "Player") 
        {
            if(collision.transform.parent == transform)
            {
                collision.transform.SetParent(null);
            }
            // mvt = collision.gameObject.GetComponent<Movement>();
            // mvt.noChao = false;
            isAbove = false;
        }

    }

}
