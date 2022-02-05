using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetLife : MonoBehaviour
{
 
    [SerializeField] private string tagOfPlayer = "Player";

    bool alreadyPickedUp=false;


    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == tagOfPlayer && !alreadyPickedUp)
        {
            alreadyPickedUp = true;

            print("Pegou vida");

            // executar animação caso tenha

            DestroyMe();

        }
    }


    void DestroyMe(float waitTime=1f)
    {
        transform.localScale = Vector3.zero;
        Destroy(gameObject, waitTime);
    }
}
