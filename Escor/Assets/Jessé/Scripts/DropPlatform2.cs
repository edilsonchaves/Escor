using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// usar com CreatePlatform

public class DropPlatform2 : MonoBehaviour
{
    [SerializeField] private int                  numberOfPeoplesToDrop = 1;
    [SerializeField] private float                timeToStartDrop       = 0.5f;
    [SerializeField] private float                timeSwinging          = 2f;
    [SerializeField] private AbovePlatformManager abovePlatformManager;
    [SerializeField] private CreatePlatform       createPlatform;
                     private Animator             myAnimator;
                     private bool                 alreadyAnimade        = false;


    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }


    void Update()
    {
        if(!alreadyAnimade && abovePlatformManager.numberOfAbove >= numberOfPeoplesToDrop)
        {
            alreadyAnimade = true;
            // myAnimator.SetBool("StartAnimation", true);
            StartCoroutine("Drop");
        }
    }

    IEnumerator Drop()
    {
        yield return new WaitForSeconds(timeToStartDrop);
        myAnimator.SetBool("StartAnimation", true);
        yield return new WaitForSeconds(timeSwinging);
        myAnimator.SetBool("Drop", true);
        createPlatform.FadeInAllBlocks();
    }

}
