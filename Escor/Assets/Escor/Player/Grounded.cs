using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grounded : MonoBehaviour
{
    Movement Player;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private string groundTag;

    void Start()
    {
        Player = gameObject.transform.parent.gameObject.GetComponent<Movement>();
    }

    void OnTriggerEnter2D(Collider2D collisor)
    {
        if(collisor.gameObject.tag == groundTag || collisor.gameObject.layer == groundLayer)
        {
            Player.noChao = true;
        }
    }

    void OnTriggerExit2D(Collider2D collisor)
    {
        if(collisor.gameObject.tag == groundTag || collisor.gameObject.layer == groundLayer)
        {
            Player.noChao = false;
        }
    }
}
