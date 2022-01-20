using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CavernParallax : ParallaxEffect
{
    public float m_farMovementSpeed, m_bettewMovementSpeed;

    // Start is called before the first frame update
    void Start()
    {
        farMovementSpeed = m_farMovementSpeed;
        bettewMovementSpeed = m_bettewMovementSpeed;
        camStartPos = Camera.main.transform.position;
        SetABS(); // se certifica que as variáveis sejam positivas
        SetLayers();
    }


    // Update is called once per frame
    void Update()
    {
        MoveCavernParallax(); 
    }

    override protected void SetLayers()
    {
        for(int c=0; c<transform.childCount; c++)
        {
            layersGameObjectStartPosition.Add(transform.GetChild(c).gameObject.transform.position);
            layersGameObject.Add(transform.GetChild(c).gameObject);
        }
    }
}
