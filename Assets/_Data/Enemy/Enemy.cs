using System;
using System.Collections;
using UnityEngine;

public class Enemy : NhoxBehaviour
{
    [Header("Data")] public float idleTime = 2f;
    public float aggroRange = 4f;

    [SerializeField] protected Core core;
    public Core Core => core;

    [SerializeField] protected EnemyMove enemyMove;
    public EnemyMove EnemyMove => enemyMove;

    [SerializeField] protected EnemyAnimation enemyAnimation;
    public EnemyAnimation EnemyAnimation => enemyAnimation;

    [SerializeField] protected EnemyAttack enemyAttack;
    public EnemyAttack EnemyAttack => enemyAttack;
    
    public bool InBattleMode {  get; protected set; }
    public EnemyStateMachine StateMachine { get; protected set; }

    protected override void Awake()
    {
        base.Awake();
        StateMachine = new EnemyStateMachine();
        core.DamageReceiver.GetDamage += EnterBattleMode;
    }

    protected virtual void OnDestroy() => core.DamageReceiver.GetDamage -= EnterBattleMode;

    protected virtual void Update()
    {
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadCore();
        LoadEnemyMove();
        LoadEnemyAnimation();
        LoadEnemyAttack();
    }

    protected void LoadCore()
    {
        if (core != null) return;
        core = GetComponentInChildren<Core>();
        Debug.Log(transform.name + " LoadCore", gameObject);
    }

    protected void LoadEnemyMove()
    {
        if (enemyMove != null) return;
        enemyMove = GetComponentInChildren<EnemyMove>();
        Debug.Log(transform.name + " LoadEnemyMove", gameObject);
    }

    protected void LoadEnemyAnimation()
    {
        if (enemyAnimation != null) return;
        enemyAnimation = GetComponentInChildren<EnemyAnimation>();
        Debug.Log(transform.name + " LoadEnemyAnimation", gameObject);
    }

    protected void LoadEnemyAttack()
    {
        if (enemyAttack != null) return;
        enemyAttack = GetComponentInChildren<EnemyAttack>();
        Debug.Log(transform.name + " LoadEnemyAttack", gameObject);
    }

    protected bool ShouldEnterBattleMode()
    {
        bool inAggroRange = Vector3.Distance(transform.position, PlayerCtrl.Instance.transform.position) < aggroRange;
        if (!inAggroRange || InBattleMode) return false;
        EnterBattleMode();
        return true;
    }
    
    public virtual void EnterBattleMode() => InBattleMode = true;

    public virtual void AbilityTrigger()
    {
        StateMachine.currentState.AbilityTrigger();
    }

    protected virtual void EnemyDead()
    {
    }

    public virtual void DeadImpact(Vector3 force, Vector3 hitPoint, Rigidbody rb) =>
        StartCoroutine(DeadImpactCoroutine(force, hitPoint, rb));

    protected IEnumerator DeadImpactCoroutine(Vector3 force, Vector3 hitPoint, Rigidbody rb)
    {
        yield return new WaitForSeconds(0.1f);
        rb.AddForceAtPosition(force, hitPoint, ForceMode.Impulse);
    }

    public void AnimationTrigger() => StateMachine.currentState.AnimationTrigger();

    protected void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, aggroRange);
    }
}