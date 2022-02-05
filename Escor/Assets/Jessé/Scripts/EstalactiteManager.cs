using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EstalactiteManager : MonoBehaviour
{

    public GameObject GotaPrefab, SplashPrefab;
    public Transform shotPoint;
    public float delayMinime = 2f;

    [Range(0,40)]
    public float randomDelayIcrement;

    public bool useRandomChanceToFall;

    [Range(0,100)]
    public float chanceToFall;

    public bool killInstantly=false;

    public string tagOfPlayer = "Player";

    public LayerMask groundLayer;
    
    private GameObject gota, splash, gameObjectColliderToCanFall;
    private Rigidbody2D gotaRB, myRB;
    private CapsuleCollider2D gotaCollider;
    private Animator myAnimator;


    bool splashFinished = true, gotaFall, fall, isFallen, isDestroyed;
    float auxDelay, currentDelay, gotaScale;



    // Start is called before the first frame update
    void Start()
    {
        myAnimator                              = GetComponent<Animator>();
        gameObjectColliderToCanFall             = transform.GetChild(0).transform.GetChild(0).gameObject;


        gota                                    = Instantiate(GotaPrefab, transform.GetChild(0).transform);
        gotaScale                               = gota.transform.lossyScale.x ;
        gota.GetComponent<GotaManager>().scriptOfEstalactite = this;


        if(!(gotaScale > 0.02f && gotaScale < 0.045f))
        {
            gotaScale = gotaScale < 0.02f ? 0.02f : 0.045f;
        }


        gotaRB                                  = gota.GetComponent<Rigidbody2D>();
        gotaCollider                            = gota.GetComponent<CapsuleCollider2D>();
        gota.transform.localScale               = Vector3.zero;
        gotaRB.isKinematic                      = true;
        gotaRB.gravityScale                     = gotaScale/0.06f;


        splash = Instantiate(SplashPrefab, transform.GetChild(0).transform);
        splash.transform.localScale             = Vector3.one*(gotaScale/transform.localScale.x);
        splash.SetActive(false);


        if(useRandomChanceToFall)
        {
            chanceToFall                        = Random.Range(0.0f, 100.0f);
        }


        fall                                    = Random.Range(0.0f, 100.0f) <= chanceToFall;


        if(fall)
        {
            myRB                                = GetComponent<Rigidbody2D>();
            myRB.isKinematic                    = true;
            myRB.gravityScale                   = transform.lossyScale.x/0.8f;
            gameObjectColliderToCanFall.SetActive(true);
            GetComponent<Collider2D>().enabled  = true;
            ConfigureColliderSize();
        }


        ResetGotaPosition();
        currentDelay = GetNewDelay();

    }


    // Update is called once per frame
    void Update()
    {
        if(splashFinished && !isFallen)
        {
            auxDelay                       += Time.deltaTime;

            gota.transform.localScale       = Vector3.one * ((auxDelay/currentDelay*gotaScale)/transform.localScale.x);

            gotaCollider.enabled = false;

            if(auxDelay >= currentDelay)
            {
                gotaCollider.enabled        = true;
                auxDelay                    = 0;
                gota.transform.localScale   = Vector3.one * (gotaScale/transform.localScale.x);
                gota.transform.SetParent(null);
                gotaRB.isKinematic          = false;
                splashFinished              = false;
                gotaFall                    = true;
                currentDelay                = GetNewDelay();
            }
        }

    }


    void ConfigureColliderSize()
    {
        float groundDistance = Physics2D.Raycast((Vector2)transform.GetChild(0).position, -Vector2.up, Mathf.Infinity, groundLayer).distance;
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.GetChild(0).position, -Vector2.up, Mathf.Infinity, groundLayer);

        Vector3 currentSize = gameObjectColliderToCanFall.transform.localScale;
        Vector3 newSize = new Vector3(currentSize.x, groundDistance, currentSize.z);

        gameObjectColliderToCanFall.transform.localScale = newSize/transform.lossyScale.x;
        gameObjectColliderToCanFall.transform.position = transform.GetChild(0).position - transform.up * (groundDistance / 2);
    }


    float GetNewDelay()
    {
        return delayMinime + Random.Range(0.0f, randomDelayIcrement);
    }


    void ResetGotaPosition()
    {
        gota.transform.position = shotPoint.position;
        gota.transform.SetParent(transform.GetChild(0).transform);
    }


    public void ShowSplashAnimation()
    {
        gotaFall                    = false;
        splashFinished              = false;
        gotaRB.isKinematic          = true;
        gotaRB.velocity             = Vector3.zero;
        gota.transform.localScale   = Vector3.zero;
        
        if(gota)
            splash.transform.position   = gota.transform.position;

        StartCoroutine(ShowSplash());
    }


    IEnumerator ShowSplash(float timeToHide=.5f)
    {
        splash.SetActive(true);
        yield return new WaitForSeconds(timeToHide);
        splash.SetActive(false);
        ResetGotaPosition();
        splashFinished = true;

    }


    void DestroyMe()
    {
        Destroy(gameObject, .5f);
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if(fall && !isDestroyed)
        {
            if(!isFallen)
            {
                if(col.tag == tagOfPlayer)
                {
                    myAnimator.Play("EstalactitePreparandoParaCairStart");
                }
            }
            else
            {
                // if(col.tag == "Player" || (col.tag != "Gota" && col.tag != "Estalactite"))
                if(groundLayer == (groundLayer | (1 << col.gameObject.layer)))
                {

                    StartAnimationOfDestroyEstalactite();
                
                    if(col.tag == tagOfPlayer)
                    {
                        // dano no player aqui 

                        Movement playerMovement = col.GetComponent<Movement>();
                        if (!playerMovement.isInvunerable && playerMovement.Life > 0)
                        {

                            playerMovement.Life -= killInstantly ? playerMovement.Life : 1;
                            // Debug.Log("E tome dano");
                        }
                    }
                }
            }
        }
    }


    void DropEstalactite()
    {
        gameObjectColliderToCanFall.SetActive(false);
        isFallen                = true;
        myRB.velocity           = Vector2.zero;
        myRB.isKinematic        = false;
    }


    void StartAnimationOfDestroyEstalactite()
    {
        isDestroyed         = true;
        myRB.isKinematic    = true;
        myRB.velocity       = Vector2.zero;
        myAnimator.Play("EstalactiteDestroyed");
    }

}
