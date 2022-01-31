using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotaManager : MonoBehaviour
{
    public EstalactiteManager script;
    public string tagOfPlayer = "Player";
    public string tagOfGround = "ground";
    public string tagOfPlatform = "Platform";


    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == tagOfPlayer || col.tag == tagOfGround || col.tag == tagOfPlatform)
        {
            script.ShowSplashAnimation();
        }
    }
}
