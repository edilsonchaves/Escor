using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityParallax : ParallaxEffect
{
    public float m_farMovementSpeed, m_bettewMovementSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        vcam = GameObject.Find("CM vcam1").transform;
        camStartPos = vcam.position;
        sptBounds = transform.GetChild(0).GetComponent<SpriteRenderer>().sprite.bounds;
        farMovementSpeed = m_farMovementSpeed;
        bettewMovementSpeed = m_bettewMovementSpeed;
        pixelsPerUnit = transform.GetChild(0).GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
        camStartPos = Camera.main.transform.position;
        SetABS(); // se certifica que as variáveis sejam positivas
        SetLayers();
    }


    // Update is called once per frame
    void LateUpdate()
    {
        MoveCityParallax();
    }

    // override protected void SetLayers()
    // {
    //     for(int c=0; c<transform.childCount; c++)
    //     {
    //         layersMaterial.Add(transform.GetChild(c).gameObject.GetComponent<SpriteRenderer>().material);
    //         layersGameObject.Add(transform.GetChild(c).gameObject);
    //         layersStartOffset.Add(transform.GetChild(c).gameObject.GetComponent<SpriteRenderer>().material.GetTextureOffset("_MainTex"));
    //     }
    // }
}
