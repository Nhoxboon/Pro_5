using UnityEngine;

public class EBossMoveState : EnemyState
{
    protected EnemyBoss enemy;
    protected EnemyMove enemyMove;
    protected EBossAttack enemyAttack;
    protected EBossAbility enemyAbility;

    protected Vector3 destination;
    protected float timeBeforeSpeedUp = 8f;
    protected bool speedUpActivated;
    protected float actionTimer;

    public EBossMoveState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase,
        stateMachine, animBoolName)
    {
        enemy = enemyBase as EnemyBoss;
        enemyMove = enemy?.EnemyMove;
        enemyAttack = enemy?.EnemyAttack as EBossAttack;
        enemyAbility = enemy?.EnemyAbility;
    }

    public override void Enter()
    {
        base.Enter();
        SpeedReset();
        destination = enemyMove.GetPatrolDestination();
        enemyMove.Agent.SetDestination(destination);

        actionTimer = enemy.ActionCooldown;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        actionTimer -= Time.deltaTime;
        enemyMove.FaceTarget(GetNextPathPoint());

        if (enemy.InBattleMode)
        {
            if (ShouldSpeedUp())
                SpeedUp();

            enemyMove.Agent.SetDestination(PlayerCtrl.Instance.transform.position);

            if (actionTimer < 0)
                PerformRandomAction();
            else if (enemyAttack.PlayerInAttackRange())
                stateMachine.ChangeState(enemy.AttackState);
        }
        else
        {
            if (Vector3.Distance(enemy.transform.position, destination) < 0.05f)
                stateMachine.ChangeState(enemy.IdleState);
        }
    }

    protected void SpeedUp()
    {
        enemyMove.Agent.speed = enemyMove.RunSpeed;
        enemy.EnemyAnimator.AnimationSetFloat("moveIndex", 1);
        enemy.EnemyAnimator.AnimationSetFloat("moveAnimSpeedMultiplier", 1.5f);
        speedUpActivated = true;
    }

    protected void SpeedReset()
    {
        speedUpActivated = false;
        enemy.EnemyAnimator.AnimationSetFloat("moveAnimSpeedMultiplier", 1f);
        enemy.EnemyAnimator.AnimationSetFloat("moveIndex", 0);
        enemyMove.Agent.speed = enemyMove.WalkSpeed;
    }

    protected bool ShouldSpeedUp()
    {
        if (speedUpActivated) return false;
        return Time.time > enemy.AttackState.LastTimeAttacked + timeBeforeSpeedUp;
    }

    protected void PerformRandomAction()
    {
        actionTimer = enemy.ActionCooldown;
        if (Random.Range(0, 2) == 0)
            TryAbility();
        else
        {
            if (enemyAttack.CanJumpAttack())
                stateMachine.ChangeState(enemy.JumpAtkState);
            else if(enemyAttack.WeaponType == BossWeaponType.Hammer)
                TryAbility();
        }
    }

    protected void TryAbility()
    {
        if (enemyAbility.CanUseAbility())
            stateMachine.ChangeState(enemy.AbilityState);
    }
}