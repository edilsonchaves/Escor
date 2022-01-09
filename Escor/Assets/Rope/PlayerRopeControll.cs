using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRopeControll : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D rb;
    private HingeJoint2D hj;

    public float pushForce;
    public float climbSpeed;

    [HideInInspector]
    public bool attached = false;
    
    [HideInInspector]
    public Transform attachedTo;
    private GameObject disregard;

    [HideInInspector]
    public GameObject pulleySelected = null;
    private Movement mvtScript;

    void Awake()
    {
        mvtScript   = GetComponent<Movement>();
        rb          = GetComponent<Rigidbody2D>();
        hj          = GetComponent<HingeJoint2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(mvtScript.isGrounded && !attached)
        {
            attachedTo = null;
        }
        CheckKeyboardInputs();
    }

    void CheckKeyboardInputs()
    {
        if(attached)
        {
            if(Input.GetKey("a") || Input.GetKey("left"))
            {
                rb.AddRelativeForce(-Vector2.right * pushForce);
            }
            if(Input.GetKey("d") || Input.GetKey("right"))
            {
                rb.AddRelativeForce(Vector2.right * pushForce);
            }

            if(Input.GetKey("w") || Input.GetKey("up"))
            {
                Slide(1);
            }
            else if(Input.GetKey("s") || Input.GetKey("down"))
            {
                Slide(-1);
            }

            if(Input.GetKeyDown(KeyCode.Space))
            {
                Detach();
            }
        }   
    }

    public void Attach(Rigidbody2D ropeBone)
    {
        // print("Attach");
        ropeBone.gameObject.GetComponent<RopeSegment>().isPlayerAttached = true;
        ropeBone.velocity = rb.velocity*3;
        hj.connectedBody = ropeBone;
        transform.position = ropeBone.transform.position;
        hj.enabled = true;
        attached = true;
        attachedTo = ropeBone.gameObject.transform.parent;
    }

    public void Detach()
    {
        // print("Detach");
        hj.connectedBody.gameObject.GetComponent<RopeSegment>().isPlayerAttached = false;
        hj.enabled = false;
        hj.connectedBody = null;
        attached = false;
        // attachedTo = null;
    }

    public void Slide(int direction)
    {
        RopeSegment myConnection = hj.connectedBody.gameObject.GetComponent<RopeSegment>();
        GameObject newSeg = null;
        Vector3 dir = Vector3.zero;
        bool canClimb = true;

        if(direction == 1 )
        {
            if(myConnection.connectedAbove != null && myConnection.connectedAbove.gameObject.GetComponent<RopeSegment>() != null)
            {
                dir = (Vector2)myConnection.connectedAbove.transform.position - (Vector2)transform.position;
                float dist = dir.magnitude;

                if (dist <= 0.05f)
                {
                    newSeg = myConnection.connectedAbove;
                }
            }
        }
        else
        {
            if(myConnection.connectedBelow != null)
            {
                dir = (Vector2)myConnection.connectedBelow.transform.position - (Vector2)transform.position;
                float dist = dir.magnitude;

                if (dist<= 0.05f)
                {
                    newSeg = myConnection.connectedBelow;
                }
            }
            else if(hj.connectedBody != null)
            {
                dir = ((Vector2)hj.connectedBody.transform.position - (Vector2)transform.position);
                float dist = dir.magnitude;

                // if (dist >= 1.5f)
                // {
                //     canClimb = false;
                // }

            }       

        }


        if(newSeg != null)
        {
            transform.position = newSeg.transform.position;
            myConnection.isPlayerAttached = false;
            newSeg.GetComponent<RopeSegment>().isPlayerAttached = true;
            hj.connectedBody = newSeg.GetComponent<Rigidbody2D>();
        }
        else if(canClimb)
        {
            transform.position = transform.position + dir.normalized * climbSpeed * Time.deltaTime;
        }

    }


    protected Vector3 GetConnectionNear(bool below=false)
    {
        RopeSegment myConnection = hj.connectedBody.gameObject.GetComponent<RopeSegment>();
        float dist=-1, dist2=-1;

        if(myConnection.connectedBelow != null)
        {
            dist  = ((Vector2)myConnection.connectedBelow.transform.position - (Vector2)transform.position).magnitude;
        }

        if(myConnection.connectedAbove != null)
        {
            dist2  = ((Vector2)myConnection.connectedAbove.transform.position - (Vector2)transform.position).magnitude;
        }

        if(dist != -1 && dist2 != -1)
        {

            if(dist < dist2 || below)
            {
                return myConnection.connectedBelow.transform.position;
            }
            else
            {
                return myConnection.connectedAbove.transform.position;
            }

        }
        else if(dist != -1)
        {
            return myConnection.connectedBelow.transform.position;
        }
        else
        {
            return myConnection.connectedAbove.transform.position;
        }
    }

    protected bool CheckIfHaveConnection()
    {
        try
        {
            RopeSegment myConnection = hj.connectedBody.gameObject.GetComponent<RopeSegment>();
            print("Debug");
            return myConnection.connectedBelow != null || myConnection.connectedAbove != null; 
        }
        catch
        {
            return false;
        }
    }


    void OnTriggerStay2D(Collider2D col)
    {
        if(!attached)
        {
            if(col.tag == "Rope")
            {
                if(attachedTo != col.gameObject.transform.parent)
                {
                    if(disregard == null || col.gameObject.transform.parent.gameObject != disregard)
                    {
                        Attach(col.gameObject.GetComponent<Rigidbody2D>());
                    }
                }
            }
        }

    }

}
