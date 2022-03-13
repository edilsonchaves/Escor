using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grounded : MonoBehaviour
{

    // tem um bug quando kuro sai de uma plataforma para o chão, isso quando a plataforma está encostada em um tile do tipo chão.
    // então o bug é que kuro fica sempre caindo, já que noChao está falso


    Movement PlayerMovement;
    PlayerRopeControll PlayerRopeControll;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private string groundTag;
    [SerializeField] private string platformTag = "Plataform";

    List<Collider2D> currentCollisions = new List<Collider2D>();

    void Start()
    {
        PlayerMovement = gameObject.transform.parent.gameObject.GetComponent<Movement>();
        // PlayerRopeControll = gameObject.transform.parent.gameObject.GetComponent<PlayerRopeControll>(); //
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if((groundLayer == (groundLayer | ( 1 << col.gameObject.layer))) && !PlayerMovement.noChao)
        {
            PlayerMovement.noChao  = true;
            
            if(!currentCollisions.Contains(col))
                currentCollisions.Add(col);

            if(currentCollisions.Count <= 1) // isso serve para não cancelar o pulo caso toque outra superficie no inicio do pulo
                PlayerMovement.pulando = false;
        }
    }


    void OnTriggerStay2D(Collider2D col)
    {
        // if(PlayerMovement.pulando)
            // return;
        
        if((groundLayer == (groundLayer | ( 1 << col.gameObject.layer))))
        {
            if(!currentCollisions.Contains(col))
                currentCollisions.Add(col);

            PlayerMovement.noChao = true;
        }
    }


    void OnTriggerExit2D(Collider2D col)
    {

        if((groundLayer == (groundLayer | ( 1 << col.gameObject.layer))))
        {
            if(currentCollisions.Contains(col))
                currentCollisions.Remove(col);

            PlayerMovement.noChao = false;
        }
    }
















    // Método antigo - Backup ---------------------------------------------------------------------

    // void OnTriggerEnter2D(Collider2D collisor)
    // {

    //     if(!(collisor.gameObject.tag == groundTag || (groundLayer == (groundLayer | ( 1 << collisor.gameObject.layer)))))
    //         return;

    //     if(!PlayerMovement)
    //         PlayerMovement = gameObject.transform.parent.gameObject.GetComponent<Movement>();

    //     PlayerMovement.noChao = true;

    //     if(collisor.gameObject.tag == platformTag)
    //         return;

    //     PlayerMovement.transform.SetParent(null);
    // }

    // void OnTriggerExit2D(Collider2D collisor)
    // {
    //     if(!(transform.parent.parent == null))
    //     {
    //         return;
    //     }

    //     if(!(collisor.gameObject.tag == groundTag || collisor.gameObject.tag == platformTag || groundLayer == (groundLayer | ( 1 << collisor.gameObject.layer))))
    //     {
    //         return;
    //     }

    //     if(!PlayerMovement)
    //         PlayerMovement = gameObject.transform.parent.gameObject.GetComponent<Movement>();


    //     PlayerMovement.noChao = false;
    // }
}
