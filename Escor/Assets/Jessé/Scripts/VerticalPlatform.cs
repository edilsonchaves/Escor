using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalPlatform : MonoBehaviour
{
    private PlatformEffector2D effector;
    private float waitTime;

    bool isDown;

    // Start is called before the first frame update
    void Start()
    {
        effector = transform.GetChild(0).GetComponent<PlatformEffector2D>();
    }


    // Update is called once per frame
    void Update()
    {
        if((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && !isDown)
        {
            isDown = true;
            effector.rotationalOffset = 180f;
            waitTime = 0.4f;
        }
        else if(isDown)
        {
            if(waitTime <= 0)
            {
                effector.rotationalOffset = 0f;
                isDown = false;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }
}
