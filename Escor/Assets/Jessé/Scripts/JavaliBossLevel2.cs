using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JavaliBossLevel2 : MonoBehaviour
{

    public GameObject javali, porta, porta2;
    public VcamFocusObject vcam;
    private bool triggered=false;

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        triggered=false;
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag != "Player" || triggered)
            return;

        triggered = true;
        player    = col.gameObject;
        StartCoroutine("StartCutScene");

    }

    IEnumerator StartCutScene()
    {
        yield return new WaitForSeconds(1f);

        Animator animPorta = porta.GetComponent<Animator>();
        animPorta.Play("PortaoFechando");
        // yield return new WaitForSeconds(0.25f); // tempo até levar o susto

        player.GetComponent<Animator>().Play("assustando", -1, 0.1f); // susto
        player.GetComponent<Movement>().LookDirection(180); // olha para a esquerda
        // yield return new WaitForSeconds(0.25f);

        vcam.StartFocus(new GameObject[3]{porta, javali, porta2}, true);
        yield return new WaitUntil(() => ((Vector2)(vcam.currentPosition.position - porta.transform.position)).magnitude <= 0.25f); // espera até o foco chegar perto da porta
        yield return new WaitForSeconds(1f); // tempo de foco na porta
        // yield return new WaitUntil(() => (animPorta.GetCurrentAnimatorStateInfo(0).IsName("PortaoAbrindo"))); // espera a animação mudar para 'PortaoAbrindo'
        // yield return new WaitUntil(() => (animPorta.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.75f)); // espera a animação chegar em 50%
        
        
        player.GetComponent<Animator>().Play("assustando", -1, 0.1f); // susto
        player.GetComponent<Movement>().LookDirection(0); // olha para a direita
        yield return new WaitForSeconds(0.2f); // tempo de foco na porta (total 1.2f) 

        vcam.GoToNextStep(); // vai pro javali

        yield return new WaitUntil(() => ((Vector2)(vcam.currentPosition.position - javali.transform.position)).magnitude <= 0.25f); // espera até o foco chegar perto do javali
        yield return new WaitForSeconds(1.5f); // tempo de foco no javali
        
        vcam.GoToNextStep(); // vai pra porta2
        yield return new WaitUntil(() => ((Vector2)(vcam.currentPosition.position - porta2.transform.position)).magnitude <= 0.25f); // espera até o foco chegar perto da porta2
        yield return new WaitForSeconds(1f); // tempo de foco na porta2

        vcam.GoToNextStep(); // termina
    }

}
