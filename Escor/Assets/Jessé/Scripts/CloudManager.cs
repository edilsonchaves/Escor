using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour
{

//
//  Usar um valor muito alto para núvens ou distancia mínina ou uma combinação de ambos
//  pode ocasionar em um loop infinito
//
//  OBS: Eu tomei algumas medidas de precaução para evitar isso, então deve ficar tudo bem. 
//       Mas nunca se sabe né :)  ...
//

    public int numberOfClouds;
    public float minimeDistance=10, movementSpeed;
    public bool movementForLeft;
    public Transform limitLeft, limitRight, limitDown, limitUp;
    public GameObject[] cloudPrefabs;

    private List<Transform> cloudsTrans;
    private List<Vector2> usedPositions;

    [HideInInspector]
    public static bool StartUnactived;

    private bool stopLoop=false;



    //for debug

        [Header("DEBUG")]
        public bool changeColor;
        public Color color;
    
    //---------------    
    

    // Start is called before the first frame update
    void Start()
    {
        // StartCoroutine("StopInfiniteLoop");

        // int sizeX = (int)(limitRight.position.x-limitLeft.position.x);
        // int sizeY = (int)(limitUp.position.y-limitDown.position.y);
        // int pixelsCount = (int)(sizeX*sizeY);
        // print("pixelsCount:   "+pixelsCount);

        // if(minDistance <= 0)
        // {
        //     minDistance = 1;
        // }



        GenerateClouds(numberOfClouds, minimeDistance, false);
    }

    IEnumerator StopInfiniteLoop()
    {
        yield return new WaitForSeconds(10);
        stopLoop = true;
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

        int sizeX = (int)(limitRight.position.x-limitLeft.position.x);
        int sizeY = (int)(limitUp.position.y-limitDown.position.y);

        if(minDistance <= 0)
        {
            minDistance = 1;
        }

        int pixelsCount = (int)(sizeX*sizeY);

        // float maxDistance = Mathf.Floor(pixelsCount/quant*(minDistance+1));
        float maxDistance = pixelsCount/minDistance/quant/2;
        // float maxDistance = Mathf.Floor(pixelsCount/quant/2);

        float mediaOfSize = (sizeX + sizeY) / 2;
        // print("pixelsCount:   "+pixelsCount);
            // print("old quant: "+quant);
            // print("old maxDistance: "+maxDistance);
            // print("old minDistance: "+minDistance);

        if(minDistance > maxDistance)
        {
            minDistance = maxDistance/1.5f;
            maxDistance = pixelsCount/minDistance/quant/2;
        }

        if(quant >= pixelsCount || maxDistance <= 2.5f)
        {
            // print("Vai dá não mermão tu tá maluco");
            // print("old quant: "+quant);
            // print("old maxDistance: "+maxDistance);
            // print("old minDistance: "+minDistance);
            quant = (int) pixelsCount/8;
            maxDistance = pixelsCount/quant/2;
            // maxDistance = 1;
            minDistance = maxDistance/5;
            // print("new quant: "+quant);
            // print("new maxDistance: "+maxDistance);
            // print("new minDistance: "+minDistance);
            // print("Agora sim :)");
        }    



        // return;





// ---------------------------------------------------

        // if(quant >= (limitRight.position.x-limitLeft.position.x))
        // {
        //     quant = (int) (limitRight.position.x-limitLeft.position.x)/2;
        // }    

        // float maxDistance = (limitRight.position.x-limitLeft.position.x)/quant;
        
        // print("A  "+(int)((limitRight.position.x-limitLeft.position.x)/minDistance/1.25f));
        // print("B  "+(maxDistance/10));
        // if(quant >= (int)((limitRight.position.x-limitLeft.position.x)/minDistance/1.25f))
        // {
        //     minDistance = maxDistance/10;
        // }

// ---------------------------------------------------
        
        // int sizeX = (int)(limitRight.position.x-limitLeft.position.x);
        // int sizeY = (int)(limitUp.position.y-limitDown.position.y);

        // int maxSize = sizeY > sizeX ? sizeY : sizeX; 


        // float mediaOfSize = (sizeX + sizeY) / 2;

        // if(sizeY > sizeX)
        // {
        //     if(sizeY % 2 == 0)
        //     {
        //         sizeY -= 1; 
        //     }

        //     maxSize = sizeY;
        // }
        // else
        // {
        //     if(sizeX % 2 == 0)
        //     {
        //         sizeX -= 1; 
        //     }
        //     maxSize = sizeX;

        // }
        

        // int pixelsCount = (int)(sizeX*sizeY);


        // if(quant >= pixelsCount)
        // {
        //     quant = (int) pixelsCount/4;
        // }    

        // float maxDistance = Mathf.Ceil(Mathf.Ceil(sizeX/2)*Mathf.Ceil(sizeY/2) / quant / (mediaOfSize**0.5)); //12=3.4641 //11=3.3166 //8=2.8284 //7=2.6458 //6=2.4495 
        // // float maxDistance = pixelsCount / quant / (mediaOfSize**0.5); //12=3.4641 //11=3.3166 //8=2.8284 //7=2.6458 //6=2.4495 
        
        // if(minDistance > maxDistance)
        // {
        //     minDistance = maxDistance;
        // }

        // else if(maxDistance < 1)
        // {
        //     quant = pixelsCount/4;
        //     maxDistance = 1;
        //     minDistance = 1;
        // }

        // else
        // {
        //     if(quant >= (int)((limitRight.position.x-limitLeft.position.x)*(limitUp.position.y-limitDown.position.y)/(minDistance+1)/1.25f))
        //     {
        //         minDistance = maxDistance/10;
        //     }

        // }
        // print("A  "+(int)((limitRight.position.x-limitLeft.position.x)/minDistance/1.25f));
        // print("B  "+(maxDistance/10));

        // ClearClouds();
        usedPositions = new List<Vector2>();

        bool stopLoop = false;

        for(int c=0; c<quant; c++)
        {
            if(!stopLoop)
            {

                float timePassed=0;

                int randomIdx               = Random.Range(0, cloudPrefabs.Length);
                float randomX               = limitLeft.position.x+maxDistance/2+(maxDistance*c);
                float randomY               = Random.Range(transform.position.y, limitUp.position.y);
                Vector2 posi = new Vector2(randomX, randomY);

                if(!useMaxDistance)
                {
                    randomX                 = Random.Range(limitLeft.position.x, limitRight.position.x);
                    randomY               = Random.Range(transform.position.y, limitUp.position.y);

                    while(!CheckIfOutMinimeDistance(new Vector2(randomX, randomY), minDistance) && !stopLoop)
                    {   
                        timePassed += 2f;
                        
                        if(timePassed >= 10000000)
                        {
                            stopLoop = true;
                            print("LOOP INFINITO");
                        }

                        randomX             = Random.Range(limitLeft.position.x, limitRight.position.x);
                        randomY               = Random.Range(transform.position.y, limitUp.position.y);
                    }
                }

                GameObject tmpCloud         = Instantiate(cloudPrefabs[randomIdx], transform);
                if(changeColor)
                {
                    tmpCloud.transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;
                }

                tmpCloud.transform.GetChild(0).gameObject.SetActive(!StartUnactived); // parar a animação, fiz isso porque script de otmização não tava dando conta sozinho 
                tmpCloud.transform.position = new Vector3(randomX, randomY  , transform.position.z);
                
                usedPositions.Add(new Vector2(randomX, randomY));
            }

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


    bool CheckIfOutMinimeDistance(Vector2 pos, float minDistance)
    {
        foreach(Vector2 xy in usedPositions)
        {
            // if((pos.x >= xy.x && pos.x <= xy.x+minDistance) || (pos.y <= xy.y-minDistance && pos.y >= xy.y))
            if(pos.x <= xy.x+minDistance && pos.x >= xy.x-minDistance && pos.y <= xy.y+minDistance && pos.y >= xy.y-minDistance)
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
    // void Test_CheckIfOutMinimeDistance()
    // {
    //     float[] PositionsX = {0, 7, 15, 27, 40};
    //     usedPositionsX = new List<float>();
    //     usedPositionsX.Add(0);
    //     usedPositionsX.Add(4);
    //     usedPositionsX.Add(6);
    //     usedPositionsX.Add(-6);
    //     usedPositionsX.Add(-2);
    //     usedPositionsX.Add(-10);

    //     foreach(float x in PositionsX)
    //     {
    //         if(CheckIfOutMinimeDistance(x, 5))
    //         {
    //             print("OUT :"+x);
    //         }
    //         else
    //         {
    //             print("IN :"+x);
    //         }
    //     }
    // }

//     void Test_CheckDistance()
//     {
//         int[] quants = {5, 10, 15, 20, 30, 40, 80, 100};
//         float minDistance = 50;

//         foreach(int quant in quants)
//         {
//             string text = "SEM LOOP INFINITO";
//             if(quant >= (limitRight.position.x-limitLeft.position.x)/minDistance*2)
//             {
//                 minDistance = (limitRight.position.x-limitLeft.position.x)/quant;
//                 text = "LOOP INFINITO";

//                 if(quant >= (limitRight.position.x-limitLeft.position.x)/minDistance*2)
//                 {
//                     text += " | LOOP INFINITO NÂO CORRIGIDO";
//                 }
//                 else
//                 {
//                     text += " | LOOP INFINITO CORRIGIDO";
//                 }    
//             }
//             print(text);
//         }
//     }
}

