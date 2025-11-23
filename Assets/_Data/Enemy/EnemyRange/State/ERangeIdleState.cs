using UnityEngine;

public class ERangeIdleState : EnemyState
{
    protected EnemyRange enemy;

    public ERangeIdleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase,
        stateMachine, animBoolName)
    {
        enemy = enemyBase as EnemyRange;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.idleTime;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.MoveState);
    }
}