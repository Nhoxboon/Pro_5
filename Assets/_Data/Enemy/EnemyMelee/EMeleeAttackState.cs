using System.Collections.Generic;
using UnityEngine;

public class EMeleeAttackState : EnemyState
{
    protected EnemyMelee enemy;
    protected EnemyMove enemyMove;
    protected EnemyAnimation enemyAnimation;
    protected EnemyAttack enemyAttack;
    
    protected Vector3 attackDirection;
    protected float attackMoveSpeed;

    protected const float MAX_ATTACK_DISTANCE = 50f;

    public EMeleeAttackState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase,
        stateMachine, animBoolName)
    {
        enemy = enemyBase as EnemyMelee;
        enemyMove = enemy?.EnemyMove;
        enemyAnimation = enemy?.EnemyAnimation;
        enemyAttack = enemy?.EnemyAttack;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.PullWeapon();
        SetAttackProperties();

        enemyMove.Agent.isStopped = true;
        enemyMove.Agent.velocity = Vector3.zero;

        attackDirection = enemy.transform.position + (enemy.transform.forward * MAX_ATTACK_DISTANCE);
    }

    public override void Exit()
    {
        base.Exit();
        SetupNextAttack();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (enemyMove.ManualRotation)
        {
            enemyMove.FaceTarget(PlayerCtrl.Instance.transform.position);
            attackDirection = enemy.transform.position + (enemy.transform.forward * MAX_ATTACK_DISTANCE);
        }

        if (enemyMove.ManualMovement)
        {
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, attackDirection,
                attackMoveSpeed * Time.deltaTime);
        }

        if (!triggerCalled) return;
        if (enemyAttack.PlayerInAttackRange())
            stateMachine.ChangeState(enemy.RecoveryState);
        else
            stateMachine.ChangeState(enemy.ChaseState);
    }

    protected void SetupNextAttack()
    {
        int recoveryIndex = PlayerClose() ? 1 : 0;
        enemyAnimation.AnimationSetFloat("recoveryIndex", recoveryIndex);

        enemyAttack.attackData = UpdateAttackData();
    }

    protected void SetAttackProperties()
    {
        attackMoveSpeed = enemyAttack.attackData.moveSpeed;
        enemyAnimation.AnimationSetFloat("attackAnimationSpeed", enemyAttack.attackData.animationSpeed);
        enemyAnimation.AnimationSetFloat("attackIndex", enemyAttack.attackData.attackIndex);
        enemyAnimation.AnimationSetFloat("slashAttackIndex", Random.Range(0, 5));
    }

    protected bool PlayerClose() =>
        Vector3.Distance(enemy.transform.position, PlayerCtrl.Instance.transform.position) <= 1f;

    protected AttackData UpdateAttackData()
    {
        List<AttackData> validAttacks = new List<AttackData>(enemyAttack.AttackList);
        if (PlayerClose())
            validAttacks.RemoveAll(parameter => parameter.attackType == AttackTypeMelee.Charge);
        int random = Random.Range(0, validAttacks.Count);

        return validAttacks[random];
    }
}