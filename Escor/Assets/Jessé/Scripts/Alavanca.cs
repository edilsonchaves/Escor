using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alavanca : MonoBehaviour
{

    [SerializeField] private string tagOfPlayer;
    [SerializeField] private KeyCode actionButton;
    [SerializeField] private Animator myAnimator;
    [SerializeField] private GameObject[] portoes;

    public bool openInSequence=false; // abre os portões em sequencia casa exista mais de um

    public VcamFocusObject vcamFocusObject;

    private Movement mvt;

    bool alreadyTriggered=false;


    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }


    void OnTriggerStay2D(Collider2D col)
    {
        if(col.tag == tagOfPlayer && Input.GetKey(actionButton) && !alreadyTriggered)
        {
            myAnimator.Play("alavanca", -1, 0);

            vcamFocusObject.StartFocus(portoes, openInSequence);

            // vcamFocusObject.StartFocus(portoes, false);
            // vcamFocusObject.StartFocus(new GameObject[]{portoes[0]});
            // vcamFocusObject.StartFocus(new GameObject[]{portoes[0]}, false);

            col.transform.position = new Vector3(transform.position.x-0.18f, col.transform.position.y, col.transform.position.z);
            col.transform.eulerAngles = Vector3.zero;
            mvt = col.GetComponent<Movement>();
            SegurarAlavanca();
        }
    }


    // quem está chamando é a própria animação da alavanca
    void OpenAllDoors()
    {
        StartCoroutine(OpenAllDoors_());
    }


    IEnumerator OpenAllDoors_()
    {
        foreach(GameObject goPortao in portoes)
        {
            Animator portao = goPortao.GetComponent<Animator>();
            portao.Play("PortaoAbrindoStart", -1, 0);

            if(openInSequence)
            {
                yield return new WaitUntil(() => (portao.GetCurrentAnimatorStateInfo(0).IsName("PortaoAbrindoStart"))); // espera a animação mudar para 'PortaoAbrindoStart'
                yield return new WaitUntil(() => (portao.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)); // espera a animação chegar no final
                vcamFocusObject.GoToNextStep();
                // vcamFocusObject.Finish();
            }


            yield return null;
        }

        // vcamFocusObject.Finish();
    }


    void SegurarAlavanca()
    {
        alreadyTriggered = true;
        mvt.animator.SetBool("Pegando", true);
        Movement.canMove = false;
    }


    void Soltarlavanca()
    {
        mvt.animator.SetBool("Pegando", false);
        Movement.canMove = true;
    }


}
