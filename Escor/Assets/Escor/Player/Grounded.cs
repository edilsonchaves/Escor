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
            print("noChao");
            Player.noChao = true;
        }
    }

    void OnTriggerExit2D(Collider2D collisor)
    {
        // as vezes o player está na plataforma porém noChao=false, para contorna isso estou verificando se ele está ou não em uma plataforma
        if((collisor.gameObject.tag == groundTag || collisor.gameObject.layer == groundLayer) && (collisor.transform.parent == null))
        {
            print("!noChao");
            Player.noChao = false;
        }
    }
}
