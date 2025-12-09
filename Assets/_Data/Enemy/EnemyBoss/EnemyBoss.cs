using UnityEngine;

public class EnemyBoss : Enemy
{
    #region State

    public EBossIdleState IdleState { get; protected set; }
    public EBossMoveState MoveState { get; protected set; }
    public EBossAttackState AttackState { get; protected set; }
    public EBossJumpAtkState JumpAtkState { get; protected set; }
    public EBossAbilityState AbilityState { get; protected set; }
    public EBossDeadState DeadState { get; protected set; }

    #endregion

    [Header("Boss Settings")] 
    [SerializeField] protected EBossVisuals enemyVisuals;
    public EBossVisuals EnemyVisuals => enemyVisuals;
    [SerializeField] protected EBossAbility enemyAbility;
    public EBossAbility EnemyAbility => enemyAbility;
    [SerializeField] protected float actionCooldown = 10f;
    public float ActionCooldown => actionCooldown;

    protected override void Awake()
    {
        base.Awake();

        IdleState = new EBossIdleState(this, StateMachine, "idle");
        MoveState = new EBossMoveState(this, StateMachine, "move");
        AttackState = new EBossAttackState(this, StateMachine, "attack");
        JumpAtkState = new EBossJumpAtkState(this, StateMachine, "jumpAttack");
        AbilityState = new EBossAbilityState(this, StateMachine, "ability");
        DeadState = new EBossDeadState(this, StateMachine, "idle");
    }

    protected override void Start()
    {
        base.Start();
        StateMachine.Initialize(IdleState);
    }

    protected override void Update()
    {
        base.Update();
        StateMachine.currentState.LogicUpdate();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadEnemyVisuals();
        LoadEnemyAbility();
    }

    protected void LoadEnemyVisuals()
    {
        if (enemyVisuals != null) return;
        enemyVisuals = GetComponentInChildren<EBossVisuals>();
        Debug.Log(transform.name + " LoadEnemyVisuals", gameObject);
    }

    protected void LoadEnemyAbility()
    {
        if (enemyAbility != null) return;
        enemyAbility = GetComponentInChildren<EBossAbility>();
        Debug.Log(transform.name + " LoadEnemyAbility", gameObject);
    }

    public override void EnterBattleMode()
    {
        if (InBattleMode) return;
        base.EnterBattleMode();
        StateMachine.ChangeState(MoveState);
    }

    protected override void EnemyDie()
    {
        base.EnemyDie();
        StateMachine.ChangeState(DeadState);
    }
}