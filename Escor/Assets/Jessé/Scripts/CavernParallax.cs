using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CavernParallax : ParallaxEffect
{
    public float m_farMovementSpeed, m_bettewMovementSpeed, m_movementRange;

    // Start is called before the first frame update
    void Start()
    {
        vcam = GameObject.Find("CM vcam1").transform;
        camStartPos = vcam.position;

        farMovementSpeed = m_farMovementSpeed;
        bettewMovementSpeed = m_bettewMovementSpeed;
        cavernMovementRange = m_movementRange;

        // camStartPos = GameObject.Find("CM vcam1").transform.position;
        // camStartPos = Camera.main.transform.position;
        SetABS(); // se certifica que as variáveis sejam positivas
        SetLayers();
    }


    void FixedUpdate()
    {
        // farMovementSpeed = m_farMovementSpeed;
        // bettewMovementSpeed = m_bettewMovementSpeed;
        // cavernMovementRange = m_movementRange;
        // SetABS(); // se certifica que as variáveis sejam positivas
        MoveCavernParallax();
        // TestOfSmoothMoveCavernParallax();
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
