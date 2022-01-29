using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventsManager : MonoBehaviour
{
    public UnityEvent cinematic1;
    public UnityEvent cinematic2;
    public Movement Player;
    private Transform target;
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
            cinematic1.Invoke();
            GetComponent<BoxCollider2D>().enabled = false;
            StartCoroutine(ProxCinematica());
            
        }

        
    }

}
