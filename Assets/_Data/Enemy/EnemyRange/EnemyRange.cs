using System.Collections.Generic;
using UnityEngine;

public class EnemyRange : Enemy
{
    #region State

    public ERangeIdleState IdleState { get; protected set; }
    public ERangeMoveState MoveState { get; protected set; }
    public ERangeBattleState BattleState { get; protected set; }
    public ERangeRunToCoverState RunToCoverState { get; protected set; }
    public ERangeAdvancePlayerState AdvancePlayerState { get; protected set; }
    public ERangeThrowGrenadeState ThrowGrenadeState { get; protected set; }
    public ERangeDeadState DeadState { get; protected set; }

    #endregion

    [SerializeField] protected EnemyCoverSystem enemyCover;
    public EnemyCoverSystem EnemyCover => enemyCover;
    [SerializeField] protected EnemyAim enemyAim;
    public EnemyAim EnemyAim => enemyAim;
    
    protected override void Awake()
    {
        base.Awake();
        IdleState = new ERangeIdleState(this, StateMachine, "idle");
        MoveState = new ERangeMoveState(this, StateMachine, "move");
        BattleState = new ERangeBattleState(this, StateMachine, "battle");
        RunToCoverState = new ERangeRunToCoverState(this, StateMachine, "run");
        AdvancePlayerState = new ERangeAdvancePlayerState(this, StateMachine, "advance");
        ThrowGrenadeState = new ERangeThrowGrenadeState(this, StateMachine, "throwGrenade");
        DeadState = new ERangeDeadState(this, StateMachine, "idle");  //Idle anim is just a placeholder, we use rag doll
        
        core.Stats.Health.OnCurrentValueZero += EnemyDead;
    }
    
    protected override void OnDestroy()
    {
        base.OnDestroy();
        core.Stats.Health.OnCurrentValueZero -= EnemyDead;
    }

    protected override void Start()
    {
        base.Start();
        InitializePerk();
        StateMachine.Initialize(IdleState);
        enemyVisuals.SetupLook();
        enemyAttack.UpdateAttackData();
    }

    protected override void Update()
    {
        base.Update();
        StateMachine.currentState.LogicUpdate();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadEnemyCover();
        LoadEnemyAim();
    }

    protected void LoadEnemyCover()
    {
        if (enemyCover != null) return;
        enemyCover = GetComponentInChildren<EnemyCoverSystem>();
        Debug.Log(transform.name + " LoadEnemyCover", gameObject);
    }

    protected void LoadEnemyAim()
    {
        if (enemyAim != null) return;
        enemyAim = GetComponentInChildren<EnemyAim>();
        Debug.Log(transform.name + " LoadEnemyAim", gameObject);
    }

    public override void EnterBattleMode()
    {
        if (InBattleMode) return;
        base.EnterBattleMode();
        if (enemyCover.CanGetCover())
            StateMachine.ChangeState(RunToCoverState);
        else
            StateMachine.ChangeState(BattleState);
    }

    protected override void InitializePerk()
    {
        if (enemyCover.IsUnstoppable())
            enemyAnimator.AnimationSetFloat("advanceIndex", 1);
    }
    
    protected override void EnemyDead()
    {
        base.EnemyDead();
        StateMachine.ChangeState(DeadState);
    }
}