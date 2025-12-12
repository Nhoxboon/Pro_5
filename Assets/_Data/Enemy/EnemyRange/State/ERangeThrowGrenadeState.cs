using System.Collections;
using UnityEngine;

public class ERangeThrowGrenadeState : EnemyState
{
    protected EnemyRange enemy;
    protected ERangeAbility enemyAbility;
    protected ERangeVisuals enemyVisuals;

    public bool FinishedThrowing { get; protected set; } = true;

    public ERangeThrowGrenadeState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(
        enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as EnemyRange;
        enemyAbility = enemy?.EnemyAbility;
        enemyVisuals = enemy?.EnemyVisuals;
    }

    public override void Enter()
    {
        base.Enter();
        FinishedThrowing = false;

        enemy.EnemyMove.Agent.isStopped = true;
        enemyVisuals.EnableWeaponModel(false);
        enemyVisuals.EnableIK(false, false);
        enemyVisuals.EnableSecondaryWeaponModel(true);
        enemyVisuals.EnableGrenadeModel(true);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.EnemyMove.Agent.isStopped = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        Vector3 playerPos = PlayerCtrl.Instance.transform.position + Vector3.up;
        enemy.EnemyMove.FaceTarget(playerPos);
        enemy.EnemyAim.aim.position = playerPos;

        if (triggerCalled)
            stateMachine.ChangeState(enemy.BattleState);
    }

    public override void AbilityTrigger()
    {
        base.AbilityTrigger();
        FinishedThrowing = true;
        enemyAbility.ThrowGrenade();
        enemyVisuals.EnableGrenadeModel(false);
        enemy.StartCoroutine(ResetIKDelay());
    }

    protected IEnumerator ResetIKDelay()
    {
        yield return new WaitForSeconds(0.5f);
        enemyVisuals.EnableIK(true, true, 1f);
    }
}