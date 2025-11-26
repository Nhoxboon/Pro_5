
using UnityEngine;

public class EBossAbilityState : EnemyState
{
    protected EnemyBoss enemy;
    protected EnemyMove enemyMove;
    protected EBossAbility enemyAbility;
    
    public EBossAbilityState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as EnemyBoss;
        enemyMove = enemy?.EnemyMove;
        enemyAbility = enemy?.EnemyAbility;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemyAbility.FlameThrowDuration;
        enemyMove.Agent.isStopped = true;
        enemyMove.Agent.velocity = Vector3.zero;
        enemy.EnemyVisuals.EnableWeaponTrail(true);
    }

    public override void Exit()
    {
        base.Exit();
        enemyMove.Agent.isStopped = false;
        enemyAbility.SetAbilityOnCooldown();
        enemy.EnemyVisuals.ResetBatteries();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        enemyMove.FaceTarget(PlayerCtrl.Instance.transform.position);
        
        if (stateTimer < 0 && enemyAbility.FlameThrowActive)
            enemyAbility.ActivateFlameThrower(false);
        
        if(triggerCalled)
            stateMachine.ChangeState(enemy.MoveState);
    }

    public override void AbilityTrigger()
    {
        base.AbilityTrigger();
        enemyAbility.ActivateFlameThrower(true);
        enemy.EnemyVisuals.DischargeBatteries();
        enemy.EnemyVisuals.EnableWeaponTrail(false);
    }
}
