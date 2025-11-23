
using UnityEngine;

public class ERangeDeadState : EnemyState
{
    protected EnemyRange enemy;
    protected ERangeAttack enemyAttack;
    protected bool interactionDisable;
    
    public ERangeDeadState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as EnemyRange;
        enemyAttack = enemy?.EnemyAttack as ERangeAttack;
    }

    public override void Enter()
    {
        base.Enter();
        
        if(!enemy.ThrowGrenadeState.FinishedThrowing)
            enemyAttack.ThrowGrenade();
        
        interactionDisable = false;

        enemy.EnemyAnimator.SetAnimator(false);
        enemy.EnemyMove.Agent.isStopped = true;

        enemy.Ragdoll.RagdollActive(true);
        // startTimer = 1.5f; //Note: Uncomment if need to disable interaction with enemy
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        //Note: Uncomment if need to disable interaction with enemy
        // DisableInteraction();
    }
    
    protected void DisableInteraction()
    {
        if (!(stateTimer < 0) || interactionDisable) return;
        interactionDisable = true;
        enemy.Ragdoll.RagdollActive(false);
        enemy.Ragdoll.ColliderActive(false);
    }
}
