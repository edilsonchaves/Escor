using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetLife : MonoBehaviour
{
 
    [SerializeField] private string tagOfPlayer;

    bool alreadyPickedUp=false;


    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == tagOfPlayer && !alreadyPickedUp)
        {
            alreadyPickedUp = true;

            print("Pegou vida");

            // executar animação 
            // dizer ao player que ele pegou uma vida

        }
    }
}
