using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{

    protected float[] speedOfLayers;
    protected float farMovementSpeed, bettewMovementSpeed;
    // protected bool moveGameObject, startInCurrentPosition;

    protected List<Material> layersMaterial = new List<Material>();
    protected List<GameObject> layersGameObject = new List<GameObject>();
    protected List<Vector3> layersGameObjectStartPosition = new List<Vector3>();
    protected List<Vector3> layersStartOffset = new List<Vector3>();

    protected Collider2D m_Collider;
    protected Vector3 camStartPos, currentCamPos;
    protected float pixelsPerUnit, cavernMovementRange;
    protected Bounds sptBounds;
    protected Transform vcam;

    protected void SetABS()
    {
        farMovementSpeed    = Mathf.Abs(farMovementSpeed);
        bettewMovementSpeed = Mathf.Abs(bettewMovementSpeed);
        cavernMovementRange = Mathf.Abs(cavernMovementRange);
    }


    virtual protected void SetLayers()
    {
        for(int c=0; c<transform.childCount; c++)
        {
            layersMaterial.Add(transform.GetChild(c).gameObject.GetComponent<SpriteRenderer>().material);
            layersGameObjectStartPosition.Add(transform.GetChild(c).gameObject.transform.position);
            layersGameObject.Add(transform.GetChild(c).gameObject);
            layersStartOffset.Add(transform.GetChild(c).gameObject.GetComponent<SpriteRenderer>().material.GetTextureOffset("_MainTex"));
        }
    }


    protected void MoveCavernParallax()
    {
        currentCamPos = vcam.position; 

        if(((Vector2)(currentCamPos-transform.position)).magnitude < cavernMovementRange)
        {

            for(int c=0; c<layersGameObject.Count; c++)
            {
                Vector3 currentCamMovement              = (currentCamPos - transform.position);
                // Vector3 currentCamMovement              = (currentCamPos - layersGameObjectStartPosition[c]);
                Vector3 offset                          = currentCamMovement * ((layersMaterial.Count-c) * bettewMovementSpeed + farMovementSpeed);
                Vector3 newPos                          = layersGameObjectStartPosition[c] + offset;
                newPos.z                                = layersGameObjectStartPosition[c].z;
                layersGameObject[c].transform.position  = newPos;
            }

        }

    }


    protected void MoveCityParallax()
    {
        currentCamPos = vcam.position; 
        for(int c=0; c<layersMaterial.Count; c++)
        {
            // print(sptBounds);
            Vector3 currentCamMovement = (vcam.position-camStartPos);
            Vector2 offset = currentCamMovement / layersGameObject[c].transform.lossyScale.x /10.08f*pixelsPerUnit/100;
            offset = new Vector2(currentCamMovement.x/sptBounds.extents.x/2/layersGameObject[c].transform.lossyScale.x, currentCamMovement.y/sptBounds.extents.y/2/layersGameObject[c].transform.lossyScale.y);
            offset      -= offset*(c * bettewMovementSpeed + farMovementSpeed)*Time.fixedDeltaTime;
            // offset.y    -= offset.y/1.97f;
            layersMaterial[c].SetTextureOffset("_MainTex", new Vector2(layersStartOffset[c].x-offset.x, layersStartOffset[c].y-offset.y));
            // layersMaterial[c].SetTextureOffset("_MainTex", -offset);
            // layersMaterial[c].SetTextureOffset("_MainTex", new Vector2(-offset.x+layersStartOffset[c].x, offsetY+layersStartOffset[c].y));
        }
    }


    // protected void MoveCityParallaxbackup2()
    // {
    //     currentCamPos = vcam.position; 
    //     for(int c=0; c<layersMaterial.Count; c++)
    //     {
    //         Vector3 currentCamMovement = (currentCamPos-camStartPos);
    //         Vector2 offset = currentCamMovement / layersGameObject[c].transform.lossyScale.x /10.08f*pixelsPerUnit/100;
    //         offset      -= offset*(c * bettewMovementSpeed + farMovementSpeed)*Time.deltaTime;
    //         offset.y    -= offset.y/1.97f;
    //         layersMaterial[c].SetTextureOffset("_MainTex", -offset);
    //         // layersMaterial[c].SetTextureOffset("_MainTex", new Vector2(-offset.x, offset.y));
    //         // layersMaterial[c].SetTextureOffset("_MainTex", new Vector2(-offset.x+layersStartOffset[c].x, offsetY+layersStartOffset[c].y));
    //     }
    // }


    // protected void MoveCityParallaxBackup()
    // {
    //     currentCamPos = vcam.position; 
    //     for(int c=0; c<layersMaterial.Count; c++)
    //     {
    //         Vector3 currentCamMovement = (currentCamPos-layersGameObject[c].transform.position); // efeito parallax apenas na horizontal
    //         Vector2 offset = currentCamMovement / layersGameObject[c].transform.lossyScale.x /12.8f*pixelsPerUnit/100;
    //         offset      -= offset*(c * bettewMovementSpeed + farMovementSpeed);
    //         layersMaterial[c].SetTextureOffset("_MainTex", -offset);
    //     }
    // }



    protected void MoveCloudParallax()
    {
        currentCamPos = vcam.position; 
        // print("speedOfLayers.Length:       "+speedOfLayers.Length);
        for(int c=0; c<layersGameObject.Count; c++)
        {
                // Vector3 currentCamMovement              = (currentCamPos - camStartPos);
            // Vector3 offset                          = currentCamMovement * (c * bettewMovementSpeed + farMovementSpeed) / 85 / layersGameObject[c].transform.lossyScale.x;
                // float offset                            = (c * bettewMovementSpeed + farMovementSpeed) / layersGameObject[c].transform.lossyScale.x;
            // offset                                 += offset**Time.deltaTime;
                // Vector3 newPos                          = (Vector2)layersGameObjectStartPosition[c] + (Vector2)currentCamMovement / offset;
                // print(newPos);
                // newPos.z                                = layersGameObjectStartPosition[c].z;
                // layersGameObject[c].transform.position  = newPos;
            // print(offset);


            Vector3 currentCamMovement              = (currentCamPos - camStartPos);
            Vector3 offset                          = currentCamMovement * speedOfLayers[c];
            Vector3 newPos                          = layersGameObjectStartPosition[c] + currentCamMovement - offset;
            newPos.z                                = layersGameObjectStartPosition[c].z;
            layersGameObject[c].transform.position  = newPos;
        }

    }
}
