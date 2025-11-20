using UnityEngine;

public class ERangeBattleState : EnemyState
{
    protected EnemyRange enemy;
    protected ERangeAttack enemyAttack;
    protected ERangeVisuals enemyVisuals;

    protected float lastTimeShot = -10;
    protected int bulletsFired = 0;
    protected int bulletsPerAttack;
    protected float weaponCooldown;

    public ERangeBattleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase,
        stateMachine, animBoolName)
    {
        enemy = enemyBase as EnemyRange;
        enemyAttack = enemy?.EnemyAttack as ERangeAttack;
        enemyVisuals = enemy?.EnemyVisuals as ERangeVisuals;
    }

    public override void Enter()
    {
        base.Enter();
        bulletsPerAttack = enemyAttack.WeaponData.GetBulletsPerAttack();
        weaponCooldown = enemyAttack.WeaponData.GetWeaponCooldown();

        enemyVisuals.EnableIK(true, true);
        enemy.EnemyMove.Agent.isStopped = true;
    }

    public override void Exit()
    {
        base.Exit();
        enemyVisuals.EnableIK(false, false);
        enemy.EnemyMove.Agent.isStopped = false;
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

    protected void ResetBulletsFired()
    {
        bulletsFired = 0;
        bulletsPerAttack = enemyAttack.WeaponData.GetBulletsPerAttack();
        weaponCooldown = enemyAttack.WeaponData.GetWeaponCooldown();
    }

    protected bool CanShoot() => Time.time > lastTimeShot + 1 / enemyAttack.WeaponData.fireRate;

    protected bool WeaponOnCooldown() => Time.time > lastTimeShot + weaponCooldown;
    protected bool WeaponOutOfBullet() => bulletsFired >= bulletsPerAttack;

    protected void Shoot()
    {
        enemyAttack.FireSingleBullet();
        lastTimeShot = Time.time;
        bulletsFired++;
    }
}