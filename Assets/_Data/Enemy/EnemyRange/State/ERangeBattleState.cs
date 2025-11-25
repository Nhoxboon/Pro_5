using UnityEngine;

public class ERangeBattleState : EnemyState
{
    protected EnemyRange enemy;
    protected EnemyMove enemyMove;
    protected ERangeAttack enemyAttack;
    protected ERangeVisuals enemyVisuals;
    protected EnemyCoverSystem enemyCover;

    protected float lastTimeShot = -10;
    protected int bulletsFired = 0;
    protected int bulletsPerAttack;
    protected float weaponCooldown;

    protected float coverCheckTimer;
    protected bool firstTimeAttack = true;

    public ERangeBattleState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase,
        stateMachine, animBoolName)
    {
        enemy = enemyBase as EnemyRange;
        enemyMove = enemy?.EnemyMove;
        enemyAttack = enemy?.EnemyAttack as ERangeAttack;
        enemyVisuals = enemy?.EnemyVisuals as ERangeVisuals;
        enemyCover = enemy?.EnemyCover;
    }

    public override void Enter()
    {
        base.Enter();
        SetupForFirstAttack();

        enemyMove.Agent.isStopped = true;
        enemyMove.Agent.velocity = Vector3.zero;
        enemyVisuals.EnableIK(true, true);
        stateTimer = enemyAttack.AttackDelay;
    }

    public override void Exit()
    {
        base.Exit();
        // enemyVisuals.EnableIK(false, false);
        enemyMove.Agent.isStopped = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (enemy.EnemyAim.IsSeeingPlayer())
            enemyMove.FaceTarget(enemy.EnemyAim.aim.position);

        if (enemyAttack.CanThrowGrenade())
            stateMachine.ChangeState(enemy.ThrowGrenadeState);

        if (AdvancePlayer())
            stateMachine.ChangeState(enemy.AdvancePlayerState);

        ChangeCoverIfShould();
        if (stateTimer > 0) return;

        if (WeaponOutOfBullet())
        {
            if (enemyCover.IsUnstoppable() && UnstoppableWalkReady())
            {
                enemyMove.SetAdvanceDuration(weaponCooldown);
                stateMachine.ChangeState(enemy.AdvancePlayerState);
            }

            if (WeaponOnCooldown())
                ResetBulletsFired();
            return;
        }

        if (CanShoot() && enemy.EnemyAim.AimOnPlayer())
            Shoot();
    }

    protected bool AdvancePlayer()
    {
        if (enemyCover.IsUnstoppable())
            return false;

        return !enemy.IsPlayerInAggressRange() && ReadyToLeaveCover();
    }

    protected bool UnstoppableWalkReady()
    {
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, PlayerCtrl.Instance.transform.position);
        bool outOfStoppingDistance = distanceToPlayer > enemyMove.AdvanceStoppingDistance;
        bool unstoppableWalkOnCooldown =
            Time.time < enemyAttack.WeaponData.minWeaponCooldown + enemy.AdvancePlayerState.LastTimeAdvanced;
        return outOfStoppingDistance && !unstoppableWalkOnCooldown;
    }

    #region Cover System

    protected bool ReadyToChangeCover()
    {
        bool inDanger = IsPlayerInClearSight() || IsPlayerClose();
        bool advanceTimeIsOver = Time.time > enemy.AdvancePlayerState.LastTimeAdvanced + enemyMove.AdvanceDuration;
        return inDanger && advanceTimeIsOver;
    }

    protected bool ReadyToLeaveCover() =>
        Time.time > enemyCover.MinCoverTime + enemy.RunToCoverState.LastTimeRunToCover;

    protected bool IsPlayerClose() =>
        Vector3.Distance(enemy.transform.position, PlayerCtrl.Instance.transform.position) <
        enemyCover.SafeDistance;

    protected bool IsPlayerInClearSight()
    {
        Vector3 directionToPlayer = PlayerCtrl.Instance.transform.position - enemy.transform.position;
        if (Physics.Raycast(enemy.transform.position + new Vector3(0, 0.3f, 0), directionToPlayer, out RaycastHit hit))
        {
            return hit.transform.parent == PlayerCtrl.Instance.transform ||
                   hit.transform == PlayerCtrl.Instance.transform;
        }

        return false;
    }

    protected void ChangeCoverIfShould()
    {
        if (enemyCover.coverPerk != CoverPerk.CanTakeAndChangeCover) return;

        coverCheckTimer -= Time.deltaTime;
        if (!(coverCheckTimer < 0)) return;
        coverCheckTimer = 0.5f;
        if (!ReadyToChangeCover() || !ReadyToLeaveCover()) return;
        if (enemyCover.CanGetCover())
            stateMachine.ChangeState(enemy.RunToCoverState);
    }

    #endregion

    #region Weapon

    protected void SetupForFirstAttack()
    {
        if (!firstTimeAttack) return;
        firstTimeAttack = false;
        bulletsPerAttack = enemyAttack.WeaponData.GetBulletsPerAttack();
        weaponCooldown = enemyAttack.WeaponData.GetWeaponCooldown();
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

    #endregion
}