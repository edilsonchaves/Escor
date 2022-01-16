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

    void OnTriggerEnter2D(Collider2D collisor)
    {
        if(collisor.gameObject.tag == "ground" || collisor.gameObject.layer == 7)
        {
            Player.isGrounded = true;
            Player.isJumping  = false;
        }
    }

    void OnTriggerExit2D(Collider2D collisor)
    {
        if(collisor.gameObject.tag == "ground" || collisor.gameObject.layer == 7)
        {
            Player.isGrounded = false;
            Player.isJumping  = true;
        }
    }
}
