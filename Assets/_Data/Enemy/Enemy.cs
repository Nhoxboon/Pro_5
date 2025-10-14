using System;
using System.Collections;
using UnityEngine;

public class Enemy : NhoxBehaviour
{
    [Header("Data")] public float idleTime = 2f;
    public float aggroRange = 4f;

    [SerializeField] protected EnemyMove enemyMove;
    public EnemyMove EnemyMove => enemyMove;

    [SerializeField] protected EnemyAnimation enemyAnimation;
    public EnemyAnimation EnemyAnimation => enemyAnimation;

    [SerializeField] protected EnemyAttack enemyAttack;
    public EnemyAttack EnemyAttack => enemyAttack;

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
        LoadEnemyAttack();
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

    public virtual void GetHit()
    {
    }

    public virtual void HitImpact(Vector3 force, Vector3 hitPoint, Rigidbody rb) =>
        StartCoroutine(HitImpactCoroutine(force, hitPoint, rb));

    protected IEnumerator HitImpactCoroutine(Vector3 force, Vector3 hitPoint, Rigidbody rb)
    {
        yield return new WaitForSeconds(0.1f);
        rb.AddForceAtPosition(force, hitPoint, ForceMode.Impulse);
    }

    public bool PlayerInAggroRange() =>
        Vector3.Distance(transform.position, PlayerCtrl.Instance.transform.position) < aggroRange;

    public void AnimationTrigger() => StateMachine.currentState.AnimationTrigger();

    protected void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, aggroRange);
    }
}