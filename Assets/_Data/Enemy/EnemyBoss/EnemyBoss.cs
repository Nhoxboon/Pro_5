using UnityEngine;

public class EnemyBoss : Enemy
{
    public EBossIdleState IdleState { get; protected set; }
    public EBossMoveState MoveState { get; protected set; }
    public EBossAttackState AttackState { get; protected set; }
    public EBossJumpAtkState JumpAtkState { get; protected set; }

    protected override void Awake()
    {
        base.Awake();

        IdleState = new EBossIdleState(this, StateMachine, "idle");
        MoveState = new EBossMoveState(this, StateMachine, "move");
        AttackState = new EBossAttackState(this, StateMachine, "attack");
        JumpAtkState = new EBossJumpAtkState(this, StateMachine, "jumpAttack");
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

    public override void EnterBattleMode()
    {
        base.EnterBattleMode();
        StateMachine.ChangeState(MoveState);
    }
}