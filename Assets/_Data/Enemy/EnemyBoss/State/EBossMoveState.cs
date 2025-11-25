using UnityEngine;

public class EBossMoveState : EnemyState
{
    protected EnemyBoss enemy;
    protected EnemyMove enemyMove;
    protected EBossAttack enemyAttack;

    protected Vector3 destination;

    public EBossMoveState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase,
        stateMachine, animBoolName)
    {
        enemy = enemyBase as EnemyBoss;
        enemyMove = enemy?.EnemyMove;
        enemyAttack = enemy?.EnemyAttack as EBossAttack;
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

        if (enemy.InBattleMode)
        {
            enemyMove.Agent.SetDestination(PlayerCtrl.Instance.transform.position);

            if (enemyAttack.CanJumpAttack())
                stateMachine.ChangeState(enemy.JumpAtkState);
            else if (enemyAttack.PlayerInAttackRange())
                stateMachine.ChangeState(enemy.AttackState);
        }
        else
        {
            if (Vector3.Distance(enemy.transform.position, destination) < 0.05f)
                stateMachine.ChangeState(enemy.IdleState);
        }
    }
}