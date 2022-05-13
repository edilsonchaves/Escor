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
        // portao.Play("PortaoAbrindo", -1, 0);
        portao.SetBool("Start", true);
        GameObject.FindWithTag("ParedeJavali").GetComponent<Animator>().Play("parede sumindo2");
        yield return new WaitUntil(() => (portao.GetCurrentAnimatorStateInfo(0).IsName("PortaoAbrindo"))); // espera a animação mudar para 'PortaoAbrindo'
        yield return new WaitUntil(() => (portao.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)); // espera a animação chegar no final
        // print("Debug");
        vcam.GoToNextStep();
        // GameObject.FindWithTag("ParedeJavali").GetComponent<Animator>().Play("parede sumindo2");
        yield return new WaitForSeconds(3); // focando no javalizao
        IA_Javali jav = objects[1].GetComponent<IA_Javali>();
        jav.Move = true; // ativa o movimento do javalizao
        jav.MovementSpeed = 1.25f; // ativa o movimento do javalizao
        jav.JavaliAnimator.SetFloat("WalkSpeed", 0.5f);
        // yield return new WaitUntil(() => (jav.JavaliAnimator.GetCurrentAnimatorStateInfo(0).IsName("JavaliAndando"))); // espera a animação mudar para 'PortaoAbrindo'
        // jav.JavaliAnimator.GetCurrentAnimatorClipInfo(0).speed = 0.1f; // ativa o movimento do javalizao
        // objects[1].GetComponent<IA_Javali>().JavaliAnimator.speed = 0.1f; // ativa o movimento do javalizao
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
