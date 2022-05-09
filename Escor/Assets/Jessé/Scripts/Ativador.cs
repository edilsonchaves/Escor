using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Ativador : MonoBehaviour
{

    [SerializeField] protected string          tagOfPlayer = "Player";
    [SerializeField] protected Animator        myAnimator;

    // objetos que serão ativados
    [SerializeField] protected GameObject[]    portoes;     // portão normal ou de galhos
    [SerializeField] protected GameObject[]    plataformas; // plataforma que se movimenta
    [SerializeField] protected GameObject[]    colisores;   // objeto que é apenas um colisor
    // --------------------------

    [SerializeField] protected VcamFocusObject vcamFocusObject;
    [SerializeField] protected bool            focusOnce = false; // [true] -> foca nos objetos uma única vez
                     protected bool            alreadyFocused;
    [SerializeField] protected bool            openInSequence=false; // ativa em sequencia

    string _tagOfObj = "Player";

    void Start()
    {
        // print("Ativador");
        myAnimator = GetComponent<Animator>();
        SetPlatformToWait(); // faz com que as plataformas esperem a ativação
    }


    // retorna em um único Array todos os objetos que devem ser ativados
    protected GameObject[] GetAllObjectsToActivate()
    {
        return portoes.Concat(plataformas).Concat(colisores).ToArray();
    }


    // ativa todos os objetos (eles vão se ativando em seguida)
    protected void ActivateAll(string tagOfObj="Player")
    {
        _tagOfObj = tagOfObj;
        if(!alreadyFocused && tagOfObj == "Player")
            vcamFocusObject.StartFocus(GetAllObjectsToActivate(), openInSequence);

        StartCoroutine(OpenAllDoors_());
    }


    protected void DisableAll()
    {
        // a camera não deve focar nos objetos sendo desativados
        // para que não fique algo muito repetitivo

        foreach(GameObject goPortao in portoes)
            goPortao.GetComponent<Animator>().Play("PortaoFechando", -1, 0);

        foreach(GameObject goPlataforma in plataformas)
            goPlataforma.GetComponent<MovePlataform>().esperarAtivador = false;
    }



    IEnumerator OpenAllDoors_()
    {
        yield return new WaitForSeconds(vcamFocusObject.timeToStartFocus+vcamFocusObject.transitionTimeGoing);

        foreach(GameObject goPortao in portoes)
        {
            Animator portao = goPortao.GetComponent<Animator>();
            portao.Play("PortaoAbrindo", -1, 0);

            if(openInSequence)
            {
                yield return new WaitUntil(() => (portao.GetCurrentAnimatorStateInfo(0).IsName("PortaoAbrindo"))); // espera a animação mudar para 'PortaoAbrindo'
                yield return new WaitUntil(() => (portao.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)); // espera a animação chegar no final
                vcamFocusObject.GoToNextStep();
            }

            yield return null;
        }

        AtivarPlataformas();
    }


    // deve ser chamado após os portões
    protected void AtivarPlataformas()
    {
        StartCoroutine(AtivarPlataformas_());
    }


    IEnumerator AtivarPlataformas_()
    {
        foreach(GameObject goPlataforma in plataformas)
        {
            goPlataforma.GetComponent<MovePlataform>().StartMovement();
            goPlataforma.GetComponent<MovePlataform>().esperarAtivador = false;

            if(openInSequence)
            {
                yield return new WaitForSeconds(1.5f);
                vcamFocusObject.GoToNextStep();
            }

            yield return null;
        }

        DesativarColisores();
    }


    // deve ser chamado após os portões
    protected void DesativarColisores()
    {
        StartCoroutine(DesativarColisores_());
    }


    IEnumerator DesativarColisores_()
    {
        foreach(GameObject goColisor in colisores)
        {
            goColisor.SetActive(false);

            if(openInSequence && !alreadyFocused)
            {
                yield return new WaitForSeconds(vcamFocusObject.focusTime);
                vcamFocusObject.GoToNextStep();
            }

            yield return null;
        }

        if(_tagOfObj == "Player")
            alreadyFocused = focusOnce;
    }


    protected void SetPlatformToWait()
    {
        foreach(GameObject plt in plataformas)
        {
            plt.GetComponent<MovePlataform>().esperarAtivador = true;
            // plt.GetComponent<MovePlataform>().startMovement = false;
        }
    }




}
