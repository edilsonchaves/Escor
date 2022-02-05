using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EspinhosManager : MonoBehaviour
{

    public bool killInstantly=false;

    void OnTriggerStay2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            // dá o dano no player ou mata
            // print("Collider2D");    
            
            Movement playerMovement = col.GetComponent<Movement>();
            if (!playerMovement.isInvunerable && playerMovement.Life > 0)
            {

                playerMovement.Life -= killInstantly ? playerMovement.Life : 1;
                // Debug.Log("E tome dano");
            }

        }
    }


    // caso seja colisor sem trigger
    void OnCollisionStay2D(Collision2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            // dá o dano no player ou mata
            // print("Collision2D");    

            Movement playerMovement = col.gameObject.GetComponent<Movement>();
            if (!playerMovement.isInvunerable && playerMovement.Life > 0)
            {

                playerMovement.Life -= killInstantly ? playerMovement.Life : 1;
                // Debug.Log("E tome dano");
            }

        }

    }

}
