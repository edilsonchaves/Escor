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
    bool isInverting = false;


    void OnEnable()
    {
        // transform.rotation = Quaternion.Euler(0, 0, 0);
        // startRotation      = transform.GetChild(0).GetChild(0).eulerAngles;
        // myRb.isKinematic   = false; // dynamic
        isStopped          = false;
        folhaSumindo       = false;
        bool isInverting   = false;
        myRb.simulated     = true;
        gameObject.name    = "FolhaSolta"; // a animação de kuro derrubando a folha é linkada pelo nome 'FolhaSolta'

        SetSpriteColor();

        if(startWithRandomOffset)
        {
            // myAnim.SetFloat("startOffset", Random.value); // isso causa um BUG na hora de inverter a direção quando bate em uma parede
            myAnim.Play("FolhaBalancando2", -1, Random.value); // isso deve funcionar melhor
        }

        FolhaCaindo();
    }


    public void SetSpriteColor(int colorIdx=-1)
    {
        if(colorIdx != -1)
            colorToUse = colorIdx;
            
        spriteWithColor.color       = colors[colorToUse];
        spriteWithoutColor.color    = grayColors[colorToUse];
    }


    IEnumerator FadeOut()
    {
        FolhaParada(); // folha não se mexe
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

    void OnTriggerEnter2D(Collider2D col)
    {
        // só detecta colisão com o chão, kuro ou a cabeça de kuro
        Debug.DrawLine(transform.GetChild(0).position, col.ClosestPoint(transform.GetChild(0).position), Color.red, 15);

        if(!(col.tag == "ground" || col.tag == "Player" || col.tag == "TopoDaCabecaDeKuro") || folhaSumindo || isInverting || isOnKuroHead)
            return;

        Vector2 colDir = GetDirectionOfContact(col); // a direção pode não ser tão precisa ás vezes
        Debug.DrawLine(transform.GetChild(0).position, (Vector2)transform.GetChild(0).position+colDir, Color.green, 15);

        if(col.tag == "TopoDaCabecaDeKuro") // detecta a colisão sem se importar com a direção
        {
            if(col.transform.parent.GetComponent<CabecaDeKuro>().GetPodeFolhaNaCabeca())    
                StopOnKurosHead(col.transform.parent.gameObject);
        }
        else if(VerifyEquality(colDir,-Vector2.up)) // colisão em baixo da folha
        {

            if(col.tag == "Player") // não faz nada se colidir em cima do player
                return; 

            StartCoroutine("FadeOut"); // folha desligada e sumindo
        }
        else if(!VerifyEquality(colDir, Vector2.up)) // colisão na esquerda ou direita da folha
        {
            InverteDirectionX(); 
        }
        else // colisão em cima da folha
        {
            // TODO
        }

    }

    // verificar se dois vetores são iguais
    bool VerifyEquality(Vector2 a, Vector2 b)
    {
        return (a-b).magnitude < 0.0001f;
    }


    // coloca a folha na cabeça de kuro
    void StopOnKurosHead(GameObject player)
    {
        FolhaParada();
        transform.SetParent(player.transform.Find("TopoDaCabeca"));
        player.GetComponent<Animator>().SetBool("FolhaNaCabeca", true);
        myRb.simulated  = false; 
        isOnKuroHead    = true;
    }

    // inverte a direção da folha
    void InverteDirectionX()
    {
        if(isInverting)
            return;

        isInverting = true;

        myAnim.SetFloat("startOffset", 0); // não to mais usando
        float currentAnimationStep  = AnimationNormalizedTime();
        transform.position          = transform.GetChild(0).position;
        print("58114       "+currentAnimationStep);
        transform.Rotate(0,currentAnimationStep <= 0.5f ? 180 : 0,0); // espelhamento do pai no eixo Y
        myAnim.Play("FolhaBalancando2", -1, 0.0f);

        StartCoroutine(UpdateInvertingVariable());
    }


    float AnimationNormalizedTime()
    {
        float currentAnimationStep  = myAnim.GetCurrentAnimatorStateInfo(0).normalizedTime;
        return currentAnimationStep-Mathf.Floor(currentAnimationStep); //  currentAnimationStep = 0.3 -> 0.3-0 = 0.3
                                                                       //  currentAnimationStep = 1.3 -> 1.3-1 = 0.3
                                                                       //  currentAnimationStep = 3.5 -> 3.5-3 = 0.5

    }



    // verificar de qual direção a folha recebeu contato
    Vector2 GetDirectionOfContact(Collider2D col)
    {
        /*
             left = (-1, 0)
            right = ( 1, 0)
               up = ( 0, 1)
             down = ( 0,-1)
        */

        Vector2 myColPos    = transform.GetChild(0).position;
        Vector2 contactPos  = (col.ClosestPoint(myColPos)-myColPos).normalized;

        if(Mathf.Abs(contactPos.x) >= Mathf.Abs(contactPos.y)) // esquerda ou direita
        {
            if(contactPos.x > 0) // direita
                return Vector2.right;

            return -Vector2.right; // esquerda
        }
        else // cima ou baixo
        {
            if(contactPos.y > 0) // cima
                return Vector2.up;

            return -Vector2.up; // baixo
        }

        return Vector2.zero; // garantia de retorno
    }


    void OnCollisionEnter2D(Collision2D col)
    {
        return;

        if(col.gameObject.tag != "TopoDaCabecaDeKuro")
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
        float yy = transform.localEulerAngles.y;
        transform.SetParent(controller ? controller.transform : null); 
        myAnim.SetFloat("startOffset", 0);
        myAnim.Play("FolhaBalancando2", -1, 0f);
        yield return new WaitUntil(() => (myAnim.GetCurrentAnimatorStateInfo(0).IsName("FolhaBalancando2"))); // espera a animação mudar para 'FolhaBalancando2'
     

        Transform colisor       = transform.GetChild(0);
        transform.position      = colisor.position;
        colisor.position        = Vector3.zero;    

        transform.rotation      = Quaternion.Euler(0, yy, 0);
        FolhaCaindo();
    }


    // só serve mesmo para esperar algum tempo antes de trocar o valor de 'isInverting'
    IEnumerator UpdateInvertingVariable(bool newValue = false, float waitTime = 0.1f)
    {
        yield return new WaitForSeconds(waitTime);
        isInverting = newValue;
    }


    public void TurnOffMe()
    {
        myRb.simulated = false;    

        if(!controller)
            return;

        controller.TurnOffFolha(this);
    }


    void FolhaParada()
    {
        isStopped       = true;
        myRb.simulated  = false;
        myAnim.SetFloat("speed", 0);
    }


    void FolhaCaindo()
    {
        isStopped       = false;
        myRb.simulated  = true;
        isOnKuroHead    = false;
        myAnim.SetFloat("speed", 1);
    }
}
