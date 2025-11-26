using UnityEngine;

public class EMeleeRecoveryState : EnemyState
{
    protected EnemyMelee enemy;
    protected EMeleeAttack enemyAttack;
    protected EMeleeAbility enemyAbility;

    public EMeleeRecoveryState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase,
        stateMachine, animBoolName)
    {
        enemy = enemyBase as EnemyMelee;
        enemyAttack = enemy?.EnemyAttack as EMeleeAttack;
        enemyAbility = enemy?.EnemyAbility;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.EnemyMove.Agent.isStopped = true;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        enemy.EnemyMove.FaceTarget(PlayerCtrl.Instance.transform.position);

        if (!triggerCalled) return;
        if(enemyAbility.CanUseAbility())
            stateMachine.ChangeState(enemy.AbilityState);
        else if(enemyAttack.PlayerInAttackRange())
            stateMachine.ChangeState(enemy.AttackState);
        else
            stateMachine.ChangeState(enemy.ChaseState);
    }
}