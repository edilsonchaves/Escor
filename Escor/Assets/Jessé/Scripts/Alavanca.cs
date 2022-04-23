using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alavanca : MonoBehaviour
{

    [SerializeField] private string tagOfPlayer; 
    [SerializeField] private KeyCode actionButton;
    [SerializeField] private Animator myAnimator;
    [SerializeField] private Animator[] portoes;
    
    
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
            col.transform.position = new Vector3(transform.position.x-0.18f, col.transform.position.y, col.transform.position.z);
            col.transform.eulerAngles = Vector3.zero;
            mvt = col.GetComponent<Movement>();
            SegurarAlavanca();
        }
    }


    // quem está chamando é a própria animação da alavanca
    void OpenAllDoors()
    {
        foreach(Animator portao in portoes)
        {
            portao.Play("PortaoAbrindoStart", -1, 0);
        }
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
