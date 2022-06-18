using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transicaocena : MonoBehaviour
{

    [SerializeField] private Animator animator;
    // private int cenaIndice;


    void Start()
    {
        StartCoroutine(TrocarCena());
    }

    void update ()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StopAllCoroutines();
            StartCoroutine(TrocarCena(1));
        }
    }


    IEnumerator TrocarCena(float delay=5f)
    {
        yield return new WaitForSeconds(delay);
        animator.Play("Fade In");
        print("Fade in");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(1);
    }


}