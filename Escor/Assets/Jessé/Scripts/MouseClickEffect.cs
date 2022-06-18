using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClickEffect : MonoBehaviour
{
    public Animator clickEffect;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        clickEffect.SetBool("Clicked", Input.GetMouseButton(0) || Input.GetMouseButtonDown(0));
    }
}
