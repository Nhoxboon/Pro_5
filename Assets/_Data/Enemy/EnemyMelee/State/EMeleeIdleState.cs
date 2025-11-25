using UnityEngine;

public class EMeleeIdleState : EnemyState
{
    protected EnemyMelee enemy;

    public EMeleeIdleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase,
        stateMachine, animBoolName)
    {
        enemy = enemyBase as EnemyMelee;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.idleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.MoveState);
    }
}