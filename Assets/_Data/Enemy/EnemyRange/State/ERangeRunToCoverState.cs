using UnityEngine;

public class ERangeRunToCoverState : EnemyState
{
    protected EnemyRange enemy;
    protected EnemyMove enemyMove;
    protected ERangeVisuals enemyVisuals;
    
    protected Vector3 destination;
    public float LastTimeRunToCover { get; protected set;}

    public ERangeRunToCoverState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase,
        stateMachine, animBoolName)
    {
        enemy = enemyBase as EnemyRange;
        enemyMove = enemy?.EnemyMove;
        enemyVisuals = enemy?.EnemyVisuals;
    }

    public override void Enter()
    {
        base.Enter();
        destination = enemy.EnemyCover.CurrentCover.transform.position;
        
        enemyVisuals.EnableIK(true, false);
        enemyMove.Agent.isStopped = false;
        enemyMove.Agent.speed = enemyMove.RunSpeed;
        enemyMove.Agent.SetDestination(destination);
    }

    public override void Exit()
    {
        base.Exit();
        LastTimeRunToCover = Time.time;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(Vector3.Distance(enemy.transform.position, destination) <= 0.3f)
            stateMachine.ChangeState(enemy.BattleState);
    }
}