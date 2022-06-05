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
        // Movement.StopKeepPlayerStopped();
    }

    void OnTriggerEnter2D(Collider2D collisor)
    {
        if(collisor.gameObject.tag == "Player" && cinematic1 != null)
        {

            // Movement.KeepPlayerStopped();
            vcam.StartFocus(objects);
            StartCoroutine(TrocaCameraAnimation());
            // cinematic1.Invoke();
            GetComponent<Collider2D>().enabled = false;
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

        yield return new WaitUntil(() => !Movement.canMove);  // espera até o player não conseguir mais se mexer antes de virar ele para o portão
        yield return new WaitForSeconds(1f);
        ManagerEvents.PlayerMovementsEvents.LookedDirection(180);

        yield return new WaitForSeconds(2);

        portao.SetBool("Start", true);

        yield return new WaitUntil(() => (portao.GetCurrentAnimatorStateInfo(0).IsName("PortaoAbrindo"))); // espera a animação mudar para 'PortaoAbrindo'
        yield return new WaitUntil(() => (portao.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.4f)); // espera a animação chegar em 40%
        GameObject.FindWithTag("ParedeJavali").GetComponent<Animator>().Play("parede sumindo2");
        yield return new WaitUntil(() => (portao.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)); // espera a animação chegar no final
        // print("Debug");
        vcam.GoToNextStep();
        // GameObject.FindWithTag("ParedeJavali").GetComponent<Animator>().Play("parede sumindo2");
        yield return new WaitForSeconds(3); // focando no javalizao
        IA_Javali jav = objects[1].GetComponent<IA_Javali>();
        jav.Move = true; // ativa o movimento do javalizao
        jav.MovementSpeed = 1.2f; // ativa o movimento do javalizao
        jav.JavaliAnimator.SetFloat("WalkSpeed", 1f);
        vcam.transitionTimeComingBack = 0.5f;
        GameObject ply = GameObject.FindWithTag("Player");

        Movement.KeepPlayerStopped(); // player para
        vcam.GoToNextStep(); // volta pro player

        yield return new WaitForSeconds(0.4f); // tempo até o susto

        ply.GetComponent<Animator>().Play("assustando");
        GameObject.FindWithTag("Exclamation").GetComponent<Animator>().Play("exclamacao",-1,0);
        
        Movement.StopKeepPlayerStopped(); // player anda

        // ManagerEvents.PlayerMovementsEvents.LookedDirection(180);
        // [Jessé] não precisa mais, pq a exclamação está na propria animação de 'assustando'

        // yield return new WaitForSeconds(2);


    }
}
