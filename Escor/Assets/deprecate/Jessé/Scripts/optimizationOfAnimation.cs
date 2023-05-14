using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class optimizationOfAnimation : MonoBehaviour
{

    public float minimeDistance;

    private List<Animator> animators = new List<Animator>();
    private GameObject[] gameObjectsWithAnimatorAndAnimation;
    private Transform Player; 
    private float stepOfAnimation = 0.75f;


    // Start is called before the first frame update
    void Start()
    {
        Player = Camera.main.transform;
        // Player = GameObject.FindGameObjectWithTag("Player").transform;
        Optimaze();
    }


    void Update()
    {
        stepOfAnimation += Time.deltaTime/12;

        if(stepOfAnimation >= 1f)
            stepOfAnimation = .0f;

        foreach(Animator anim in animators)
        {
            if(IsClose(anim.gameObject.transform.position))
            {   
                if(!anim.enabled)
                {
                    anim.enabled = true;;
                    // anim.normalizedTime = stepOfAnimation;   
                    anim.Play(anim.GetCurrentAnimatorClipInfo(0)[0].clip.name, -1, stepOfAnimation);
                    // anim.Play(, -1, stepOfAnimation);
                }
            }
            else
            {
                anim.enabled = false;
            }

            // yield return null;
        }

    }


    private void Optimaze()
    {
        GetAllGameObjectWithAnimators();
        GetAllAnimators();
        DesactiveAllAnimators();
        // StartCoroutine("Optimization");
    }


    private IEnumerator Optimization()
    {   
        while(true)
        {
            foreach(Animator anim in animators)
            {
                if(IsClose(anim.gameObject.transform.position))
                {   
                    if(!anim.enabled)
                    {
                        anim.enabled = true;;
                        // anim.normalizedTime = stepOfAnimation;   
                        anim.Play(anim.GetCurrentAnimatorClipInfo(0)[0].clip.name, -1, stepOfAnimation);
                        // anim.Play(, -1, stepOfAnimation);
                    }
                }
                else
                {
                    anim.enabled = false;
                }

                yield return null;
            }

            yield return null;
        }

    }


    private void GetAllGameObjectWithAnimators()
    {
        gameObjectsWithAnimatorAndAnimation = GameObject.FindGameObjectsWithTag("DecoraçãoComAnimator");
    }


    private void GetAllAnimators()
    {
        foreach(GameObject decoracao in gameObjectsWithAnimatorAndAnimation)
        {
            animators.Add(decoracao.GetComponent<Animator>());
        }
    }


    private void DesactiveAllAnimators()
    {
        foreach(Animator anim in animators)
        {
            anim.enabled = false;
        }        
    }


    private bool IsClose(Vector3 pos)
    {
        // pos = transform.TransformPoint(pos);
        // print(Vector2.Distance((Vector2)(transform.TransformPoint(pos)), (Vector2)transform.TransformPoint(Player.position)));
        return ((Vector2)(transform.TransformPoint(pos) - transform.TransformPoint(Player.position))).magnitude < minimeDistance;
    }

}
