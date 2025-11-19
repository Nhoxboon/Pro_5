using UnityEngine;

public class ERangeBattleState : EnemyState
{
    protected EnemyRange enemy;
    protected ERangeAttack enemyAttack;

    protected float lastTimeShot = -10;
    protected int bulletsFired = 0;

    public ERangeBattleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase,
        stateMachine, animBoolName)
    {
        enemy = enemyBase as EnemyRange;
        enemyAttack = enemy?.EnemyAttack as ERangeAttack;
    }

    public override void Enter()
    {
        base.Enter();
        // enemy.EnemyMove.Agent.isStopped = true;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        enemy.EnemyMove.FaceTarget(PlayerCtrl.Instance.transform.position);
        if (WeaponOutOfBullet())
        {
            if (WeaponOnCooldown())
                ResetBulletsFired();
            return;
        }

        if (CanShoot())
            Shoot();
    }

    protected void ResetBulletsFired() => bulletsFired = 0;

    protected bool CanShoot() => Time.time > lastTimeShot + 1 / enemyAttack.fireRate;

    protected bool WeaponOnCooldown() => Time.time > lastTimeShot + enemyAttack.weaponCooldown;
    protected bool WeaponOutOfBullet() => bulletsFired >= enemyAttack.bulletToShoot;

    protected void Shoot()
    {
        enemyAttack.FireSingleBullet();
        lastTimeShot = Time.time;
        bulletsFired++;
    }
}