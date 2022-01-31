using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public LayerMask groundLayer;

    // Start is called before the first frame update
    void Start()
    {
        if(transform.parent != null)
        {
            transform.localScale = Vector3.one*(2/transform.parent.lossyScale.x);   
        }
        else
        {
            transform.localScale = Vector3.one*2;   
        }
    }

    // Update is called once per frame
    void Update()
    {
        // float groundDistance = Physics2D.Raycast((Vector2)transform.position, -Vector2.up, Mathf.Infinity, groundLayer).distance;
        // RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, -Vector2.up, Mathf.Infinity, groundLayer);
        // print("groundDistance:      "+groundDistance);
        // Debug.DrawRay((Vector2)transform.position, -Vector2.up*groundDistance, Color.red);

    }
}
