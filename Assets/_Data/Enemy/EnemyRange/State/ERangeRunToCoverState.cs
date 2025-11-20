using UnityEngine;

public class ERangeRunToCoverState : EnemyState
{
    protected EnemyRange enemy;
    protected EnemyMove enemyMove;
    protected ERangeVisuals enemyVisuals;
    
    protected Vector3 destination;

    public ERangeRunToCoverState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase,
        stateMachine, animBoolName)
    {
        enemy = enemyBase as EnemyRange;
        enemyMove = enemy?.EnemyMove;
        enemyVisuals = enemy?.EnemyVisuals as ERangeVisuals;
    }

    public override void Enter()
    {
        base.Enter();
        enemyVisuals.EnableIK(true, false);
        enemyMove.Agent.isStopped = false;
        enemyMove.Agent.speed = enemyMove.RunSpeed;
        destination = enemy.EnemyCover.AttemptToFindCover().position;
        enemyMove.Agent.SetDestination(destination);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(Vector3.Distance(enemy.transform.position, destination) < 0.5f)
        {
            stateMachine.ChangeState(enemy.BattleState);
        }
    }
}