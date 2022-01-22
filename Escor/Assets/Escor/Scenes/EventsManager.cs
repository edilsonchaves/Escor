using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventsManager : MonoBehaviour
{
    public UnityEvent cinematic;
    public Movement Player;
    private Transform target;
    void Start()
    {
        // Player = gameObject.transform.parent.gameObject.GetComponent<Movement>();
        // target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void OnTriggerEnter2D(Collider2D collisor)
    {
        if(collisor.gameObject.tag == "Player" && cinematic != null)
        {
            Debug.Log("colisou");
            cinematic.Invoke();
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if(cinematic != null)
        // {
            
        // }
    }
}
