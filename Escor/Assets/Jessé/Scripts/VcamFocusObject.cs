using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VcamFocusObject : MonoBehaviour
{

    // ainda falta fazer o tempo parar enquanto a camera de movementa


    // variáveis de controle ----------------------
    public float timeToStartFocus           = 0.5f;
    public float transitionTimeGoing        = 1;
    public float transitionTimeComingBack   = 0.7f;
    public float focusTime                  = 1;
    // --------------------------------------------


    private CinemachineVirtualCamera virtualCam;
    private GameObject objectToFocus;

    Transform  virtualCamTargetBackup;
    GameObject currentTarget;

    bool            nextStep;
    GameObject      objToSmooth;
    float           distance;
    Vector2         startPos;
    int             idx=0; // index do alvo atual
    GameObject[]    allTargets;



    // Start is called before the first frame update
    void Start()
    {
        try
        {
            virtualCam = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        }
        catch
        {
            Debug.LogWarning("Não foi possível encontrar a virtual camera Cinemachine");
        }
    }


    public void StartFocus(GameObject[] targets, bool stepByStep=true)
    {
        // mesmo que só exista 1 alvo para o foco, o mesmo deve está dentro de um GameObject[]
        // Exemplo:
        //      ...
        //      StartFocus(new GameObject[]{alvo})
        //      ...

        // stepByStep=true  significa que você vai usar GoToNextStep() para guiar a camera passo a passo entre os pontos de foco
        // stepByStep=false significa que será usado 'focusTime' para guiar a camera entre os pontos de foco


        finish                          = false;
        allTargets                      = targets;
        startPos                        = Camera.main.transform.position;
        objToSmooth                     = objToSmooth == null ? new GameObject() : objToSmooth; // objeto que a camera irá seguir
        objToSmooth.transform.position  = Camera.main.transform.position;
        distance                        = ((Vector2)(allTargets[0].transform.position-objToSmooth.transform.position)).magnitude;
        virtualCamTargetBackup          = virtualCam.Follow;

        StartCoroutine(StartFocus_(stepByStep));
    }


    IEnumerator StartFocus_(bool stepByStep)
    {
        yield return new WaitForSeconds(timeToStartFocus);

        virtualCam.Follow = objToSmooth.transform; // muda o alvo da camera para o objeto que será movido entre os objetos de foco

        for(int c=0; c < allTargets.Length; c++)
        {
            currentTarget   = allTargets[c]; // ponto de foco atual
            distance        = ((Vector2)(currentTarget.transform.position-objToSmooth.transform.position)).magnitude; // usado para calcular a velocidade de movimento da camera
            nextStep        = false;

            // indo para o alvo
            while(((Vector2)(currentTarget.transform.position-objToSmooth.transform.position)).magnitude > 0.01f) // espera chegar no alvo
            {
                objToSmooth.transform.position = Vector2.MoveTowards(objToSmooth.transform.position, currentTarget.transform.position, distance/transitionTimeGoing*Time.deltaTime);
                yield return null;
            }


            if(stepByStep)
            {
                yield return new WaitUntil(() => (nextStep)); // espera ficar true para ir para o próximo passo
            }
            else
            {
                yield return new WaitForSeconds(focusTime); // espera o tempo para ir para o próximo passo
            }
        }

        distance = ((Vector2)(objToSmooth.transform.position)-startPos).magnitude;

        while(((Vector2)(objToSmooth.transform.position)-startPos).magnitude > 0.01f) // espera chegar no alvo
        {
            objToSmooth.transform.position = Vector2.MoveTowards(objToSmooth.transform.position, startPos, distance/transitionTimeComingBack*Time.deltaTime);
            yield return null;
        }

        virtualCam.Follow = virtualCamTargetBackup; // reseta a camera para o alvo inicial
    }


    public void GoToNextStep()
    {
        nextStep = true;
    }


}
