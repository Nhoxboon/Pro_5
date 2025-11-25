using UnityEngine;

public class EMeleeAbilityState : EnemyState
{
    protected EnemyMelee enemy;
    protected EnemyMove enemyMove;
    protected EMeleeAbility enemyAbility;

    protected float moveSpeed;
    protected Vector3 movementDirection;
    protected const float MAX_MOVEMENT_DISTANCE = 20f;

    public EMeleeAbilityState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase,
        stateMachine, animBoolName)
    {
        enemy = enemyBase as EnemyMelee;
        enemyMove = enemy?.EnemyMove;
        enemyAbility = enemy?.EnemyAbility;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.EnemyVisuals.EnableWeaponModel(true);
        moveSpeed = enemyMove.WalkSpeed;
        movementDirection = enemy.transform.position + (enemy.transform.forward * MAX_MOVEMENT_DISTANCE);
    }

    public override void Exit()
    {
        base.Exit();
        enemyMove.SetMoveSpeed(moveSpeed);
        enemy.EnemyAnimator.AnimationSetFloat("recoveryIndex", 0);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (enemyMove.ManualRotation)
        {
            enemyMove.FaceTarget(PlayerCtrl.Instance.transform.position);
            movementDirection = enemy.transform.position + (enemy.transform.forward * MAX_MOVEMENT_DISTANCE);
        }

        if (enemyMove.ManualMovement)
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, movementDirection,
                enemyMove.WalkSpeed * Time.deltaTime);

        if (triggerCalled)
            stateMachine.ChangeState(enemy.RecoveryState);
    }

    public override void AbilityTrigger()
    {
        base.AbilityTrigger();
        enemyAbility.ThrowAxe();
    }
}