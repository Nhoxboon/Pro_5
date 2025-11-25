using UnityEngine;

public class EBossIdleState : EnemyState
{
    protected EnemyBoss enemy;

    public EBossIdleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase,
        stateMachine, animBoolName)
    {
        enemy = enemyBase as EnemyBoss;
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

        if (enemy.InBattleMode && enemy.EnemyAttack.PlayerInAttackRange())
            stateMachine.ChangeState(enemy.AttackState);

        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.MoveState);
    }
}