using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossJavali2CutScene : MonoBehaviour
{
    public int lifesOfJavali=3;
    public BoxCollider2D boxCol1, boxCol2, boxCol3;

    public GameObject kuroJogadaLonge;

    public GameObject javali, javali2, porta, porta2;
    public VcamFocusObject vcam;

    GameObject player;
    JavaliTiro javaliTiro;
    bool javaliDesativado;

    int currentStep=1;

    // Start is called before the first frame update
    void Start()
    {
        currentStep         = 1;
        boxCol1.enabled     = true;
        boxCol2.enabled     = false;
        boxCol3.enabled     = false;
        javaliTiro          = javali.GetComponent<JavaliTiro>();
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag != "Player")
            return;

        if(!player)
            player  = col.gameObject;

        switch(currentStep)
        {
            case 1:
                StartCoroutine("StartCutSceneStep1");
                break;
            case 2:
                StartCoroutine("StartCutSceneStep2");
                break;
            case 3:
                StartCoroutine("StartCutSceneStep3");
                break;
        }

        // triggered = true;
        // StartCoroutine("StartCutScene");

    }

    void Update()
    {
        if(javaliTiro.attacksReceived >= lifesOfJavali && !javaliDesativado)
        {
            javaliDesativado = true;
            StartCoroutine(TurnOffJavali_());
            StartCoroutine(Win());
            // OpenDoors();
        }
    }


    void OpenDoors()
    {
        porta.GetComponent<Animator>().Play("PortaoAbrindoDeCima");
        porta2.GetComponent<Animator>().Play("PortaoAbrindoDeCima");
    }


    void TurnOffJavali()
    {
        javaliTiro.JavaliAnimator.Play("JavaliTontoLoop");
        javaliTiro.Move             = false;
        javaliTiro.activateAttack   = false;
        javaliTiro.GetComponent<Rigidbody2D>().isKinematic = true;
        javaliTiro.StopAllCoroutines();
        
        foreach(Collider2D col in javaliTiro.GetComponents<Collider2D>())
            col.enabled = false;

        javaliTiro.enabled          = false;
    }

    IEnumerator Win()
    {
        PararPlayer();
        player.GetComponent<Movement>().LookDirection(0); // olha para a direita
        vcam.ResetParametersToDefault();
        vcam.transitionTimeGoing = 0.2f;
        vcam.StartFocus(new GameObject[2]{javali, porta2}, true);
        yield return new WaitForSeconds(0.2f+3f); // 3 de foco no javali
        vcam.transitionTimeGoing = 0.6f;
        vcam.GoToNextStep();
        yield return new WaitForSeconds(1.2f); // tempo ate abrir as portas 
        OpenDoors();
        yield return new WaitForSeconds(2f); // 3 de foco na porta
        vcam.GoToNextStep();
        MoverPlayer();
    }

    IEnumerator TurnOffJavali_()
    {
        yield return new WaitForSeconds(1f);
        TurnOffJavali();
        yield return new WaitUntil(() => !javaliTiro.enabled);
        // this.enabled = false; // desligar esse script
    }


    // fecha a primeira porta e Kurô toma um susto
    IEnumerator StartCutSceneStep1()
    {
        // Movement.KeepPlayerStopped();
        // PararPlayer();
        // yield return new WaitForSeconds(0.1f);

        Animator animPorta = porta.GetComponent<Animator>();
        animPorta.Play("PortaoFechandoDeCima");
        vcam.timeToStartFocus = 0.5f;
        vcam.transitionTimeGoing = 0.3f;
        vcam.transitionTimeComingBack = 0.3f;
        vcam.StartFocus(new GameObject[1]{porta}, true);
        yield return new WaitForSeconds(0.3f); // tempo até levar o susto

        player.GetComponent<Animator>().Play("assustando", -1, 0.2f); // susto
        player.GetComponent<Movement>().LookDirection(180); // olha para a esquerda
        yield return new WaitForSeconds(1.5f); // tempo de foco na porta
        vcam.GoToNextStep();

        // javali.GetComponent<JavaliTiro>().Move = false; // quando a camera volta pro player está fazendo o javali se mover

        currentStep         = 2;
        boxCol1.enabled     = false;
        boxCol2.enabled     = true;

        // Movement.StopKeepPlayerStopped();
        // MoverPlayer();

        yield return null;
    }

    void PararPlayer()
    {
        Movement.KeepPlayerStopped();
    }

    void MoverPlayer()
    {
        Movement.StopKeepPlayerStopped();
    }


    IEnumerator StartCutSceneStep2()
    {
        /*
            1 - player para
            2 - camera vai pro javali
            3 - javali vem andando até perto do player
            4 - javali para perto do player
            5 - camera vai pra porta atrás do javali
            6 - porta fecha atrás do javali
            7 - player toma susto
            8 - camera volta para o player
        */

        vcam.ResetParametersToDefault();
        PararPlayer(); // 1
        // player.transform.position = kuroJogadaLonge.transform.position;
        vcam.transitionTimeGoing = 0.5f;
        vcam.StartFocus(new GameObject[3]{javali, kuroJogadaLonge, porta2}, true); // 2
        vcam.keepFocusingTarget = true;
        javaliTiro.activateAttack = false;
        javaliTiro.Move = true; // 3
        javaliTiro.MovementSpeed = 4;
        javaliTiro.JavaliAnimator.SetFloat("WalkSpeed", 1.5f);

        yield return new WaitUntil(() => ((Vector2)(javali.transform.position - kuroJogadaLonge.transform.position)).magnitude < 15); // esperar o javali chegar perto do player
        // vcam.transitionTimeGoing = 0.5f;
        player.GetComponent<SpriteRenderer>().enabled = false;
        kuroJogadaLonge.SetActive(true);
        player.GetComponent<Animator>().Play("parado");
        // vcam.keepFocusingTarget = false;
        print("_> já");
        vcam.GoToNextStep(false); // vai pro player
        vcam.keepFocusingTarget = true;
        yield return new WaitUntil(() => (Mathf.Abs(javali.transform.position.x - kuroJogadaLonge.transform.position.x) <= 5f)); // esperar o javali chegar perto do player
        javaliTiro.shootForce = 10f; // 4
        javaliTiro.Move = false; // 4
        javaliTiro.activateAttack = true;
        javaliTiro.JavaliAnimator.Play("JavaliAtacando2", -1, 0.3f);
        SfxManager.PlaySound(SfxManager.Sound.javaliAttack);
        // vcam.transitionTimeGoing = 0.0f;
        yield return new WaitForSeconds(0.2f); // tempo de ataque do javali
        print("_> passou 0.2 seg");
        // player.GetComponent<Animator>().Play("KuroJogadaLonge", -1, 0);
        kuroJogadaLonge.GetComponent<Animator>().Play("KuroJogadaLonge", -1, 0);
        yield return new WaitForSeconds(0.25f); // tempo de foco em KuroJogadaLonge
        SfxManager.PlayRandomHurt();
        yield return new WaitForSeconds(1.25f); // tempo de foco em KuroJogadaLonge
        print("_> passou 0.5 seg");
        vcam.ResetParametersToDefault();
        vcam.transitionTimeGoing = 0.3f;
        vcam.GoToNextStep(); // 5 vai pra porta
        print("_> indo pra porta");
        yield return new WaitForSeconds(0.75f); // tempo ate fechar a porta
        print("_> passou 0.75 seg");
        // player.GetComponent<SpriteRenderer>().enabled = false;
        porta2.GetComponent<Animator>().Play("PortaoFechandoDeCima", -1, 0);
        yield return new WaitForSeconds(1.2f); // tempo de foco na porta
        print("_> passou 1.2 seg");
        vcam.GoToNextStep(); // 5 termina
        player.GetComponent<SpriteRenderer>().enabled = true;
        kuroJogadaLonge.SetActive(false);
        player.transform.position = kuroJogadaLonge.transform.position;
        javali.GetComponent<JavaliTiro>().Move = true; // 4
        MoverPlayer();
        boxCol2.enabled     = false;


        currentStep         = 3;
        boxCol3.enabled     = true;

        // yield return new WaitUntil(() => ((Vector2)(vcam.currentPosition.position - porta.transform.position)).magnitude <= 0.25f); // espera até o foco chegar perto da porta
        // yield return new WaitForSeconds(1f); // tempo de foco na porta
        // yield return new WaitUntil(() => (animPorta.GetCurrentAnimatorStateInfo(0).IsName("PortaoAbrindo"))); // espera a animação mudar para 'PortaoAbrindo'
        // yield return new WaitUntil(() => (animPorta.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.75f)); // espera a animação chegar em 50%


        // player.GetComponent<Animator>().Play("assustando", -1, 0.1f); // susto
        // player.GetComponent<Movement>().LookDirection(0); // olha para a direita
        // yield return new WaitForSeconds(0.2f); // tempo de foco na porta (total 1.2f)

        // vcam.GoToNextStep(); // vai pro javali

        // yield return new WaitUntil(() => ((Vector2)(vcam.currentPosition.position - javali.transform.position)).magnitude <= 0.25f); // espera até o foco chegar perto do javali
        // yield return new WaitForSeconds(1.5f); // tempo de foco no javali

        // vcam.GoToNextStep(); // vai pra porta2
        // yield return new WaitUntil(() => ((Vector2)(vcam.currentPosition.position - porta2.transform.position)).magnitude <= 0.25f); // espera até o foco chegar perto da porta2
        // yield return new WaitForSeconds(1f); // tempo de foco na porta2

        // vcam.GoToNextStep(); // termina
    }

    IEnumerator StartCutSceneStep3()
    {
        PararPlayer();

        javali.SetActive(false);
        javali2.transform.position = javali.transform.position-Vector3.right;
        javali2.SetActive(true);


        javali2.GetComponent<IA_Javali>().MovementSpeed = 3.2f;
        javali2.GetComponent<IA_Javali>().Move = false;
        javali2.GetComponent<IA_Javali>().JavaliAnimator.SetFloat("WalkSpeed", 1.3f);

        vcam.transitionTimeGoing = 0.15f;
        vcam.transitionTimeComingBack = 0.1f;
        vcam.StartFocus(new GameObject[1]{javali2}, true); // 2
        vcam.keepFocusingTarget = true;
        yield return new WaitForSeconds(0.2f); // tempo de foco 2 seg
        javali2.GetComponent<IA_Javali>().Move = true;
        yield return new WaitForSeconds(1.5f); // tempo de foco 2 seg
        vcam.GoToNextStep(); // volta pro player
        
        boxCol3.enabled     = false;
        currentStep = 4;


        MoverPlayer();
        this.enabled = false; // desligar esse script
    }
}
