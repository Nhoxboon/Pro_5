
using UnityEngine;

public class EnemyMeleeMoveState : EnemyState
{
    protected EnemyMelee enemy;
    
    protected EnemyMove enemyMove;
    protected Vector3 destination;
    
    public EnemyMeleeMoveState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as EnemyMelee;
        enemyMove = enemy?.EnemyMove;
    }

    public override void Enter()
    {
        base.Enter();
        destination = enemyMove.GetPatrolDestination();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        enemyMove.Agent.SetDestination(destination);
        if(enemyMove.Agent.remainingDistance <= 1)
            stateMachine.ChangeState(enemy.IdleState);
    }
}
