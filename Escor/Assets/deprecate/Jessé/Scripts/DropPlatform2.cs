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
        // if(!alreadyAnimade && abovePlatformManager.numberOfAbove >= numberOfPeoplesToDrop)
        // {
            // if(abovePlatformManager.javalisAbove.Count != 0)
            // myAnimator.SetBool("StartAnimation", true);
        // }

        if(alreadyAnimade)
            return;

        if(abovePlatformManager.numberOfAbove < numberOfPeoplesToDrop)
            return;

        if(numberOfPeoplesToDrop > 1 && !abovePlatformManager.JavaliIsOnCenterOfPlataform())
            return;

        StartCoroutine("Drop");
    }

    IEnumerator Drop()
    {
        alreadyAnimade = true;
        StopJavalis();                                    // javali param de se mover
        yield return new WaitForSeconds(timeToStartDrop); // esperando para começar a balançar
        myAnimator.SetBool("StartAnimation", true);       // começa a balançar
        yield return new WaitForSeconds(timeSwinging);    // tempo balançando
        myAnimator.SetBool("Drop", true);                 // cai
        MoveJavalis();                                    // javali volta a se mover
        createPlatform.FadeInAllBlocks();                 // desaparecer com os blocos
    }


    private void StopJavalis()
    {
        abovePlatformManager.StopJavalis();
    }


    private void MoveJavalis()
    {
        abovePlatformManager.MoveJavalis();
    }

}
