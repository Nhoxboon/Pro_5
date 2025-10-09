
using UnityEngine;

public class EnemyMelee : Enemy
{
    public EnemyMeleeIdleState IdleState { get; protected set; }
    public EnemyMeleeMoveState MoveState { get; protected set; }

    protected override void Awake()
    {
        base.Awake();
        IdleState = new EnemyMeleeIdleState(this, StateMachine, "idle");
        MoveState = new EnemyMeleeMoveState(this, StateMachine, "move");
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
