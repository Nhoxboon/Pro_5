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
        lastPlayerPos = PlayerCtrl.Instance.transform.position;
        enemyMove.FaceTarget(lastPlayerPos, 1000);
        enemyMove.Agent.isStopped = true;
        enemyMove.Agent.velocity = Vector3.zero;

        enemy.EnemyVisuals.PlaceLandingZoneFX(lastPlayerPos);
        enemy.EnemyVisuals.EnableWeaponTrail(true);

        float distanceToPlayer = Vector3.Distance(enemy.transform.position, lastPlayerPos);
        jumpAtkSpeed = distanceToPlayer / enemyAttack.TravelTimeToTarget;


        if (enemyAttack.WeaponType != BossWeaponType.Hammer) return;
        enemyMove.Agent.isStopped = false;
        enemyMove.Agent.speed = enemyMove.RunSpeed;
        enemyMove.Agent.SetDestination(lastPlayerPos);
    }

    public override void Exit()
    {
        base.Exit();
        enemyMove.Agent.isStopped = false;
        enemyAttack.SetJumpAtkOnCooldown();
        enemy.EnemyVisuals.EnableWeaponTrail(false);
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