using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudParallax : ParallaxEffect
{
    public float[] m_speedOfLayers;
    
    // Start is called before the first frame update
    void Start()
    {
        vcam = GameObject.Find("CM vcam1").transform;
        camStartPos = vcam.position;



        

        camStartPos = GameObject.Find("CM vcam1").transform.position;
        speedOfLayers = m_speedOfLayers;
        // camStartPos = Camera.main.transform.position;
        
        // camStartPos = GameObject.Find("CM vcam1").transform.position;
        SetABS(); // se certifica que as variáveis sejam positivas
        SetLayers();
    }


    // Update is called once per frame
    void Update()
    {
        MoveCloudParallax();
    }


    override protected void SetLayers()
    {
        // print("TEST");
        for(int c=0; c<transform.childCount; c++)
        {
            layersGameObjectStartPosition.Add(transform.GetChild(c).gameObject.transform.position);
            layersGameObject.Add(transform.GetChild(c).gameObject);
        }
    }

}
