using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class optimiationOfAnimationWithCollider : MonoBehaviour
{

    private Dictionary<Transform, Animator> allAnimatorFound = new Dictionary<Transform, Animator>();

    Animator anim;
    float stepOfAnimation;

    // void Awake()
    // {
    //     StopAllAnimator();

    // }

    // Start is called before the first frame update
    void Start()
    {
        CloudManager.StartUnactived = true;
        HideAllAnimation();
    }

    // Update is called once per frame
    void Update()
    {
        stepOfAnimation += Time.deltaTime/12;

        if(stepOfAnimation >= 1f)
            stepOfAnimation = .0f;

    }


    bool CheckIfExist(Transform trans)
    {
        return allAnimatorFound.ContainsKey(trans);
    }


    private void StopAllAnimator()
    {
        foreach(GameObject decoracao in GameObject.FindGameObjectsWithTag("DecoraçãoComAnimator"))
        {
            decoracao.GetComponent<Animator>().enabled = false;
        }
    }

    private void HideAllAnimation()
    {
        foreach(GameObject decoracao in GameObject.FindGameObjectsWithTag("DecoraçãoComCollider"))
        {
            decoracao.transform.GetChild(0).gameObject.SetActive(false);
        }

    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "DecoraçãoComCollider")
        {
            // if(CheckIfExist(col.transform))
            // {
            //     anim = allAnimatorFound[col.transform];
            // }
            // else
            // {
            //     anim = col.GetComponent<Animator>();
            //     allAnimatorFound.Add(col.transform, anim);
            // }
            col.transform.GetChild(0).gameObject.SetActive(true);
            // anim.enabled = true;
            // anim.Play(anim.GetCurrentAnimatorClipInfo(0)[0].clip.name, -1, stepOfAnimation);
        }
    }


    void OnTriggerExit2D(Collider2D col)
    {
        if(col.tag == "DecoraçãoComCollider")
        {
            col.transform.GetChild(0).gameObject.SetActive(false);
            // anim = allAnimatorFound[col.transform];
            // anim.enabled = false;
        }
    }

}
