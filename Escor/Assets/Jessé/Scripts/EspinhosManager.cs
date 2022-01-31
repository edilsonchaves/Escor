using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EspinhosManager : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            // dá o dano no player ou mata
        }
    }

}
