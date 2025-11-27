using UnityEngine;

public class EBossIdleState : EnemyState
{
    protected EnemyBoss enemy;
    protected EBossAbility enemyAbility;

    public EBossIdleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase,
        stateMachine, animBoolName)
    {
        enemy = enemyBase as EnemyBoss;
        enemyAbility = enemy?.EnemyAbility;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.idleTime;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (enemy.InBattleMode && enemy.EnemyAttack.PlayerInAttackRange())
        {
            if(enemyAbility.CanUseAbility())
                stateMachine.ChangeState(enemy.AbilityState);
            else
                stateMachine.ChangeState(enemy.AttackState);
        }

        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.MoveState);
    }
}