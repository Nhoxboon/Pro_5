using UnityEngine;

public class EnemyMelee : Enemy
{
    public EMeleeIdleState IdleState { get; protected set; }
    public EMeleeMoveState MoveState { get; protected set; }
    public EMeleeRecoveryState RecoveryState { get; protected set; }
    public EMeleeChaseState ChaseState { get; protected set; }

    protected override void Awake()
    {
        base.Awake();
        IdleState = new EMeleeIdleState(this, StateMachine, "idle");
        MoveState = new EMeleeMoveState(this, StateMachine, "move");
        RecoveryState = new EMeleeRecoveryState(this, StateMachine, "recovery");
        ChaseState = new EMeleeChaseState(this, StateMachine, "chase");
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
}