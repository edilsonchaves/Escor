using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grounded : MonoBehaviour
{

    
    Movement Player;
    void Start()
    {
        Player = gameObject.transform.parent.gameObject.GetComponent<Movement>();
    }

    void OnCollisionEnter2D(Collision2D collisor)
    {
        if(collisor.gameObject.tag == "ground")
        {
            Player.noChao = true;
        }
    }

    void OnCollisionExit2D(Collision2D collisor)
    {
        if(collisor.gameObject.tag == "ground")
        {
            Player.noChao = false;
        }
    }
}
