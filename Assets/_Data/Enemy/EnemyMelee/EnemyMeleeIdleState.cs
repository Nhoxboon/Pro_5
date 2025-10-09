
using UnityEngine;

public class EnemyMeleeIdleState : EnemyState
{
    protected EnemyMelee enemy;
    
    public EnemyMeleeIdleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
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
        if(startTimer < 0)
            stateMachine.ChangeState(enemy.MoveState);
    }
}
