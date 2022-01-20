using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [HideInInspector]
    public IA_Javali_Tiro script;
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player" || col.gameObject.layer == 7)
        {
            script.DeletBullet(this.gameObject);
            print("Tiro acertou o player");
        }
    }

}
