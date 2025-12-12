using System;
using UnityEngine;
using System.Collections.Generic;

public class EMeleeAttack : EnemyAttack
{
    [Header("Melee Attack")]
    protected EnemyMelee enemyMelee;
    
    public AttackData_EnemyMelee attackData;
    [SerializeField] protected List<AttackData_EnemyMelee> attackList;
    public List<AttackData_EnemyMelee> AttackList => attackList;
    
    protected EnemyWeaponModel currentWeapon;
    protected string meleeFX = "MeleeImpactFX";

    protected override void Awake()
    {
        base.Awake();
        enemyMelee = enemy as EnemyMelee;
    }

    protected void Update()
    {
        MeleeAttackCheck(currentWeapon.DamagePoints, currentWeapon.AttackRadius, attackData.attackDamage);
    }

    public override void UpdateAttackData()
    {
        currentWeapon = enemyMelee.EnemyVisuals.CurrentWeaponModel.GetComponent<EnemyWeaponModel>();
        if (currentWeapon != null)
        {
            attackList = new List<AttackData_EnemyMelee>(currentWeapon.weaponData.attackData);
            enemy.EnemyMove.SetTurnSpeed(currentWeapon.weaponData.turnSpeed);
        }

        if (attackList is { Count: > 0 })
            attackData = attackList[0];
    }

    protected override void SpawnFX(Transform point)
    {
        Transform newFX = FXSpawner.Instance.Spawn(meleeFX, point.position, Quaternion.identity);
        newFX.gameObject.SetActive(true);
    }
    
    public override bool PlayerInAttackRange() =>
        Vector3.Distance(transform.parent.position, PlayerCtrl.Instance.transform.position) < attackData.attackRange;

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.parent.position, attackData.attackRange);
    }
}