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

    void Start()
    {
        PlayerMovement = gameObject.transform.parent.gameObject.GetComponent<Movement>();
        PlayerRopeControll = gameObject.transform.parent.gameObject.GetComponent<PlayerRopeControll>();
    }

    void OnTriggerEnter2D(Collider2D collisor)
    {

        if(!(collisor.gameObject.tag == groundTag || (groundLayer == (groundLayer | ( 1 << collisor.gameObject.layer)))))
            return;

        if(!PlayerMovement)
            PlayerMovement = gameObject.transform.parent.gameObject.GetComponent<Movement>();

        PlayerMovement.noChao = true;

        if(collisor.gameObject.tag == platformTag)
            return;

        PlayerMovement.transform.SetParent(null);
    }

    void OnTriggerExit2D(Collider2D collisor)
    {
        if(!(transform.parent.parent == null))
        {
            return;
        }

        if(!(collisor.gameObject.tag == groundTag || collisor.gameObject.tag == platformTag || groundLayer == (groundLayer | ( 1 << collisor.gameObject.layer))))
        {
            return;
        }

        if(!PlayerMovement)
            PlayerMovement = gameObject.transform.parent.gameObject.GetComponent<Movement>();


        PlayerMovement.noChao = false;
    }
}
