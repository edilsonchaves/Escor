using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Folha : MonoBehaviour
{

    public Animator       myAnim;
    public Rigidbody2D    myRb;
    public bool           isOnKuroHead,  startWithRandomOffset,  isStopped;
    public List<Color>    colors,  grayColors;
    public int            colorToUse;

    public float timeToStartFadeout = 4, durationOfFadeout = 10;

    public SpriteRenderer spriteWithColor,  spriteWithoutColor;

    public FolhasManager controller;

    Vector3 startRotation;
    bool folhaSumindo = false;


    // Start is called before the first frame update
    // void Start()
    // {

    //     SetSpriteColor();
    //     OnEnable();
    // }

    void OnEnable()
    {
        // transform.rotation = Quaternion.Euler(0, 0, 0);
        // startRotation      = transform.GetChild(0).GetChild(0).eulerAngles;
        // myRb.isKinematic   = false; // dynamic
        folhaSumindo        = false;
        myRb.simulated      = true;
        gameObject.name     = "FolhaSolta"; // a animação de kuro derrubando a folha é linkada pelo nome 'FolhaSolta'

        SetSpriteColor();

        if(startWithRandomOffset)
            myAnim.SetFloat("startOffset", Random.value);
    }


    public void SetSpriteColor(int colorIdx=-1)
    {
        if(colorIdx != -1)
            colorToUse = colorIdx;
            
        spriteWithColor.color       = colors[colorToUse];
        spriteWithoutColor.color    = grayColors[colorToUse];
    }


    // Update is called once per frame
    void Update()
    {
        if(folhaSumindo)
            return;

        isStopped = myRb.velocity.y >= -0.1f; 
     
        if(isStopped)
        {
            FolhaParada();

            if(!isOnKuroHead)
                StartCoroutine("FadeOut");
        }
        else
        {
            FolhaCaindo();
        }
    }


    IEnumerator FadeOut()
    {
        folhaSumindo = true;

        yield return new WaitForSeconds(timeToStartFadeout); // fica uns segundos no chão

        float fadeDuration  = durationOfFadeout,  currentFadeTime = 0f;
        Color withC = spriteWithColor.color,  withoutC = spriteWithoutColor.color;

        while(currentFadeTime < fadeDuration)
        {
            currentFadeTime += Time.deltaTime;

            spriteWithColor.color       = new Color(withC.r, withC.g, withC.b, 1-currentFadeTime/fadeDuration);
            spriteWithoutColor.color    = new Color(withoutC.r, withoutC.g, withoutC.b, 1-currentFadeTime/fadeDuration);

            yield return null;
        }

        spriteWithColor.color       = new Color(withC.r, withC.g, withC.b, 0);
        spriteWithoutColor.color    = new Color(withoutC.r, withoutC.g, withoutC.b, 0);

        TurnOffMe();

    }


    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag != "Player")
            return; // retorna se não for kuro

        if(Mathf.Round(col.contacts[0].normal.y) != 1)
            return; // retorna se a superfice não vier de baixo

        if(!col.gameObject.GetComponent<CabecaDeKuro>().GetPodeFolhaNaCabeca())    
            return; // retorna caso não possa cair folha na cabeça de kuro


        transform.SetParent(col.gameObject.transform.Find("TopoDaCabeca"));
        col.gameObject.GetComponent<Animator>().SetBool("FolhaNaCabeca", true);
        // col.gameObject.GetComponent<Animator>().SetTrigger("FolhaNaCabeca"); // nesse caso não funciona muito bem pq ocorre loop
        myRb.simulated  = false; 
        isOnKuroHead    = true;
    }


    public void DropFromKurosHead()
    {
        // Transform transform   = topoDaCabeca.GetChild(0);
        StartCoroutine("SwitchPosition");
    }


    // o animator.play não muda a animação no exato frame que está,
    // isso causa um teleporte da folha quando eu mudo sua posição
    // por isso eu preciso desse IEnumerator para esperar a animação mudar
    // e só então eu posso mudar a posição
    IEnumerator SwitchPosition()
    {
        transform.SetParent(null); 
        myAnim.SetFloat("startOffset", 0);
        myAnim.Play("FolhaBalancando2", -1, 0f);
        yield return new WaitUntil(() => (myAnim.GetCurrentAnimatorStateInfo(0).IsName("FolhaBalancando2"))); // espera a animação mudar para 'FolhaBalancando2'
     

        Transform colisor       = transform.GetChild(0);
        transform.position      = colisor.position;
        colisor.position        = Vector3.zero;    
        // myAnim.SetFloat("startOffset", 0.01f);
        // myAnim.Play("FolhaBalancando2", -1, 0f);

        transform.rotation      = Quaternion.Euler(0, 0, 0);

        myRb.simulated          = true; 
    }



    public void TurnOffMe()
    {
        myRb.simulated = false;    

        if(!controller)
            return;

        controller.TurnOffFolha(this);
    }



    // void OnCollisionEnter2D(Collision2D col)
    // {
    //     if(col.gameObject.tag == "Player")
    //         isOnKuroHead = false;
    // }


    void FolhaParada()
    {
        myAnim.SetFloat("speed", 0);
    }




    void FolhaCaindo()
    {
        myAnim.SetFloat("speed", 1);
        isOnKuroHead = false;
    }
}
