using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MostrarCor : MonoBehaviour
{
    [SerializeField] private Movement movementScript;
    [SerializeField] private Animator myAnimator;
    [SerializeField] private Transform player;    

    // Start is called before the first frame update
    void Start()
    {
        player          = transform.parent;
        movementScript  = player.GetComponent<Movement>();
        myAnimator      = GetComponent<Animator>();
        transform.SetParent(null);
        transform.localScale = Vector3.one *1.34f;
    }

    // Update is called once per frame
    void Update()
    {
        myAnimator.SetBool("Jump", movementScript.pulando);
        myAnimator.SetBool("Shield", movementScript.defendendo);
        transform.position = player.position;
    }


}
