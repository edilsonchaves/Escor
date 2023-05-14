using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TochaManager : MonoBehaviour
{
    [SerializeField] private Animator myAnimator;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        RandomStartAnimationTime();
    }

    void RandomStartAnimationTime()
    {
        myAnimator.Play("TochaLoop", -1, Random.Range(0.0f, 1.0f));
    }
}
