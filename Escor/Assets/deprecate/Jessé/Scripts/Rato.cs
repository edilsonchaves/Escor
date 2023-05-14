using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rato : MonoBehaviour
{
    public Animator ratoAnim;
    public Alavanca alavanca;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Start_");
    }


    IEnumerator Start_()
    {
        yield return new WaitUntil(() => alavanca.AlreadyTriggered());
        yield return new WaitForSeconds(6f);

        ratoAnim.Play("RatoAndando", -1, 0f);
        this.enabled = false;
    }
}
