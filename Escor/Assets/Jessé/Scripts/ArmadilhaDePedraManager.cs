using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmadilhaDePedraManager : MonoBehaviour
{


    [Header("Random")]
    public bool useRandom;
    [Range(1, 10)]
    public float delayRange;

    [Header("Manual")]
    public float[] Delays;

    private bool stop=false;


    // Start is called before the first frame update
    void Start()
    {
        for(int c=0; c<transform.childCount; c++)
        {
            StartCoroutine(StartOne(transform.GetChild(c).GetComponent<Animator>(), Delays[c]));
        }
    }


    IEnumerator StartOne(Animator anim, float delay)
    {
        SpriteRenderer mySpt = anim.transform.GetChild(0).GetComponent<SpriteRenderer>();
        while(true && !stop)
        {
            if(useRandom)
                delay = Random.Range(1,delayRange);

            yield return new WaitForSeconds(delay);

            if(Camera.main.IsObjectVisible(mySpt))
                SfxManager.PlaySound(SfxManager.Sound.armadilhaPedra); // som de efeito

            anim.Play("ArmadilhaDePedraStart", -1, 0);
        }
    }

    bool PlayerIsClose(Vector3 position)
    {
        return ((Vector2)(Camera.main.transform.position - position)).magnitude <= 22f;
    }


    public void StopAll()
    {
        stop = true;
    }

}
