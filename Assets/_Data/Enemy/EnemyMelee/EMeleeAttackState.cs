using UnityEngine;

public class EMeleeAttackState : EnemyState
{
    protected EnemyMelee enemy;
    protected EnemyMove enemyMove;
    protected EnemyAttack enemyAttack;
    protected Vector3 attackDirection;
    protected float attackMoveSpeed;

    protected const float MAX_ATTACK_DISTANCE = 50f;

    public EMeleeAttackState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase,
        stateMachine, animBoolName)
    {
        enemy = enemyBase as EnemyMelee;
        enemyMove = enemy?.EnemyMove;
        enemyAttack = enemy?.EnemyAttack;
    }

    public override void Enter()
    {
        base.Enter();
        SetAttackProperties();

        enemy.PullWeapon();

        enemyMove.Agent.isStopped = true;
        enemyMove.Agent.velocity = Vector3.zero;

        attackDirection = enemy.transform.position + (enemy.transform.forward * MAX_ATTACK_DISTANCE);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.EnemyAnimation.AnimationSetFloat("recoveryIndex", 0);
        
        if(enemyAttack.PlayerInAttackRange())
            enemy.EnemyAnimation.AnimationSetFloat("recoveryIndex", 1);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(enemyMove.ManualRotation)
        {
            enemy.transform.rotation = enemyMove.FaceTarget(PlayerCtrl.Instance.transform.position);
            attackDirection = enemy.transform.position + (enemy.transform.forward * MAX_ATTACK_DISTANCE);
        }
        
        if (enemyMove.ManualMovement)
        {
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, attackDirection,
                attackMoveSpeed * Time.deltaTime);
        }

        if (!triggerCalled) return;
        if(enemyAttack.PlayerInAttackRange())
            stateMachine.ChangeState(enemy.RecoveryState);
        else
            stateMachine.ChangeState(enemy.ChaseState);
    }
    
    protected void SetAttackProperties()
    {
        attackMoveSpeed = enemyAttack.attackData.moveSpeed;
        enemy.EnemyAnimation.AnimationSetFloat("attackAnimationSpeed", enemyAttack.attackData.animationSpeed);
        enemy.EnemyAnimation.AnimationSetFloat("attackIndex", enemyAttack.attackData.attackIndex);
    }
}