using UnityEngine;

public class ERangeIdleState : EnemyState
{
    protected EnemyRange enemy;
    protected ERangeVisuals enemyVisuals;

    public ERangeIdleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase,
        stateMachine, animBoolName)
    {
        enemy = enemyBase as EnemyRange;
        enemyVisuals = enemy?.EnemyVisuals;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.EnemyAnimator.AnimationSetFloat("idleIndex", Random.Range(0, 3));
        enemyVisuals.EnableIK(true, false);
        stateTimer = enemy.idleTime;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.MoveState);
    }
}