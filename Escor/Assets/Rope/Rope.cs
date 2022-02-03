using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    public bool canSwing; 
    public Rigidbody2D hook;
    public GameObject prefabRopeLastSegment;
    public GameObject prefabRopeSegs;
    public int numLinks = 5;
    private PlayerRopeControll rpCon;

    // Start is called before the first frame update
    void Start()
    {
        // rpCon = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerRopeControll>();
        GenerateRope();
    }

    void GenerateRope()
    {
        Rigidbody2D prevBod = hook;
        // GameObject newSeg           = Instantiate(prefabRopeHook);
        // newSeg.transform.parent     = this.transform;
        // newSeg.transform.position   = this.transform.position;
        // HingeJoint2D hj             = newSeg.GetComponent<HingeJoint2D>();
        // hj.connectedBody            = prevBod;
        // prevBod                     = newSeg.GetComponent<Rigidbody2D>();

        for(int i=0; i < numLinks; i++)
        {
            // int index                   = Random.Range(0, prefabRopeSegs.Length);
            GameObject newSeg           = Instantiate(prefabRopeSegs);
            newSeg.transform.parent     = this.transform;
            newSeg.transform.position   = this.transform.position;
            HingeJoint2D hj             = newSeg.GetComponent<HingeJoint2D>();
            hj.connectedBody            = prevBod;
            prevBod                     = newSeg.GetComponent<Rigidbody2D>();
            if(numLinks == i+1)
            {
                newSeg           = Instantiate(prefabRopeLastSegment);
                newSeg.transform.parent     = this.transform;
                newSeg.transform.position   = this.transform.position;
                hj             = newSeg.GetComponent<HingeJoint2D>();
                hj.connectedBody            = prevBod;
                prevBod                     = newSeg.GetComponent<Rigidbody2D>();
                // StartCoroutine(StopSwing(newSeg, newSeg.GetComponent<RopeSegment>(), newSeg.GetComponent<Rigidbody2D>()));
            }
        }
    }


    // IEnumerator StopSwing(GameObject ns, RopeSegment rs, Rigidbody2D rb)
    // {
        // while(true)
        // {
        //     // if(!rpCon.attached)
        //     // {
        //         if(rpCon.attachedTo != transform || !rpCon.attached)
        //         {
        //             float dis = Mathf.Abs(transform.position.x-rs.transform.position.x);
        //             if(dis >= 0.5f)
        //             {
        //                 // print("AddRelativeForce");
        //                 int dir = (int)(rb.velocity.x/Mathf.Abs(rb.velocity.x));
        //                 rb.AddRelativeForce(new Vector2((-Vector2.right.x*dir*Mathf.Abs(rb.velocity.x)), 0f));
        //                 // print(((-Vector2.right*dir*Mathf.Abs(rb.velocity.x))/2));
        //             }
        //             else if(rb.velocity.x > 0.2f)
        //             {
        //                 rb.velocity = new Vector2(rb.velocity.x/1.01f, rb.velocity.y/1.01f);
        //             }
        //         }
        //         // else if(!rpCon.attached)
        //         // {

        //         // }
        //     // }

        //     yield return null;
        // }
    // }
}
