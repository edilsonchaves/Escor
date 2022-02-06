using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Script : MonoBehaviour
{
    [SerializeField] Animator _animatorBoss;
    [SerializeField] private List<Conversa> conversaBoss;
    [SerializeField] private List<Conversa> conversaBossMorte;
    [SerializeField] ConversaPersonagem conversa;
    enum BossStatus {Parado,Game,Die }
    BossStatus bossStatus;
    void Start()
    {
        conversa = GameObject.FindGameObjectWithTag("ConversaPersonagem").GetComponent<ConversaPersonagem>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && bossStatus==BossStatus.Game)
        {
            StartCoroutine(DieStatusBossBattle());
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            StartCoroutine(InitialStatusBossBattle());
        }

    }

    IEnumerator InitialStatusBossBattle()
    {
        StartCoroutine(conversa.ConversaFase(conversaBoss));
        yield return new WaitUntil(() => conversa.StatusConversa);
        _animatorBoss.Play("BossLevantando");
        bossStatus = BossStatus.Game;
    }
    IEnumerator DieStatusBossBattle()
    {
        StartCoroutine(conversa.ConversaFase(conversaBossMorte));
        _animatorBoss.Play("BossDerrotadoStart");
        yield return new WaitUntil(() => conversa.StatusConversa);
        bossStatus = BossStatus.Die;
    }
}
