using System;
using UnityEngine;
using System.Collections.Generic;

public class EMeleeAttack : EnemyAttack
{
    protected EnemyMelee enemyMelee;
    
    public AttackData_EnemyMelee attackData;
    [SerializeField] protected List<AttackData_EnemyMelee> attackList;
    public List<AttackData_EnemyMelee> AttackList => attackList;
    
    [SerializeField] protected LayerMask whatIsPlayer;

    protected EnemyWeaponModel currentWeapon;
    protected bool isAttackReady;
    protected string meleeFX = "MeleeImpactFX";

    protected override void Awake()
    {
        base.Awake();
        enemyMelee = enemy as EnemyMelee;
    }

    protected void Update()
    {
        AttackCheck();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadLayerMask();
    }
    
    protected void LoadLayerMask()
    {
        if (whatIsPlayer != 0) return;
        whatIsPlayer = LayerMask.GetMask("Player");
        Debug.Log(transform.name + " LoadLayerMask", gameObject);
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

    public void AttackCheck()
    {
        if(!isAttackReady) return;
        foreach (Transform atkPoint in currentWeapon.DamagePoints)
        {
            Collider[] detectedHits = Physics.OverlapSphere(atkPoint.position, currentWeapon.AttackRadius, whatIsPlayer);

            foreach (var col in detectedHits)
            {
                if (!col.transform.root.TryGetComponent(out PlayerCtrl player)) continue;
                player.Core.DamageReceiver.TakeDamage(1);
                isAttackReady = false;
                SpawnFX(atkPoint);
                return;
            }
        }
    }

    protected void SpawnFX(Transform point)
    {
        Transform newFX = FXSpawner.Instance.Spawn(meleeFX, point.position, Quaternion.identity);
        newFX.gameObject.SetActive(true);
    }
    
    public void EnableAttackCheck(bool enable) => isAttackReady = enable;

    public override bool PlayerInAttackRange() =>
        Vector3.Distance(transform.parent.position, PlayerCtrl.Instance.transform.position) < attackData.attackRange;

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.parent.position, attackData.attackRange);
    }
}