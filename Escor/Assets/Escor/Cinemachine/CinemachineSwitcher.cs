using UnityEngine;
using System;
using System.Collections;
using UnityEngine.InputSystem;
using Cinemachine;

public class CinemachineSwitcher : MonoBehaviour
{

    [SerializeField]

    private InputAction action;
    private Animator animator;
    private bool playerCamera = true;
    [SerializeField]
    private CinemachineVirtualCamera vcam1; //Player camera
    [SerializeField]
    private CinemachineVirtualCamera vcam2; //Inimigo camera
    private void Awake()
    {
        animator = GetComponent<Animator>();

        
    }
    private void OnEnable()
    {
        action.Enable();
    }
    private void OnDisable()
    {
        action.Disable();
    }
    void Start()
    {
        // action.performed += _ => SwitchState(); //SwitchState();
    }

    IEnumerator CallAgain(float t)
    {

        yield return new WaitForSeconds(t);
        Debug.Log("2");

    }
    IEnumerator Scared()
    {
        ManagerEvents.PlayerMovementsEvents.LookedDirection(180);
        yield return new WaitForSeconds(1);
    }

    IEnumerator TrocaCameraAnimation()
    {
        ManagerEvents.PlayerMovementsEvents.LookedDirection(180);
        
        yield return new WaitForSeconds(2);
        animator.Play("Camera Boss");
        // yield return new WaitForSeconds(4);
        GameObject.FindGameObjectWithTag("Gate").GetComponent<Animator>().Play("PortaoAbrindoStart");
        // GameObject.FindGameObjectWithTag("Fake_Wall").GetComponent<Animator>().Play("parede sumindo");
        yield return new WaitForSeconds(4);
        animator.Play("Camera Player");
        yield return new WaitForSeconds(3);
        // GameObject.FindGameObjectWithTag("Exclamation").SetActive(true);
        GameObject.FindWithTag("Player").GetComponent<Animator>().Play("assustando");
        yield return new WaitForSeconds(1);
        // GameObject.FindGameObjectWithTag("Exclamation").SetActive(false);



    }
    public void SwitchState()
    {
       StartCoroutine(Scared());
    //    animator.Play("fadeTest");
    //     //Aproximar camera Javali
    //     if(playerCamera) {
    //         // StartCoroutine(dashCooldown(4));
    //         animator.Play("Camera Boss");
    //         Debug.Log("Animando o inimigo");
    //         // animator.Play("Camera Player");
    //     }
    //      else {
    //         Debug.Log("Animando o Player");
    //         animator.Play("Camera Player");
    //     } 

        // playerCamera = !playerCamera;
        // inimigoCamera = !inimigoCamera;
    }
    public void TrocaCamera()
    {
        StartCoroutine(TrocaCameraAnimation());
    }

    private void SwitchPriority()
    {
        if(playerCamera) {
            vcam1.Priority = 1;
            vcam2.Priority = 0;
        } else {
            vcam1.Priority = 0;
            vcam2.Priority = 1;
        }

        // inimigoCamera = !inimigoCamera;
        playerCamera = !playerCamera;
        

    }

    

}
