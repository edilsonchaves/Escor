using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour
{

    public int numberOfClouds;
    public float movementSpeed;
    public bool movementForLeft;
    public Transform limitLeft, limitRight, limitDown;
    public GameObject[] cloudPrefabs;

    private List<Transform> cloudsTrans;
    private List<float> usedPositionsX;

    [HideInInspector]
    public static bool StartUnactived;

    // Start is called before the first frame update
    void Start()
    {
        GenerateClouds(numberOfClouds, 10, false);
    }

    // Update is called once per frame
    void Update()
    {
        MoveClouds();
        MoveCloudToOtherSideAndUpdateLayerOnCavern();
    }   


    void RandomAnimationStap(GameObject cloudGb, int cloudIdx)
    {
        print("cloudIdx  "+cloudIdx);
        cloudGb.GetComponent<Animator>().Play("Nuvem"+cloudIdx+"Loop", -1, Random.Range(0.0f,1.0f));
        // myAnimator.Play("Nuvem");
    }


    void SetCloudsTransform()
    {
        cloudsTrans = new List<Transform>();
        for(int c=0; c<transform.childCount; c++)
        {
            cloudsTrans.Add(transform.GetChild(c));
        }

        OrganizeInCrescentOrder();
    }


    void GenerateClouds(int quant=1, float minDistance=1f, bool useMaxDistance=false)
    {    
        if(quant >= (limitRight.position.x-limitLeft.position.x))
        {
            quant = (int) (limitRight.position.x-limitLeft.position.x)/2;
        }    

        float maxDistance = (limitRight.position.x-limitLeft.position.x)/quant;
        
        // print("A  "+(int)((limitRight.position.x-limitLeft.position.x)/minDistance/1.25f));
        // print("B  "+(maxDistance/10));
        if(quant >= (int)((limitRight.position.x-limitLeft.position.x)/minDistance/1.25f))
        {
            minDistance = maxDistance/10;
        }

        // return;
        // ClearClouds();
        usedPositionsX = new List<float>();

        for(int c=0; c<quant; c++)
        {
            int randomIdx               = Random.Range(0, cloudPrefabs.Length);
            float randomX               = limitLeft.position.x+maxDistance/2+(maxDistance*c);
            float randomY               = Random.Range(transform.position.y, transform.position.y+50*transform.lossyScale.x);

            if(!useMaxDistance)
            {
                randomX                 = Random.Range(limitLeft.position.x, limitRight.position.x);

                while(!CheckIfOutMinimeDistance(randomX, minDistance))
                {
                    randomX             = Random.Range(limitLeft.position.x, limitRight.position.x);
                }
            }

            GameObject tmpCloud         = Instantiate(cloudPrefabs[randomIdx], transform);
            tmpCloud.transform.GetChild(0).gameObject.SetActive(!StartUnactived); // parar a animação, fiz isso porque script de otmização não tava dando conta sozinho 
            tmpCloud.transform.position = new Vector3(randomX, randomY  , transform.position.z);
            
            usedPositionsX.Add(randomX);

        }

        SetCloudsTransform();
    }

    void ClearClouds()
    {
        for(int c=0; c<transform.childCount; c++)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        SetCloudsTransform();
    }


    void HideOnCanvern(Transform cloud)
    {
        cloud.gameObject.SetActive(false);
    }

    void ShowOnOutCavern(Transform cloud)
    {
        cloud.gameObject.SetActive(true);
    }


    bool CheckIfOutMinimeDistance(float posX, float minDistance)
    {
        foreach(float x in usedPositionsX)
        {
            if(posX <= x+minDistance && posX >= x-minDistance)
            {
                return false;
            }
        }
        return true;
    }


    void MoveClouds()
    {
        foreach(Transform trans in cloudsTrans)
        {
            trans.position += (movementForLeft?-1:1)*Vector3.right*movementSpeed*Time.deltaTime*transform.lossyScale.x;
        }
    }


    bool CloudIsOutLimitLeft(Transform cloudTrans)
    {
        if(cloudTrans.position.x < limitLeft.position.x)
        {
            return true;
        }

        return false;
    }


    bool CloudIsOutLimitRight(Transform cloudTrans)
    {
        if(cloudTrans.position.x > limitRight.position.x)
        {
            return true;
        }

        return false;
    }


    bool CloudIsOnCavern(Transform cloudTrans)
    {
        if(cloudTrans.position.y < limitDown.position.y)
        {
            return true;
        }

        return false;
    }

    void MoveCloudToOtherSideAndUpdateLayerOnCavern()
    {
        foreach(Transform trans in cloudsTrans)
        {
            if(CloudIsOutLimitLeft(trans))
            {
                trans.position = new Vector3(limitRight.position.x, trans.position.y, trans.position.z);
            }
            else if(CloudIsOutLimitRight(trans))
            {
                // print("out");
                trans.position = new Vector3(limitLeft.position.x, trans.position.y, trans.position.z);
                // trans.position = limitLeft.position;
            }
            else if(CloudIsOnCavern(trans))
            {
                HideOnCanvern(trans);               
            }
            else
            {
                ShowOnOutCavern(trans);               
            }
        }

    }


    void OrganizeInCrescentOrder()
    {
        List<Transform> newList = new List<Transform>();
        int transCount = cloudsTrans.Count;
        Transform newTrans = cloudsTrans[0];

        while(newList.Count != transCount)
        {

            float minX = cloudsTrans[0].position.x;

            foreach(Transform trans in cloudsTrans)
            {

                if(trans.position.x <= minX)
                {
                    newTrans    = trans;
                    minX        = trans.position.x;
                }

            }

            newList.Add(newTrans);
            cloudsTrans.Remove(newTrans);

        }

        cloudsTrans = newList;

    }


    // TEST --------
    void Test_CheckIfOutMinimeDistance()
    {
        float[] PositionsX = {0, 7, 15, 27, 40};
        usedPositionsX = new List<float>();
        usedPositionsX.Add(0);
        usedPositionsX.Add(4);
        usedPositionsX.Add(6);
        usedPositionsX.Add(-6);
        usedPositionsX.Add(-2);
        usedPositionsX.Add(-10);

        foreach(float x in PositionsX)
        {
            if(CheckIfOutMinimeDistance(x, 5))
            {
                print("OUT :"+x);
            }
            else
            {
                print("IN :"+x);
            }
        }
    }

    void Test_CheckDistance()
    {
        int[] quants = {5, 10, 15, 20, 30, 40, 80, 100};
        float minDistance = 50;

        foreach(int quant in quants)
        {
            string text = "SEM LOOP INFINITO";
            if(quant >= (limitRight.position.x-limitLeft.position.x)/minDistance*2)
            {
                minDistance = (limitRight.position.x-limitLeft.position.x)/quant;
                text = "LOOP INFINITO";

                if(quant >= (limitRight.position.x-limitLeft.position.x)/minDistance*2)
                {
                    text += " | LOOP INFINITO NÂO CORRIGIDO";
                }
                else
                {
                    text += " | LOOP INFINITO CORRIGIDO";
                }    
            }
            print(text);
        }
    }
}

