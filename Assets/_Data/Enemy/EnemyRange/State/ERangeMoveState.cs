using UnityEngine;

public class ERangeMoveState : EnemyState
{
    protected EnemyRange enemy;

    protected EnemyMove enemyMove;
    protected Vector3 destination;

    public ERangeMoveState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase,
        stateMachine, animBoolName)
    {
        enemy = enemyBase as EnemyRange;
        enemyMove = enemy?.EnemyMove;
    }

    public override void Enter()
    {
        base.Enter();
        enemyMove.Agent.speed = enemyMove.WalkSpeed;
        destination = enemyMove.GetPatrolDestination();
        enemyMove.Agent.SetDestination(destination);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        enemyMove.FaceTarget(GetNextPathPoint());

        if (enemyMove.Agent.remainingDistance <= enemyMove.Agent.stoppingDistance + 0.05f)
            stateMachine.ChangeState(enemy.IdleState);
    }
}