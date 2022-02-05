using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tartaruga : MonoBehaviour
{
    [SerializeField] private string tagOfPlayer = "Player";
    [SerializeField] private Animator myAnimator;
    [SerializeField] private KeyCode actionButton = KeyCode.W;

    bool talking = false;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if(col.tag == tagOfPlayer && Input.GetKey(actionButton) && !talking)
        {
            talking = true;
            myAnimator.SetBool("Talking", true);
        }
    }

    public void StopTalking()
    {
        talking = false;
        myAnimator.SetBool("Talking", false);

    }
}
