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
    IEnumerator LevaBossAnimation()
    {
        ManagerEvents.PlayerMovementsEvents.LookedDirection(180);
        yield return new WaitForSeconds(1);
    }

    IEnumerator TrocaCameraAnimation()
    {
        yield return new WaitForSeconds(2);
        animator.Play("Camera Boss");
        yield return new WaitForSeconds(4);
        animator.Play("Camera Player");
        yield return new WaitForSeconds(2);
    }
    public void SwitchState()
    {
       StartCoroutine(LevaBossAnimation());
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
