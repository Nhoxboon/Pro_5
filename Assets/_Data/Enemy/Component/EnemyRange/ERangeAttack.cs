using UnityEngine;

public class ERangeAttack : EnemyAttack
{
    protected EnemyRange enemyRange;

    [Header("Range Attack Settings")] public float fireRate = 2f;
    public float bulletSpeed = 20f;
    public int bulletToShoot = 5;
    public float weaponCooldown = 1.5f;

    public Transform GunPoint { get; protected set; }
    protected string bulletPrefab = "EnemyBullet";

    protected override void Awake()
    {
        base.Awake();
        enemyRange = enemy as EnemyRange;
    }

    protected void GetGunPoint()
    {
        if (enemyRange.EnemyVisuals.CurrentWeaponModel.TryGetComponent(out EnemyRangeWeaponModel weapon) &&
            GunPoint != weapon.gunPoint)
            GunPoint = weapon.gunPoint;
    }

    public void FireSingleBullet()
    {
        GetGunPoint();
        enemyRange.EnemyAnimator.AnimationTrigger("attack");
        Vector3 bulletDir = ((PlayerCtrl.Instance.transform.position + Vector3.up) - GunPoint.position).normalized;
        Transform newBullet = ProjectileSpawner.Instance.Spawn(bulletPrefab, GunPoint.position,
            Quaternion.LookRotation(GunPoint.forward));
        newBullet.gameObject.SetActive(true);
        if (!newBullet.TryGetComponent(out EnemyBullet bullet)) return;
        bullet.BulletSetup();
        bullet.Fire(bulletDir, bulletSpeed);
    }
}