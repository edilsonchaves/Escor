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

    public bool stopPlayer = true;
    public bool stopJavali = true;

    [HideInInspector] public bool keepFocusingTarget;
    [HideInInspector] public bool goInstantly;

    public Transform currentPosition;

    private CinemachineVirtualCamera virtualCam;
    private GameObject objectToFocus;

    Transform  virtualCamTargetBackup;
    GameObject currentTarget;

    bool            nextStep, focusing;
    GameObject      objToSmooth;
    float           distance;
    Vector2         startPos;
    int             idx=0; // index do alvo atual
    GameObject[]    allTargets;


    float c_timeToStartFocus;
    float c_transitionTimeGoing;
    float c_transitionTimeComingBack;
    float c_focusTime;


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

        c_timeToStartFocus          = timeToStartFocus;
        c_transitionTimeGoing       = transitionTimeGoing;
        c_transitionTimeComingBack  = transitionTimeComingBack;
        c_focusTime                 = focusTime;

        // ResetParametersToDefault();
    }


    public void ResetParametersToDefault()
    {
        timeToStartFocus         = c_timeToStartFocus;
        transitionTimeGoing      = c_transitionTimeGoing;
        transitionTimeComingBack = c_transitionTimeComingBack;
        focusTime                = c_focusTime;
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


        // finish                          = false;
        if(focusing)
            return;
            
        allTargets                      = targets;
        focusing                        = true;
        keepFocusingTarget              = false;
        // startPos                        = Camera.main.transform.position;
        startPos                        = virtualCam.Follow.position;
        objToSmooth                     = objToSmooth == null ? new GameObject() : objToSmooth; // objeto que a camera irá seguir
        objToSmooth.transform.position  = startPos;
        currentPosition                 = objToSmooth.transform;
        distance                        = ((Vector2)(allTargets[0].transform.position-objToSmooth.transform.position)).magnitude;
        virtualCamTargetBackup          = virtualCam.Follow;

        // Movement.canMove = !stopPlayer;
        if(stopPlayer)
            Movement.KeepPlayerStopped(); // mantém o player sem se mover

        if(stopJavali)
            JavalisManager.instance.StopMovementOfJavalis(true);

        StartCoroutine(StartFocus_(stepByStep));
    }


    // IEnumerator KeepPlayerStoped()
    // {
    //     while(focusing)
    //     {
    //         Movement.canMove = false;
    //         yield return null;
    //     }
    // }


    IEnumerator StartFocus_(bool stepByStep)
    {
        // yield return new WaitUntil(() => ((Vector2)(Camera.main.transform.position-virtualCam.Follow.position)).magnitude < 0.1f);

        yield return new WaitForSeconds(timeToStartFocus);

        virtualCam.Follow = objToSmooth.transform; // muda o alvo da camera para o objeto que será movido entre os objetos de foco

        for(int c=0; c < allTargets.Length; c++)
        {
            currentTarget   = allTargets[c]; // ponto de foco atual
            distance        = ((Vector2)(currentTarget.transform.position-objToSmooth.transform.position)).magnitude; // usado para calcular a velocidade de movimento da camera
            nextStep        = false;

            // indo para o alvo
            while((((Vector2)(currentTarget.transform.position-objToSmooth.transform.position)).magnitude > 0.1f || keepFocusingTarget) && !nextStep) // espera chegar no alvo
            {
                if(!goInstantly)
                {
                    objToSmooth.transform.position = Vector2.MoveTowards(objToSmooth.transform.position, currentTarget.transform.position, distance/transitionTimeGoing*Time.deltaTime);
                }
                else
                {
                    objToSmooth.transform.position = currentTarget.transform.position;
                }
                yield return null;
            }

            // if(currentTarget.tag == "Gate")
            // {
            //     // yield return new WaitForSeconds(1);
            //     currentTarget.GetComponent<Animator>().Play("PortaoAbrindoStart", -1, 0);
            // }
            // if(currentTarget.tag == "Player")
            // {
            //     // yield return new WaitForSeconds(1);
            //     currentTarget.GetComponent<Animator>().Play("assustando");
            // }

            if(stepByStep)
            {
                yield return new WaitUntil(() => (nextStep)); // espera ficar true para ir para o próximo passo
            }
            else
            {
                yield return new WaitForSeconds(focusTime); // espera o tempo para ir para o próximo passo
            }
        }

        distance = ((Vector2)(objToSmooth.transform.position-virtualCamTargetBackup.position)).magnitude;

        while(((Vector2)(objToSmooth.transform.position-virtualCamTargetBackup.position)).magnitude > 0.1f) // espera chegar no alvo
        {
            objToSmooth.transform.position = Vector2.MoveTowards(objToSmooth.transform.position, virtualCamTargetBackup.position, distance/transitionTimeComingBack*Time.deltaTime);
            yield return null;
        }

        virtualCam.Follow = virtualCamTargetBackup; // reseta a camera para o alvo inicial
        // Movement.canMove  = true;

        if(stopPlayer)
            Movement.StopKeepPlayerStopped(); // faz o player se mover

        if(stopJavali)
            JavalisManager.instance.StopMovementOfJavalis(false);
        focusing          = false;
    }


    public void GoToNextStep(bool instantly=false)
    {
        keepFocusingTarget = false;
        nextStep = true;
        goInstantly = instantly;
    }


}
