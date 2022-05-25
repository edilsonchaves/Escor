using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Alavanca : Ativador
{

    // [SerializeField] private string tagOfPlayer;
    [SerializeField] private KeyCode actionButton;
    // [SerializeField] private Animator myAnimator;
    // [SerializeField] private GameObject[] portoes;
    // [SerializeField] private GameObject[] plataformas; // plataforma que se movimentam


    // public bool openInSequence=false; // abre os portões em sequencia casa exista mais de um

    // public VcamFocusObject vcamFocusObject;

    private Movement mvt;

    bool alreadyTriggered=false;


    // void Start()
    // {
    //     myAnimator = GetComponent<Animator>();
    //     SetPlatformToWait(); // faz com que as plataformas esperem a ativação da alavanca
    // }


    void OnTriggerStay2D(Collider2D col)
    {
        if(col.tag == tagOfPlayer && Input.GetKey(actionButton) && !alreadyTriggered)
        {
            myAnimator.Play("alavanca", -1, 0); // é na própria animação que inicia a ativação de tudo

            // vcamFocusObject.StartFocus(portoes, false);
            // vcamFocusObject.StartFocus(new GameObject[]{portoes[0]});
            // vcamFocusObject.StartFocus(new GameObject[]{portoes[0]}, false);

            mvt = col.GetComponent<Movement>();

            SegurarAlavanca();
        }
    }


    public bool AlreadyTriggered()
    {
        return alreadyTriggered;
    }


    // void SetPlatformToWaitAlavanca()
    // {
    //     foreach(GameObject plt in plataformas)
    //     {
    //         plt.GetComponent<MovePlataform>().esperarAlavanca = true;
    //         // plt.GetComponent<MovePlataform>().startMovement = false;
    //     }
    // }


    // // quem está chamando é a própria animação da alavanca
    // void OpenAllDoors()
    // {
    //     StartCoroutine(OpenAllDoors_());
    // }
    //
    //
    // IEnumerator OpenAllDoors_()
    // {
    //     foreach(GameObject goPortao in portoes)
    //     {
    //         Animator portao = goPortao.GetComponent<Animator>();
    //         portao.Play("PortaoAbrindoStart", -1, 0);
    //
    //         if(openInSequence)
    //         {
    //             yield return new WaitUntil(() => (portao.GetCurrentAnimatorStateInfo(0).IsName("PortaoAbrindoStart"))); // espera a animação mudar para 'PortaoAbrindoStart'
    //             yield return new WaitUntil(() => (portao.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)); // espera a animação chegar no final
    //             vcamFocusObject.GoToNextStep();
    //             // vcamFocusObject.Finish();
    //         }
    //
    //
    //         yield return null;
    //     }
    //
    //     AtivarPlataformas();
    // }


    // // deve ser chamado após os portões
    // void AtivarPlataformas()
    // {
    //     StartCoroutine(AtivarPlataformas_());
    // }
    //
    //
    // IEnumerator AtivarPlataformas_()
    // {
    //     foreach(GameObject goPlataforma in plataformas)
    //     {
    //         goPlataforma.GetComponent<MovePlataform>().StartMovement();
    //         goPlataforma.GetComponent<MovePlataform>().esperarAlavanca = false;
    //
    //         if(openInSequence)
    //         {
    //             yield return new WaitForSeconds(1.5f);
    //             vcamFocusObject.GoToNextStep();
    //         }
    //
    //         yield return null;
    //     }
    //
    //     yield return null;
    // }


    protected void SegurarAlavanca()
    {
        Movement.KeepPlayerStopped();
        alreadyTriggered = true;
        mvt.animator.SetBool("Pegando", true);
        SfxManager.PlaySound(SfxManager.Sound.ativandoAlavanca);
        mvt.transform.position = new Vector3(transform.position.x-0.18f, mvt.transform.position.y, mvt.transform.position.z);
        mvt.transform.eulerAngles = Vector3.zero;
        // Movement.canMove = false;
    }


    protected void Soltarlavanca()
    {
        mvt.animator.SetBool("Pegando", false);
        Movement.StopKeepPlayerStopped();
        // Movement.canMove = true;
    }


}
