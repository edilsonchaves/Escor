using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    BossBaseState currentState;
    public string currentStateName;
    [SerializeField] Transform target;
    [SerializeField] Animator _animatorBoss;
    [SerializeField] int bossLife=4;
    [SerializeField] ConversaPersonagem conversa;
    public InitialState initialState = new InitialState();
    public TalkState talkState = new TalkState();
    public PatrollState patrollState = new PatrollState();
    public RangeAttackState rangeState = new RangeAttackState();
    public MelleAttackState melleState = new MelleAttackState();
    public WaveAttackState waveAttackState = new WaveAttackState();
    public DieStateAttack dieState = new DieStateAttack();
    public TakeDamageStateAttack takeDamageState = new TakeDamageStateAttack();
    public Coroutine bossCoroutineAction;
    // Start is called before the first frame update
    private void Start()
    {
        conversa = GameObject.FindGameObjectWithTag("ConversaPersonagem").GetComponent<ConversaPersonagem>();
        InitializeCombat();
    }
    void InitializeCombat()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        SwitchState(initialState);
    }
    private void OnEnable()
    {
        //ManagerEvents.Enemy.onRockDelete += DeletBullet;
        ManagerEvents.Boss.onTakeDamage += TakeDamage;
    }
    private void OnDisable()
    {
        //ManagerEvents.Enemy.onRockDelete += DeletBullet;
        ManagerEvents.Boss.onTakeDamage += TakeDamage;
    }
    private void Update()
    {
        if (currentState == null)
            return;
        currentState.UpdateState(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Enter collision");
        currentState.OnCollisionEnter(this,collision);
    }
    public void SwitchState(BossBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
        currentStateName = state.GetStateName();

    }
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
    public GameObject GetTarget()
    {
        return target.gameObject;
    }
    public Vector3 TargetPosition()
    {
        return target.position;
    }

    public void PlayAnimation(string animationName)
    {
        _animatorBoss.Play(animationName);
    }

    public void TakeDamage()
    {
        bossLife--;
        ManagerEvents.Boss.UpdatedLife(bossLife);
        if (bossLife == 0)
        {
            SwitchState(dieState);
        }
        else
        {
            SwitchState(takeDamageState);
        }
    }


    public void Conversa(List<Conversa> conversaPersonagens)
    {
        
        StartCoroutine(conversa.ConversaFase(conversaPersonagens));
    }

    public bool GetStatusConversa()
    {
        return conversa.StatusConversa;
    }

    public Vector2 GetTargetPosition()
    {
        return new Vector2(target.position.x, target.position.y);
    }
}
