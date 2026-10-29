using UnityEngine;

public class EMeleeDeadState : EnemyState
{
    protected EnemyMelee enemy;

    protected bool interactionDisable;

    public EMeleeDeadState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase,
        stateMachine, animBoolName)
    {
        enemy = enemyBase as EnemyMelee;
    }

    public override void Enter()
    {
        base.Enter();
        interactionDisable = false;

        stateTimer = 1.5f; //Note: Uncomment if need to disable interaction with enemy
    }

    public override void Exit()
    {
        base.Exit();
        interactionDisable = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // Note: Uncomment if need to disable interaction with enemy
         DisableInteraction();
    }

    protected void DisableInteraction()
    {
        if (!(stateTimer < 0) || interactionDisable) return;
        interactionDisable = true;
        enemy.Core.Ragdoll.RagdollActive(false);
        enemy.Core.Ragdoll.ColliderActive(false);
    }
}