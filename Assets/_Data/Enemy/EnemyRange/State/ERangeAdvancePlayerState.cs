using UnityEngine;

public class ERangeAdvancePlayerState : EnemyState
{
    protected EnemyRange enemy;
    protected EnemyMove enemyMove;
    protected ERangeVisuals enemyVisuals;
    protected EnemyCoverSystem enemyCover;

    protected Vector3 playerPos;
    public float LastTimeAdvanced { get; protected set; }

    public ERangeAdvancePlayerState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(
        enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as EnemyRange;
        enemyMove = enemy?.EnemyMove;
        enemyVisuals = enemy?.EnemyVisuals;
        enemyCover = enemy?.EnemyCover;
    }

    public override void Enter()
    {
        base.Enter();
        enemyVisuals.EnableIK(true, false);
        enemyMove.Agent.speed = enemyCover.IsUnstoppable() ? enemyMove.WalkSpeed : enemyMove.RunSpeed;

        if (enemyCover.IsUnstoppable())
            stateTimer = enemyMove.AdvanceDuration;
    }

    public override void Exit()
    {
        base.Exit();
        LastTimeAdvanced = Time.time;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        playerPos = PlayerCtrl.Instance.transform.position;
        enemy.EnemyAim.UpdateAimPosition();

        enemyMove.Agent.SetDestination(playerPos);
        enemyMove.FaceTarget(GetNextPathPoint());

        if (CanEnterBattleState() && enemy.EnemyAim.IsSeeingPlayer())
            stateMachine.ChangeState(enemy.BattleState);
    }

    protected bool CanEnterBattleState()
    {
        bool closeEnoughToPlayer =
            Vector3.Distance(enemy.transform.position, playerPos) < enemyMove.AdvanceStoppingDistance;

        if (enemyCover.IsUnstoppable())
            return closeEnoughToPlayer || stateTimer < 0f;
        return closeEnoughToPlayer;
    }
}