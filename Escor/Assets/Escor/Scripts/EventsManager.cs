using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventsManager : MonoBehaviour
{
    public UnityEvent cinematic1;
    public UnityEvent cinematic2;
    public Movement Player;
    public VcamFocusObject vcam;
    public GameObject[] objects;
    private Transform target;
    public bool openInSequence=false;

    void Start()
    {

    }

    IEnumerator ProxCinematica()
    {
        yield return new WaitForSeconds(9);
        Movement.canMove = true;
    }

    void OnTriggerEnter2D(Collider2D collisor)
    {
        if(collisor.gameObject.tag == "Player" && cinematic1 != null)
        {

            Movement.canMove = false;
            vcam.StartFocus(objects);
            StartCoroutine(TrocaCameraAnimation());
            // cinematic1.Invoke();
            GetComponent<BoxCollider2D>().enabled = false;
            // StartCoroutine(ProxCinematica());
            
        }

        
    }

     public void TrocaCamera()
    {
        StartCoroutine(TrocaCameraAnimation());
    }
    IEnumerator TrocaCameraAnimation()
    {
        Animator portao = objects[0].GetComponent<Animator>(); 
        ManagerEvents.PlayerMovementsEvents.LookedDirection(180);
        
        yield return new WaitForSeconds(3);
        portao.Play("PortaoAbrindoStart", -1, 0);
        GameObject.FindWithTag("ParedeJavali").GetComponent<Animator>().Play("parede sumindo2");
        yield return new WaitUntil(() => (portao.GetCurrentAnimatorStateInfo(0).IsName("PortaoAbrindoStart"))); // espera a animação mudar para 'PortaoAbrindoStart'
        yield return new WaitUntil(() => (portao.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)); // espera a animação chegar no final
       
        vcam.GoToNextStep();
        // GameObject.FindWithTag("ParedeJavali").GetComponent<Animator>().Play("parede sumindo2");
        yield return new WaitForSeconds(3);
        vcam.GoToNextStep();
        yield return new WaitForSeconds(2);
        GameObject.FindWithTag("Exclamation").GetComponent<Animator>().Play("exclamacao");
        GameObject.FindWithTag("Player").GetComponent<Animator>().Play("assustando");
        yield return new WaitForSeconds(2);
        Movement.canMove = true;
        // GameObject.FindWithTag("Exclamation").SetActive(false);
        // GameObject.FindGameObjectWithTag("Exclamation").SetActive(false);



    }
}
