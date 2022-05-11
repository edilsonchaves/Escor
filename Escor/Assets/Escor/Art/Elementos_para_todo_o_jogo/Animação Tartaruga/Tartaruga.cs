using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tartaruga : MonoBehaviour
{
    [SerializeField] private string             tagOfPlayer     = "Player";
    [SerializeField] private Animator           myAnimator;
    [SerializeField] private KeyCode            actionButton    = KeyCode.W;
    [SerializeField] private GameObject         power;
    [SerializeField] private List<Conversa>     conversaMestre;
    [SerializeField]         ConversaPersonagem conversa;

    bool talking = false;

    bool canSkip = false; // o dialogo só irá ser pulado caso o player morra no nível 2 e retorne para esse mesmo nível
                          // caso tenha passado pelo dialogo e retorne ao menu e dê um play, o dialago estará lá novamente

    // Start is called before the first frame update
    void Start()
    {
        myAnimator  = GetComponent<Animator>();
        conversa    = GameObject.FindGameObjectWithTag("ConversaPersonagem").GetComponent<ConversaPersonagem>();
        canSkip     = PlayerPrefs.GetInt("SkipConversationOfTurtle", 0) == 1; // é definido como 0 no menu/selecão de níveis
                                                                              // é definido como 1 na morte de Kurô

        // caso deva pular o diálogo, o poder já estará na cena
        if(canSkip)
            DropPower();
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag != tagOfPlayer)
            return;

        if(talking)
            return;

        if(canSkip)
            return;

        StartCoroutine(TalkAnimation());
    }


    IEnumerator TalkAnimation()
    {
        talking = true;
        myAnimator.SetBool("Talking", true);
        StartCoroutine(conversa.ConversaFase(conversaMestre));
        yield return new WaitForSeconds(31f);
        DropPower();
        // yield return new WaitForSeconds(10f);
        yield return new WaitUntil(() => conversa.StatusConversa); //[Jessé] espera até que o diálogo chegue ao fim

        StopTalking();
    }

    // [Jessé]
    public void DropPower()
    {
        power.SetActive(true);
    }


    // public void DropPower()
    // {
    //     // Debug.Log("Teste");
    //     GameObject powerInstantiate = Instantiate(power, new Vector3(transform.position.x + 1, transform.position.y, 0), Quaternion.identity) as GameObject;
    //     powerInstantiate.GetComponent<PowerScript>().SetPower(PowerOptions.defesa);
    // }

    public void StopTalking()
    {
        myAnimator.SetBool("Talking", false);
    }
}
