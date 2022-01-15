using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSegment : MonoBehaviour
{
    public GameObject connectedAbove, connectedBelow;
    public bool isPlayerAttached;
    private RopeSegment aboveSegment;

    // Start is called before the first frame update
    void Start()
    {
        ResetAnchor();


    }

    void ResetAnchor()
    {
        connectedAbove = GetComponent<HingeJoint2D>().connectedBody.gameObject;
        aboveSegment = connectedAbove.GetComponent<RopeSegment>();
        if(aboveSegment != null)
        {
            aboveSegment.connectedBelow = gameObject;
            float spriteBottom = connectedAbove.GetComponent<SpriteRenderer>().bounds.size.y-0.1f;
            GetComponent<HingeJoint2D>().connectedAnchor = new Vector2(0, spriteBottom*-1);
        }
        else
        {
            GetComponent<HingeJoint2D>().connectedAnchor = Vector2.zero;
        }
    }

}
