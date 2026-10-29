using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : NhoxBehaviour
{
    [Header("Data")]
    public EnemyType enemyType;
    public float idleTime = 2f;
    protected float originalAggroRange;
    public float aggroRange = 4f;

    [Header("Components")]
    [SerializeField] protected Core core;
    public Core Core => core;

    [SerializeField] protected EnemyMove enemyMove;
    public EnemyMove EnemyMove => enemyMove;

    [SerializeField] protected EnemyAnimator enemyAnimator;
    public EnemyAnimator EnemyAnimator => enemyAnimator;

    [SerializeField] protected EnemyAttack enemyAttack;
    public EnemyAttack EnemyAttack => enemyAttack;

    [SerializeField] protected EnemyDrop enemyDrop;

    public bool InBattleMode { get; protected set; }
    public EnemyStateMachine StateMachine { get; protected set; }

    protected override void Awake()
    {
        base.Awake();
        StateMachine = new EnemyStateMachine();
    }

    protected override void Start()
    {
        base.Start();
        originalAggroRange = aggroRange;
    }

    protected void OnEnable()
    {
        if (StateMachine?.currentState != null)
            ResetEnemy();
        core.DamageReceiver.GetDamage += EnterBattleMode;
        core.Stats.Health.OnCurrentValueZero += EnemyDie;
    }

    protected virtual void OnDisable()
    {
        core.DamageReceiver.GetDamage -= EnterBattleMode;
        core.Stats.Health.OnCurrentValueZero -= EnemyDie;
    }

    protected virtual void Update()
    {
        if (ShouldEnterBattleMode())
            EnterBattleMode();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadCore();
        LoadEnemyMove();
        LoadEnemyAnimation();
        LoadEnemyAttack();
        LoadEnemyDrop();
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
        if (enemyAnimator != null) return;
        enemyAnimator = GetComponentInChildren<EnemyAnimator>();
        Debug.Log(transform.name + " LoadEnemyAnimation", gameObject);
    }

    protected void LoadEnemyAttack()
    {
        if (enemyAttack != null) return;
        enemyAttack = GetComponentInChildren<EnemyAttack>();
        Debug.Log(transform.name + " LoadEnemyAttack", gameObject);
    }

    protected void LoadEnemyDrop()
    {
        if (enemyDrop != null) return;
        enemyDrop = GetComponentInChildren<EnemyDrop>();
        Debug.Log(transform.name + " LoadEnemyDrop", gameObject);
    }

    protected bool ShouldEnterBattleMode()
    {
        if (!IsPlayerInAggressRange() || InBattleMode) return false;
        EnterBattleMode();
        return true;
    }

    public virtual void EnterBattleMode() => InBattleMode = true;

    protected virtual void InitializePerk()
    {
        //For override
    }

    public virtual void MakeEnemyStronger()
    {
        int additionalHealth = Mathf.RoundToInt(core.Stats.Health.MaxValue * 1.5f);
        core.Stats.Health.SetMaxValue(core.Stats.Health.MaxValue + additionalHealth);
        core.Stats.Health.Init();
        transform.localScale *= 1.2f;
    }

    public virtual void AbilityTrigger() => StateMachine.currentState.AbilityTrigger();

    protected virtual void EnemyDie()
    {
        enemyDrop.DropItems();
        enemyAnimator.SetAnimator(false);
        enemyMove.Agent.isStopped = true;
        enemyMove.Agent.enabled = false;

        core.Ragdoll.RagdollActive(true);

        StartCoroutine(DespawnCoroutine());
        if(this.TryGetComponent(out MissionObj_HuntTarget huntTarget))
            huntTarget.TargetKilled();
    }

    public virtual void BulletImpact(Vector3 force, Vector3 hitPoint, Rigidbody rb) =>
        StartCoroutine(DeadImpactCoroutine(force, hitPoint, rb));

    protected IEnumerator DeadImpactCoroutine(Vector3 force, Vector3 hitPoint, Rigidbody rb)
    {
        yield return new WaitForSeconds(0.1f);
        rb?.AddForceAtPosition(force, hitPoint, ForceMode.Impulse);
    }

    public void AnimationTrigger() => StateMachine.currentState.AnimationTrigger();

    public bool IsPlayerInAggressRange() =>
        Vector3.Distance(transform.position, PlayerCtrl.Instance.transform.position) < aggroRange;

    protected IEnumerator DespawnCoroutine()
    {
        yield return new WaitForSeconds(3f);
        EnemySpawner.Instance.Despawn(gameObject);
    }

    protected virtual void OnDrawGizmos() => Gizmos.DrawWireSphere(transform.position, aggroRange);

    protected virtual void ResetEnemy()
    {
        aggroRange = originalAggroRange;
        InBattleMode = false;
        core.Stats.Health.Init();
        enemyMove.ResetMove();

        core.Ragdoll.ResetRagdoll();
        enemyAnimator.SetAnimator(true);
    }
}