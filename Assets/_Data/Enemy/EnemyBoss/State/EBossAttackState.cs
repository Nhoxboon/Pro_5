using UnityEngine;

public class EBossAttackState : EnemyState
{
    protected EnemyBoss enemy;
    protected EnemyMove enemyMove;

    public EBossAttackState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase,
        stateMachine, animBoolName)
    {
        enemy = enemyBase as EnemyBoss;
        enemyMove = enemy?.EnemyMove;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.EnemyAnimator.AnimationSetFloat("attackIndex", Random.Range(0, 2));
        enemyMove.Agent.isStopped = true;
    }

    public override void Exit()
    {
        base.Exit();
        enemyMove.Agent.isStopped = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (triggerCalled)
        {
            if (enemy.EnemyAttack.PlayerInAttackRange())
                stateMachine.ChangeState(enemy.IdleState);
            else
                stateMachine.ChangeState(enemy.MoveState);
        }
    }
}