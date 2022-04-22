using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotaManager : MonoBehaviour
{
    public EstalactiteManager scriptOfEstalactite;
    public string tagOfPlayer = "Player";
    public string tagOfGround = "ground";
    public string tagOfPlatform = "Platform";

    public LayerMask layersToCanSplash;


    void OnTriggerEnter2D(Collider2D col)
    {
        // if(col.tag == tagOfPlayer || col.tag == tagOfGround || col.tag == tagOfPlatform || col.gameObject.layer == layersToCanSplash)
        // if(col.gameObject.layer == layersToCanSplash.value)
        if( layersToCanSplash == (layersToCanSplash | ( 1 << col.gameObject.layer)))
        {
            scriptOfEstalactite.ShowSplashAnimation();
            SfxManager.PlayRandomGotaSplash();
        }
    }
}
