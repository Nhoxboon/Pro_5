using UnityEngine;

public class EBossAttackState : EnemyState
{
    protected EnemyBoss enemy;
    protected EnemyMove enemyMove;
    public float LastTimeAttacked { get; protected set; }

    public EBossAttackState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase,
        stateMachine, animBoolName)
    {
        enemy = enemyBase as EnemyBoss;
        enemyMove = enemy?.EnemyMove;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.EnemyVisuals.EnableWeaponTrail(true);
        enemy.EnemyAnimator.AnimationSetFloat("attackIndex", Random.Range(0, 2));
        enemyMove.Agent.isStopped = true;
        stateTimer = 1f;
    }

    public override void Exit()
    {
        base.Exit();
        enemy.EnemyVisuals.EnableWeaponTrail(false);
        enemyMove.Agent.isStopped = false;
        LastTimeAttacked = Time.time;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        if (stateTimer > 0)
            enemyMove.FaceTarget(PlayerCtrl.Instance.transform.position, 20);

        if (triggerCalled)
        {
            if (enemy.EnemyAttack.PlayerInAttackRange())
                stateMachine.ChangeState(enemy.IdleState);
            else
                stateMachine.ChangeState(enemy.MoveState);
        }
    }
}