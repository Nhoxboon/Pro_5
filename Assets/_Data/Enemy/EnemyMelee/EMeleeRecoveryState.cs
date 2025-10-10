
using UnityEngine;

public class EMeleeRecoveryState : EnemyState
{
    protected EnemyMelee enemy;
    
    public EMeleeRecoveryState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as EnemyMelee;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.EnemyMove.Agent.isStopped = true;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        enemy.transform.rotation = enemy.EnemyMove.FaceTarget(PlayerCtrl.Instance.transform.position);
        
        if(triggerCalled)
            stateMachine.ChangeState(enemy.ChaseState);
    }
}
