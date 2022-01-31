using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideCavern : MonoBehaviour
{
    private Animator myAnimator;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            myAnimator.Play("FadeInStart", -1, 0);
        }
    }


    void OnTriggerExit2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            myAnimator.Play("FadeOutStart", -1, 0);
        }
    }
}
