using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchScene : MonoBehaviour
{
    public int sceneNumber;
    public string tagOfPlayer = "Player";

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == tagOfPlayer)
        {
            // carregar cena
        }
    }

}
