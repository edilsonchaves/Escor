using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Rigidbody2D myRb;

    // Start is called before the first frame update
    void Start()
    {
        myRb.velocity = Vector2.right * 0.66666f;   
    }

}
