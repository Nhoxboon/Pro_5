using UnityEngine;

public class EMeleeChaseState : EnemyState
{
    protected EnemyMelee enemy;
    protected EnemyMove enemyMove;
    protected float lastUpdateDestination;

    public EMeleeChaseState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase,
        stateMachine, animBoolName)
    {
        enemy = enemyBase as EnemyMelee;
        enemyMove = enemy?.EnemyMove;
    }

    public override void Enter()
    {
        base.Enter();
        enemyMove.Agent.speed = enemyMove.ChaseSpeed;
        enemyMove.Agent.isStopped = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (enemy.EnemyAttack.PlayerInAttackRange())
        {
            stateMachine.ChangeState(enemy.AttackState);
            return;
        }

        enemy.transform.rotation = enemyMove.FaceTarget(GetNextPathPoint());

        if (CanUpdateDestination())
        {
            enemyMove.Agent.destination = PlayerCtrl.Instance.transform.position;
        }
    }

    protected bool CanUpdateDestination()
    {
        if (!(Time.time > lastUpdateDestination + 0.25f)) return false;
        lastUpdateDestination = Time.time;
        return true;
    }
}