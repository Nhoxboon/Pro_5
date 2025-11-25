using UnityEngine;

public class EnemyBoss : Enemy
{
    public EBossIdleState IdleState { get; protected set; }
    public EBossMoveState MoveState { get; protected set; }
    public EBossAttackState AttackState { get; protected set; }
    public EBossJumpAtkState JumpAtkState { get; protected set; }
    public EBossAbilityState AbilityState { get; protected set; }
    
    [SerializeField] protected EBossAbility enemyAbility;
    public EBossAbility EnemyAbility => enemyAbility;

    protected override void Awake()
    {
        base.Awake();

        IdleState = new EBossIdleState(this, StateMachine, "idle");
        MoveState = new EBossMoveState(this, StateMachine, "move");
        AttackState = new EBossAttackState(this, StateMachine, "attack");
        JumpAtkState = new EBossJumpAtkState(this, StateMachine, "jumpAttack");
        AbilityState = new EBossAbilityState(this, StateMachine, "ability");
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
        
        //Test purpose
        if(Input.GetKeyDown(KeyCode.V))
            StateMachine.ChangeState(AbilityState);
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadEnemyAbility();
    }

    protected void LoadEnemyAbility()
    {
        if(enemyAbility != null) return;
        enemyAbility = GetComponentInChildren<EBossAbility>();
        Debug.Log(transform.name + " LoadEnemyAbility", gameObject);
    }

    public override void EnterBattleMode()
    {
        base.EnterBattleMode();
        StateMachine.ChangeState(MoveState);
    }
}