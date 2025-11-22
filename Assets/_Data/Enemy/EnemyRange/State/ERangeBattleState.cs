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

    protected float coverCheckTimer;

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

        enemy.EnemyMove.Agent.isStopped = true;
        enemy.EnemyMove.Agent.velocity = Vector3.zero;
        enemyVisuals.EnableIK(true, true);
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
        if(enemy.EnemyAim.IsSeeingPlayer())
            enemy.EnemyMove.FaceTarget(enemy.EnemyAim.aim.position);
        
        if (!enemy.IsPlayerInAggressRange() && ReadyToLeaveCover())
            stateMachine.ChangeState(enemy.AdvancePlayerState);
        
        ChangeCoverIfShould();

        if (WeaponOutOfBullet())
        {
            if (WeaponOnCooldown())
                ResetBulletsFired();
            return;
        }

        if (CanShoot() && enemy.EnemyAim.AimOnPlayer())
            Shoot();
    }

    #region Cover System

    protected bool ReadyToChangeCover()
    {
        bool inDanger = IsPlayerInClearSight() || IsPlayerClose();
        bool advanceTimeIsOver = Time.time > enemy.AdvancePlayerState.LastTimeAdvanced + enemy.EnemyMove.AdvanceTime;
        return inDanger && advanceTimeIsOver;
    }

    protected bool ReadyToLeaveCover() =>
        Time.time > enemy.EnemyCover.MinCoverTime + enemy.RunToCoverState.LastTimeRunToCover;

    protected bool IsPlayerClose() =>
        Vector3.Distance(enemy.transform.position, PlayerCtrl.Instance.transform.position) <
        enemy.EnemyCover.SafeDistance;

    protected bool IsPlayerInClearSight()
    {
        Vector3 directionToPlayer = PlayerCtrl.Instance.transform.position - enemy.transform.position;
        if (Physics.Raycast(enemy.transform.position + new Vector3(0, 0.3f, 0), directionToPlayer, out RaycastHit hit))
        {
            if (hit.collider.transform.parent is not null && 
                hit.collider.transform.parent.TryGetComponent(out PlayerCtrl _))
                return true;
        }

        return false;
    }

    protected void ChangeCoverIfShould()
    {
        if(enemy.EnemyCover.coverPerk != CoverPerk.CanTakeAndChangeCover) return;
        
        coverCheckTimer -= Time.deltaTime;
        if (!(coverCheckTimer < 0)) return;
        coverCheckTimer = 0.5f;
        if (!ReadyToChangeCover()) return;
        if (enemy.EnemyCover.CanGetCover())
            stateMachine.ChangeState(enemy.RunToCoverState);
    }
    #endregion

    #region Weapon

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