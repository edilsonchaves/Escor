using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tartaruga : MonoBehaviour
{
    [SerializeField] private string tagOfPlayer = "Player";
    [SerializeField] private Animator myAnimator;
    [SerializeField] private KeyCode actionButton = KeyCode.W;
    [SerializeField] private GameObject power;
    [SerializeField] private List<Conversa> conversaMestre;
    [SerializeField] ConversaPersonagem conversa;
    bool talking = false;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        conversa = GameObject.FindGameObjectWithTag("ConversaPersonagem").GetComponent<ConversaPersonagem>();
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if(col.tag == tagOfPlayer && Input.GetKey(actionButton) && !talking)
        {
            StartCoroutine(TalkAnimation());
        }
    }


    IEnumerator TalkAnimation()
    {
        talking = true;
        myAnimator.SetBool("Talking", true);
        StartCoroutine(conversa.ConversaFase(conversaMestre));
        yield return new WaitForSeconds(31f);
        DropPower();
        yield return new WaitForSeconds(10f);

        StopTalking();
    }
    public void DropPower()
    {
        Debug.Log("Teste");
        GameObject powerInstantiate = Instantiate(power, new Vector3(transform.position.x + 1, transform.position.y, 0), Quaternion.identity) as GameObject;
        powerInstantiate.GetComponent<PowerScript>().SetPower(PowerOptions.defesa);
    }
    public void StopTalking()
    {
        talking = false;
        myAnimator.SetBool("Talking", false);        
    }
}
