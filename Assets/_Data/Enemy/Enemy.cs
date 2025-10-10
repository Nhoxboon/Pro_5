using System;
using UnityEngine;

public class Enemy : NhoxBehaviour
{
    [Header("Data")]
    public float idleTime = 2f;
    public float aggroRange = 10f;

    [SerializeField] protected EnemyMove enemyMove;
    public EnemyMove EnemyMove => enemyMove;

    [SerializeField] protected EnemyAnimation enemyAnimation;
    public EnemyAnimation EnemyAnimation => enemyAnimation;
    
    public EnemyStateMachine StateMachine { get; protected set; }
    
    protected override void Awake()
    {
        base.Awake();
        StateMachine = new EnemyStateMachine();
    }

    protected virtual void Update()
    {
        
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadEnemyMove();
        LoadEnemyAnimation();
    }

    protected void LoadEnemyMove()
    {
        if(enemyMove != null) return;
        enemyMove = GetComponentInChildren<EnemyMove>();
        Debug.Log(transform.name + " LoadEnemyMove", gameObject);
    }
    
    protected void LoadEnemyAnimation()
    {
        if(enemyAnimation != null) return;
        enemyAnimation = GetComponentInChildren<EnemyAnimation>();
        Debug.Log(transform.name + " LoadEnemyAnimation", gameObject);
    }
    
    public bool PlayerInAggroRange() => Vector3.Distance(transform.position, PlayerCtrl.Instance.transform.position) < aggroRange;

    public void AnimationTrigger() => StateMachine.currentState.AnimationTrigger();

    protected void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, aggroRange);
    }
}