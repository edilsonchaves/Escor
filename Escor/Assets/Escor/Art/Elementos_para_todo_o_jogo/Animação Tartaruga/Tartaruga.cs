using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tartaruga : MonoBehaviour
{
    [SerializeField] private string tagOfPlayer = "Player";
    [SerializeField] private Animator myAnimator;
    [SerializeField] private KeyCode actionButton = KeyCode.W;
    [SerializeField] private GameObject power;
    bool talking = false;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
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
        yield return new WaitForSeconds(3f);
        StopTalking();
    }
    public void StopTalking()
    {
        talking = false;
        myAnimator.SetBool("Talking", false);
        GameObject powerInstantiate= Instantiate(power, new Vector3(transform.position.x+1, transform.position.y, 0),Quaternion.identity) as GameObject;
        powerInstantiate.GetComponent<PowerScript>().SetPower(PowerOptions.defesa);
    }
}
