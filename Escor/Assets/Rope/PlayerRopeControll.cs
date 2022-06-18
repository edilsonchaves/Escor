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

    // [HideInInspector]
    public Transform attachedTo;
    private GameObject disregard;

    [HideInInspector]
    public GameObject pulleySelected = null;
    private Movement mvtScript;
    private Transform currentSegment;

    Vector3 myPositionInCurrentSegment;
    float distanceFromSegmentCenter;
    RopeSegment myConnection;
    bool canSwing, isAboveSegment;

    void Awake()
    {
        mvtScript   = GetComponent<Movement>();
        rb          = GetComponent<Rigidbody2D>();
        hj          = GetComponent<HingeJoint2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Vector3 oldPos = transform.localPosition;
        // print("rb.velocity:  "+rb.velocity);
        if(mvtScript.noChao && !attached)
        {
            attachedTo = null;
        }
        else if(attached)
        {

            // SfxManager.PlaySound(SfxManager.Sound.cordaBalancando); // som de efeito
            if(mvtScript.transform.eulerAngles.y == 0)
            {
                // print(oldPos);
                Vector3 newRotation = new Vector3(0, Mathf.Round(mvtScript.transform.eulerAngles.y), Mathf.Round(currentSegment.eulerAngles.z));
                // print(newRotation);
                // print(currentSegment.eulerAngles.x+"      "+ Mathf.Round(mvtScript.transform.eulerAngles.y)+"      "+ Mathf.Round(currentSegment.eulerAngles.z));
                // newRotation = Vector3.zero;
                transform.rotation = Quaternion.Euler(newRotation);

                // transform.eulerAngles = newRotation;
                // print(transform.localPosition);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, Mathf.Round(mvtScript.transform.eulerAngles.y), -Mathf.Round(currentSegment.eulerAngles.z));
            }


        }

        CheckKeyboardInputs();

        UpdateMyPositionInSegment();

    }


    void UpdateMyPositionInSegment()
    {
        if(!attached)
            return;


        // print($"Above [{isAboveSegment}]  Distance [{distanceFromSegmentCenter}]");
        Debug.DrawLine(currentSegment.position, currentSegment.position + ((isAboveSegment?currentSegment.up:-currentSegment.up) * distanceFromSegmentCenter), Color.red);
        transform.position = currentSegment.position + ((isAboveSegment?currentSegment.up:-currentSegment.up) * distanceFromSegmentCenter);
    }


    void CheckKeyboardInputs()
    {
        if(attached)
        {
            bool stopped = true;
            if(Input.GetKey("a") || Input.GetKey("left"))
            {
                stopped = false;
                mvtScript.transform.eulerAngles = new Vector3(mvtScript.transform.eulerAngles.x,180, mvtScript.transform.eulerAngles.z);
                rb.AddRelativeForce(-Vector2.right * pushForce * (canSwing ? 1 : 0));
            }
            if(Input.GetKey("d") || Input.GetKey("right"))
            {
                stopped = false;
                mvtScript.transform.eulerAngles = new Vector3(mvtScript.transform.eulerAngles.x,0,mvtScript.transform.eulerAngles.z);
                rb.AddRelativeForce(Vector2.right * pushForce * (canSwing ? 1 : 0));
            }

            if(Input.GetKey("w") || Input.GetKey("up"))
            {
                stopped = false;
                Slide(1);
            }
            else if(Input.GetKey("s") || Input.GetKey("down"))
            {
                stopped = false;
                Slide(-1);
            }

            if(Input.GetKeyDown(KeyCode.Space))
            {
                Detach();
            }

            // if(stopped)
            // {
            //     Vector2 dir                 = (Vector2)(transform.position - hj.connectedBody.transform.position);
            //     // float distanceFromCenter    = (dir).magnitude;
            //     transform.position          = (Vector2) hj.connectedBody.transform.position + dir;
            // }

        }
    }

    public void Attach(Rigidbody2D ropeBone)
    {
        // print("Attach");
        SfxManager.PlayRandomCordaPegar(); // som de efeito
        StopAllCoroutines();
        // StopCoroutine(KeepPlayingSoundOfCordaBalancando());
        StartCoroutine(KeepPlayingSoundOfCordaBalancando());
        GetComponent<Movement>().jumpsLeft = 1;
        ropeBone.gameObject.GetComponent<RopeSegment>().isPlayerAttached = true;
        currentSegment = ropeBone.gameObject.GetComponent<RopeSegment>().transform;
        ropeBone.velocity = rb.velocity*3;
        hj.connectedBody = ropeBone;
        myPositionInCurrentSegment = ropeBone.transform.position;
        transform.position = myPositionInCurrentSegment;
        hj.enabled = true;
        attached = true;
        attachedTo = ropeBone.gameObject.transform.parent;
        canSwing = attachedTo.GetComponent<Rope>().canSwing;
        myConnection = hj.connectedBody.gameObject.GetComponent<RopeSegment>();
    }

    public void Detach()
    {
        // print("Detach");
        StopAllCoroutines();
        hj.connectedBody.gameObject.GetComponent<RopeSegment>().isPlayerAttached = false;
        hj.enabled = false;
        hj.connectedBody = null;
        attached = false;
        // attachedTo = null;
    }

    public void Slide(int direction)
    {
        GameObject newSeg = null;
        Vector3 dir = Vector3.zero;
        bool canClimb = true;
        myConnection = hj.connectedBody.gameObject.GetComponent<RopeSegment>();

        if(direction == 1)
        {
            if(myConnection.connectedAbove != null && myConnection.connectedAbove.gameObject.GetComponent<RopeSegment>() != null)
            {
                dir = (Vector2)myConnection.connectedAbove.transform.position - (Vector2)transform.position;
                float dist = dir.magnitude;

                float myDis = ((Vector2)(transform.position - myConnection.connectedAbove.transform.position)).magnitude;
                float segDis = ((Vector2)(myConnection.connectedAbove.transform.position - currentSegment.position)).magnitude;


                if (dist <= 0.05f || myDis < segDis)
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
            // transform.position = newSeg.transform.position;
            myConnection.isPlayerAttached = false;
            newSeg.GetComponent<RopeSegment>().isPlayerAttached = true;
            myConnection = newSeg.GetComponent<RopeSegment>();
            hj.connectedBody = newSeg.GetComponent<Rigidbody2D>();
        }
        else if(canClimb)
        {
            transform.position = transform.position + dir.normalized * climbSpeed * Time.deltaTime;
            // myPositionInCurrentSegment = dir.normalized * climbSpeed * Time.deltaTime;
        }

        currentSegment = myConnection.transform;

        if(myConnection.connectedBelow == null)
        {
            float myDis = ((Vector2)(transform.position - myConnection.connectedAbove.transform.position)).magnitude;
            float segDis = ((Vector2)(myConnection.connectedAbove.transform.position - currentSegment.position)).magnitude;

            distanceFromSegmentCenter = ((Vector2)(transform.position - currentSegment.position)).magnitude;
            isAboveSegment = myDis < segDis;
        }

        else if(myConnection.connectedAbove == null)
        {
            float myDis = ((Vector2)(transform.position - myConnection.connectedBelow.transform.position)).magnitude;
            float segDis = ((Vector2)(myConnection.connectedBelow.transform.position - currentSegment.position)).magnitude;

            distanceFromSegmentCenter = ((Vector2)(transform.position - currentSegment.position)).magnitude;
            isAboveSegment = myDis > segDis;
        }
        else
        {
            float myDisUp = ((Vector2)(transform.position - myConnection.connectedAbove.transform.position)).magnitude;
            float myDisDown = ((Vector2)(transform.position - myConnection.connectedBelow.transform.position)).magnitude;

            distanceFromSegmentCenter = ((Vector2)(transform.position - currentSegment.position)).magnitude;
            isAboveSegment = myDisUp < myDisDown;
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


    IEnumerator KeepPlayingSoundOfCordaBalancando()
    {
        while(true)
        {
            // SfxManager.PlaySound(SfxManager.Sound.cordaBalancando); // som de efeito
            yield return new WaitForSeconds(2f + Random.Range(0.0f, 1.0f));

            if(!attached)
                break;

            if (!CheckIfSegmentIsCloseToCenter())
            {
                SfxManager.PlayRandomCordaPegar(); // som de efeito
            }

            yield return null;
        }
    }

    public bool CheckIfSegmentIsCloseToCenter()
    {
        GameObject lastSegment = myConnection.gameObject;

        while(true)
        {
            if(lastSegment.GetComponent<RopeSegment>().connectedBelow!=null)
            {
                lastSegment = lastSegment.GetComponent<RopeSegment>().connectedBelow;
            }
            else
            {
                break;
            }
        }
        print(lastSegment.transform.localPosition.x);
        return Mathf.Abs(lastSegment.transform.localPosition.x) < 1;
    }



    void OnTriggerStay2D(Collider2D col)
    {
        if(!attached)
        {
            if(col.tag == "Rope")
            {
                if(attachedTo != col.gameObject.transform.parent)
                {
                    if(disregard == null || col.gameObject.transform.parent.gameObject != disregard && !mvtScript.noChao)
                    {
                        Attach(col.gameObject.GetComponent<Rigidbody2D>());
                    }
                }
            }
        }
    }

}
