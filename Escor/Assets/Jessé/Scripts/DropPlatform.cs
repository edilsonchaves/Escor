using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPlatform : MonoBehaviour
{

    public int numberOfPeoplesToDrop=1;

    public enum PlatformTypes {TwoBlocks, ThreeBlocks, ThreeBlocksWithShadow, SevenBlocks};
    public PlatformTypes PlatformType;

    public string tagOfPlayer="Player";

    public AbovePlatformManager abovePlatformManager;

    private Animator myAnimator;
    bool alreadyAnimade = false;


    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }


    void Update()
    {
        if(!alreadyAnimade && abovePlatformManager.numberOfAbove >= numberOfPeoplesToDrop)
        {
            alreadyAnimade = true;

            switch(PlatformType)
            {
                case (PlatformTypes.TwoBlocks):
                    myAnimator.Play("ChaoQueCai1Start", -1, 0);
                    break;
                case (PlatformTypes.ThreeBlocks):
                    myAnimator.Play("ChaoQueCai2Start", -1, 0);
                    break;
                case (PlatformTypes.ThreeBlocksWithShadow):
                    myAnimator.Play("ChaoQueCai3Start", -1, 0);
                    break;
                case (PlatformTypes.SevenBlocks):
                    myAnimator.Play("ChaoQueCai7Start", -1, 0);
                    break;
            }
        }
    }


    // void OnTriggerEnter2D(Collider2D col)
    // {
    //     if(col.tag.Equals(tagOfPlayer) && !alreadyAnimade)
    //     {
    //         alreadyAnimade = true;

    //         switch(PlatformType)
    //         {
    //             case (PlatformTypes.TwoBlocks):
    //                 myAnimator.Play("ChaoQueCai1Start", -1, 0);
    //                 break;
    //             case (PlatformTypes.ThreeBlocks):
    //                 myAnimator.Play("ChaoQueCai2Start", -1, 0);
    //                 break;
    //             case (PlatformTypes.ThreeBlocksWithShadow):
    //                 myAnimator.Play("ChaoQueCai3Start", -1, 0);
    //                 break;
    //         }
    //     }
    // }

    void DestroyMe()
    {
        // não pode destruir antes de remover o player ou javali, senão irá destruir eles também
        // o melhor a se fazer é apenas deixa a plataforma invisivel
        Destroy(gameObject, .2f);
    }

}
