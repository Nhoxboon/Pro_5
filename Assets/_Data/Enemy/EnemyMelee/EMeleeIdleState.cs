using UnityEngine;

public class EMeleeIdleState : EnemyState
{
    protected EnemyMelee enemy;
    
    public EMeleeIdleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as EnemyMelee;
    }

    public override void Enter()
    {
        base.Enter();
        startTimer = enemyBase.idleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(enemy.PlayerInAggroRange())
        {
            stateMachine.ChangeState(enemy.RecoveryState);
            return;
        }
        
        if(startTimer < 0)
            stateMachine.ChangeState(enemy.MoveState);
    }
}