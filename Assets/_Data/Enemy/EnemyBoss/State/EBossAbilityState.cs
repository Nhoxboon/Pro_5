using System;
using UnityEngine;

public class EBossAbilityState : EnemyState
{
    protected EnemyBoss enemy;
    protected EnemyMove enemyMove;
    protected EBossAttack enemyAttack;
    protected EBossAbility enemyAbility;
    protected EBossVisuals enemyVisuals;

    public EBossAbilityState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase,
        stateMachine, animBoolName)
    {
        enemy = enemyBase as EnemyBoss;
        enemyMove = enemy?.EnemyMove;
        enemyAttack = enemy?.EnemyAttack as EBossAttack;
        enemyAbility = enemy?.EnemyAbility;
        enemyVisuals = enemy?.EnemyVisuals;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemyAbility.FlameThrowDuration;
        enemyMove.Agent.isStopped = true;
        enemyMove.Agent.velocity = Vector3.zero;
        enemyVisuals.EnableWeaponTrail(true);
    }

    public override void Exit()
    {
        base.Exit();
        enemyMove.Agent.isStopped = false;
        enemyAbility.SetAbilityOnCooldown();
        enemyVisuals.ResetBatteries();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        enemyMove.FaceTarget(PlayerCtrl.Instance.transform.position);

        if (ShouldDisableFlameThrower())
            DisableFlameThrower();

        if (triggerCalled)
            stateMachine.ChangeState(enemy.MoveState);
    }

    protected bool ShouldDisableFlameThrower() => stateTimer < 0;

    public override void AbilityTrigger()
    {
        base.AbilityTrigger();

        switch (enemyAttack.WeaponType)
        {
            case BossWeaponType.Fist:
                enemyAbility.ActivateFlameThrower(true);
                enemyVisuals.DischargeBatteries();
                enemyVisuals.EnableWeaponTrail(false);
                break;
            case BossWeaponType.Hammer:
                enemyAbility.ActivateHammer();
                break;
            default:
                break;
        }
    }

    public void DisableFlameThrower()
    {
        if(enemyAttack.WeaponType != BossWeaponType.Fist) return;
        if (!enemyAbility.FlameThrowActive) return;
        enemyAbility.ActivateFlameThrower(false);
    }
}