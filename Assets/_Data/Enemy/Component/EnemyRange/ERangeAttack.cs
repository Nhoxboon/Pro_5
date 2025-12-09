using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ERangeAttack : EnemyAttack
{
    protected EnemyRange enemyRange;

    [Header("Weapon Details")] 
    [SerializeField] protected EnemyRangeWeaponType weaponType;
    public EnemyRangeWeaponType WeaponType => weaponType;
    [SerializeField] protected float attackDelay = 1f;
    public float AttackDelay => attackDelay;

    public ERangeWeaponDataSO WeaponData { get; protected set; }
    public Transform GunPoint { get; protected set; }
    protected string bulletPrefab = "EnemyBullet";
    [SerializeField] protected List<ERangeWeaponDataSO> availableWeaponsData;
    
    [Header("Grenade Perk")]
    public GrenadePerk grenadePerk;
    [SerializeField] protected float grenadeCooldown = 4f;
    [SerializeField] protected Transform grenadeSpawnPoint;
    protected float impactPower = 10f;
    public float timeToTarget = 1.2f;
    protected float explosionTimer = 0.75f;
    protected string grenadePrefab = "EnemyGrenade";
    protected float lastTimeGrenadeThrown = -10f;
    
    protected override void Awake()
    {
        base.Awake();
        enemyRange = enemy as EnemyRange;
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadGrenadeSpawnPoint();
    }

    protected void LoadGrenadeSpawnPoint()
    {
        if (grenadeSpawnPoint != null) return;
        grenadeSpawnPoint = transform.parent.GetComponentInChildren<EnemyRangeWeaponModel>().transform.parent.parent
            .Find("GrenadeStartPoint");
        Debug.Log(transform.name + " LoadGrenadeSpawnPoint", gameObject);
    }

    protected void GetGunPoint()
    {
        if (enemyRange.EnemyVisuals.CurrentWeaponModel.TryGetComponent(out EnemyRangeWeaponModel weapon) &&
            GunPoint != weapon.gunPoint)
            GunPoint = weapon.gunPoint;
    }

    public override void UpdateAttackData()
    {
        List<ERangeWeaponDataSO> filterData =
            availableWeaponsData.Where(wpData => wpData.weaponType == weaponType).ToList();

        if (filterData.Count == 0) return;
        int random = Random.Range(0, filterData.Count);
        WeaponData = filterData[random];

        GetGunPoint();
    }

    public void FireSingleBullet()
    {
        enemyRange.EnemyAnimator.AnimationTrigger("attack");
        Vector3 bulletDir = (enemyRange.EnemyAim.aim.position - GunPoint.position).normalized;
        Transform newBullet = ProjectileSpawner.Instance.Spawn(bulletPrefab, GunPoint.position,
            Quaternion.LookRotation(GunPoint.forward));
        newBullet.gameObject.SetActive(true);

        if (!newBullet.TryGetComponent(out Bullet bullet)) return;
        bullet.BulletSetup();
        bullet.Fire(WeaponData.ApplyWeaponSpread(bulletDir), WeaponData.bulletSpeed);
    }

    public bool CanThrowGrenade()
    {
        if(grenadePerk == GrenadePerk.Unavailable) return false;

        if (Vector3.Distance(enemyRange.transform.position, PlayerCtrl.Instance.transform.position) <
            enemyRange.EnemyCover.SafeDistance)
            return false;
        
        if(Time.time > grenadeCooldown + lastTimeGrenadeThrown)
            return true;
        return false;
    }

    public void ThrowGrenade()
    {
        lastTimeGrenadeThrown = Time.time;
        Transform newPrefab = ProjectileSpawner.Instance.Spawn(grenadePrefab, grenadeSpawnPoint.position,
            Quaternion.identity);
        newPrefab?.gameObject.SetActive(true);
        if (newPrefab is null || !newPrefab.TryGetComponent(out EnemyGrenade grenade)) return;
        if(enemyRange.StateMachine.currentState == enemyRange.DeadState)
        {
            grenade.SetupGrenade(transform.parent.position, 1, explosionTimer, impactPower);
            return;
        } 
        grenade.SetupGrenade(PlayerCtrl.Instance.transform.position, timeToTarget, explosionTimer, impactPower);
    }
}