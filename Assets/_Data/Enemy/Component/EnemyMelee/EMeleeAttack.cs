using UnityEngine;
using System.Collections.Generic;

public class EMeleeAttack : EnemyAttack
{
    protected EnemyMelee enemyMelee;

    public AttackData_EnemyMelee attackData;
    [SerializeField] protected List<AttackData_EnemyMelee> attackList;
    public List<AttackData_EnemyMelee> AttackList => attackList;

    [Header("Axe Throw Ability")] protected string axe = "Axe";
    [SerializeField] protected Transform axeSpawnPoint;

    [SerializeField] protected float axeFlySpeed = 5f;
    [SerializeField] protected float axeAimTimer;
    [SerializeField] protected float axeThrowCooldown;
    protected float lastTimeAxeThrown;
    
    protected override void Awake()
    {
        base.Awake();
        enemyMelee = enemy as EnemyMelee;
    }

    protected override void Start()
    {
        base.Start();
        lastTimeAxeThrown -= axeThrowCooldown;
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadAxeSpawnPoint();
    }

    protected void LoadAxeSpawnPoint()
    {
        if (axeSpawnPoint != null) return;
        axeSpawnPoint = transform.Find("AxeSpawnPoint");
        Debug.Log(transform.name + " LoadAxeSpawnPoint", gameObject);
    }

    public override void UpdateAttackData()
    {
        EnemyWeaponModel currentWeapon = enemy.EnemyVisuals.CurrentWeaponModel.GetComponent<EnemyWeaponModel>();
        if (currentWeapon != null)
        {
            attackList = new List<AttackData_EnemyMelee>(currentWeapon.weaponData.attackData);
            enemy.EnemyMove.SetTurnSpeed(currentWeapon.weaponData.turnSpeed);
        }

        if (attackList is { Count: > 0 })
            attackData = attackList[0];
    }

    public override bool PlayerInAttackRange() =>
        Vector3.Distance(transform.parent.position, PlayerCtrl.Instance.transform.position) < attackData.attackRange;

    public bool CanThrowAxe()
    {
        if (enemyMelee.meleeType != EnemyMeleeType.AxeThrow) return false;

        if (!(Time.time > lastTimeAxeThrown + axeThrowCooldown)) return false;
        lastTimeAxeThrown = Time.time;
        return true;
    }

    public void ThrowAxe()
    {
        Transform newAxe = ProjectileSpawner.Instance.Spawn(axe, axeSpawnPoint.position, Quaternion.identity);
        newAxe.gameObject.SetActive(true);
        if (newAxe.TryGetComponent(out EnemyAxe enemyAxe))
            enemyAxe.AxeSetup(axeFlySpeed, PlayerCtrl.Instance.transform, axeAimTimer);
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.parent.position, attackData.attackRange);
    }
}