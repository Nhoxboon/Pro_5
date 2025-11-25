using UnityEngine;

public class EBossJumpAtkState : EnemyState
{
    protected EnemyBoss enemy;
    protected EnemyMove enemyMove;
    protected EBossAttack enemyAttack;

    protected Vector3 lastPlayerPos;
    protected float jumpAtkSpeed;

    public EBossJumpAtkState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase,
        stateMachine, animBoolName)
    {
        enemy = enemyBase as EnemyBoss;
        enemyMove = enemy?.EnemyMove;
        enemyAttack = enemy?.EnemyAttack as EBossAttack;
    }

    public override void Enter()
    {
        base.Enter();
        enemyMove.Agent.isStopped = true;
        enemyMove.Agent.velocity = Vector3.zero;

        lastPlayerPos = PlayerCtrl.Instance.transform.position;
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, lastPlayerPos);
        jumpAtkSpeed = distanceToPlayer / enemyAttack.TravelTimeToTarget;
        
        enemyMove.FaceTarget(lastPlayerPos, 1000);
    }

    public override void Exit()
    {
        base.Exit();
        enemyMove.Agent.isStopped = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        enemyMove.Agent.enabled = !enemyMove.ManualMovement;

        if (enemyMove.ManualMovement)
        {
            enemy.transform.position =
                Vector3.MoveTowards(enemy.transform.position, lastPlayerPos, jumpAtkSpeed * Time.deltaTime);
        }

        if (triggerCalled)
            stateMachine.ChangeState(enemy.MoveState);
    }
}