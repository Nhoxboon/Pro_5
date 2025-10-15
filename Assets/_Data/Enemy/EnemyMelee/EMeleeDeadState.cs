using UnityEngine;

public class EMeleeDeadState : EnemyState
{
    protected EnemyMelee enemy;
    protected EnemyRagdoll ragdoll;
    
    protected bool interactionDisable;

    public EMeleeDeadState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase,
        stateMachine, animBoolName)
    {
        enemy = enemyBase as EnemyMelee;
        ragdoll = enemy?.GetComponent<EnemyRagdoll>();
    }

    public override void Enter()
    {
        base.Enter();
        interactionDisable = false;
        
        enemy.EnemyAnimation.SetAnimator(false);
        enemy.EnemyMove.Agent.isStopped = true;
        
        ragdoll.RagdollActive(true);

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
        if (!(startTimer < 0) || interactionDisable) return;
        interactionDisable = true;
        ragdoll.RagdollActive(false);
        ragdoll.ColliderActive(false);
    }
}