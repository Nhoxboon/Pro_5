using UnityEngine;

public class EBossDeadState : EnemyState
{
    protected EnemyBoss enemy;
    protected bool interactionDisable;

    public EBossDeadState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase,
        stateMachine, animBoolName)
    {
        enemy = enemyBase as EnemyBoss;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.AbilityState.DisableFlameThrower();

        interactionDisable = false;

        enemy.EnemyAnimator.SetAnimator(false);
        enemy.EnemyMove.Agent.isStopped = true;

        enemy.Core.Ragdoll.RagdollActive(true);

        stateTimer = 1.5f; //Note: Uncomment if need to disable interaction with enemy
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        //Note: Uncomment if need to disable interaction with enemy
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