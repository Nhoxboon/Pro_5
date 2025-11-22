
using UnityEngine;

public class ERangeAdvancePlayerState : EnemyState
{
    protected EnemyRange enemy;
    protected EnemyMove enemyMove;
    protected ERangeVisuals enemyVisuals;

    protected Vector3 playerPos;
    public float LastTimeAdvanced { get; protected set;}
    
    public ERangeAdvancePlayerState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as EnemyRange;
        enemyMove = enemy?.EnemyMove;
        enemyVisuals = enemy?.EnemyVisuals as ERangeVisuals;
    }

    public override void Enter()
    {
        base.Enter();
        enemyVisuals.EnableIK(true, false);
        enemyMove.Agent.speed = enemyMove.RunSpeed;
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
        
        if(CanEnterBattleState())
            stateMachine.ChangeState(enemy.BattleState);
    }

    protected bool CanEnterBattleState()
    {
        return Vector3.Distance(enemy.transform.position, playerPos) < enemyMove.AdvanceStoppingDistance &&
               enemy.EnemyAim.IsSeeingPlayer();
    }
}
