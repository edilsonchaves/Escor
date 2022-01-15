using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public bool moveGameObject;
    public float farMovementSpeed, bettewMovementSpeed;

    private List<Material> layersMaterial = new List<Material>();
    private List<GameObject> layersGameObject = new List<GameObject>();
    private List<Vector3> layersGameObjectStartPosition = new List<Vector3>();

    Collider2D m_Collider;
    Vector3 camStartPos, currentCamPos;
 

    // Start is called before the first frame update
    void Start()
    {
        camStartPos = Camera.main.transform.position;
        SetABS(); // se certifica que as variáveis sejam positivas
        SetLayers();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        currentCamPos = Camera.main.transform.position;

        if(moveGameObject)
        {
            MoveParallaxGameObject();
        }
        else
        {
            MoveParallaxMaterial();
        }
    }


    void SetABS()
    {
        farMovementSpeed    = Mathf.Abs(farMovementSpeed);
        bettewMovementSpeed = Mathf.Abs(bettewMovementSpeed);
    }


    void SetLayers()
    {
        for(int c=0; c<transform.childCount; c++)
        {
            if(!moveGameObject)
            {
                layersMaterial.Add(transform.GetChild(c).gameObject.GetComponent<SpriteRenderer>().material);
            }
            else
            {
                layersGameObjectStartPosition.Add(transform.GetChild(c).gameObject.transform.position);
            }

            layersGameObject.Add(transform.GetChild(c).gameObject);
        }
    }


    void MoveParallaxGameObject()
    {
        for(int c=0; c<layersGameObject.Count; c++)
        {
            Vector3 currentCamMovement              = (currentCamPos - layersGameObjectStartPosition[c]);
            Vector3 offset                          = currentCamMovement * (c * bettewMovementSpeed + farMovementSpeed);
            Vector3 newPos                          = layersGameObjectStartPosition[c] - offset;
            newPos.z                                = layersGameObjectStartPosition[c].z;
            layersGameObject[c].transform.position  = newPos;
        }

    }


    void MoveParallaxMaterial()
    {
        // Vector3 currentCamMovement = (currentCamPos-camStartPos); // efeito parallax apenas na horizontal

        for(int c=0; c<layersMaterial.Count; c++)
        {
            Vector3 currentCamMovement = (currentCamPos-layersGameObject[c].transform.position); // efeito parallax apenas na horizontal
            Vector2 offset = currentCamMovement / layersGameObject[c].transform.lossyScale.x /20.935f;
            offset      -= offset*(c * bettewMovementSpeed + farMovementSpeed);
            layersMaterial[c].SetTextureOffset("_MainTex", -offset);
        }
    }


    void MoveParallaxMaterialBackup()
    {
        float currentCamMovement = (currentCamPos-camStartPos).x; // efeito parallax apenas na horizontal

        for(int c=0; c<layersMaterial.Count; c++)
        {
            float offset = transform.TransformPoint(new Vector2(currentCamMovement, 0)).x / layersGameObject[c].transform.lossyScale.x /20.935f;
            offset      -= offset*(c * bettewMovementSpeed + farMovementSpeed);
            layersMaterial[c].SetTextureOffset("_MainTex", new Vector2(-offset, 0));
        }
    }


    // não é mais útil
    private bool IsInside()
    {
        foreach(Vector3 pos in GetCameraBounds(20))
        {
            if (m_Collider.OverlapPoint(pos))
                return true;
        }

        return false;
    }


    // não é mais útil
    private List<Vector3> GetCameraBounds(float extraArea = 0f)
    {
        float width = Screen.width, height = Screen.height;

        // por algum motivo as posições ficam um pouco diferentes
        // por isso estou compensando com essas variáveis
        const float X_Compensator = -5.7f, Y_Compensator = +2.5f; 

        (float, float)[] corneBounds = {(-extraArea,-extraArea),(width + extraArea,-extraArea),(width + extraArea,height + extraArea),(-extraArea,height + extraArea)};

        List<Vector3> cameraBoundsPos = new List<Vector3>();

        foreach((float, float) cornePos in corneBounds)
        {
            Vector3 corne = Camera.main.ScreenToWorldPoint(new Vector3(cornePos.Item1 - currentCamPos.x + X_Compensator, cornePos.Item2 - currentCamPos.y + Y_Compensator,0));
            cameraBoundsPos.Add(corne);
        }

        Debug.DrawLine(cameraBoundsPos[0], cameraBoundsPos[3], Color.green);
        for(int c=0; c<3; c++)
        {
            Debug.DrawLine(cameraBoundsPos[c], cameraBoundsPos[c+1], Color.green);
        }

        return cameraBoundsPos;
    }

}
