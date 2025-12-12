using System;
using System.Collections.Generic;
using UnityEngine;

public class EBossAttack : EnemyAttack
{
    protected EnemyBoss enemyBoss;
    
    [SerializeField] protected int meleeAtkDamage = 40;
    [SerializeField] protected float attackRange = 1.3f;
    [SerializeField] protected BossWeaponType weaponType;
    public BossWeaponType WeaponType => weaponType;

    [Header("Jump Attack")] 
    [SerializeField] protected int jumpAtkDamage = 15;
    [SerializeField] protected float minJumpDisRequired = 4f;
    [SerializeField] protected float jumpAtkCooldown = 10f;
    [SerializeField] protected float travelTimeToTarget = 1f;
    public float TravelTimeToTarget => travelTimeToTarget;
    
    [Space]
    [SerializeField] protected float impactRadius = 2.5f;
    [SerializeField] protected float impactPower = 30f;
    protected float upforceMultiplier = 10f;
    protected float lastTimeJumped;
    
    [Header("Attack")]
    [SerializeField] protected Transform[] damagePoints;
    [SerializeField] protected float attackRadius = 0.5f;

    [Header("Hammer Ability")] 
    [SerializeField] protected float hammerRadius = 1.3f;
    protected string meleeFX = "BossMeleeImpactFX";

    protected override void Awake()
    {
        base.Awake();
        enemyBoss = enemy as EnemyBoss;
    }

    protected void Update() => MeleeAttackCheck(damagePoints, attackRadius, meleeAtkDamage);

    public override bool PlayerInAttackRange() =>
        Vector3.Distance(enemy.transform.position, PlayerCtrl.Instance.transform.position) < attackRange;
    
    public void SetJumpAtkOnCooldown() => lastTimeJumped = Time.time;

    public bool CanJumpAttack()
    {
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, PlayerCtrl.Instance.transform.position);
        if (distanceToPlayer < minJumpDisRequired) return false;

        return Time.time > lastTimeJumped + jumpAtkCooldown && IsPlayerInClearSight();
    }

    public bool IsPlayerInClearSight()
    {
        Vector3 myPos = enemy.transform.position + new Vector3(0, 1.5f, 0);
        Vector3 playerPos = PlayerCtrl.Instance.transform.position + Vector3.up;
        Vector3 directionToPlayer = (playerPos - myPos).normalized;

        if (Physics.Raycast(myPos, directionToPlayer, out RaycastHit hit, 100, ~whatIsAlly))
            return hit.transform.root == PlayerCtrl.Instance.transform;
        
        return false;
    }

    public void JumpImpact()
    {
        Transform impactPoint = GetImpactPoint();
        MassDamage(jumpAtkDamage, impactPoint.position, impactRadius);
    }
    
    protected Transform GetImpactPoint()
    {
        if (enemy is EnemyBoss enemyBoss && enemyBoss.EnemyAbility.ImpactPoint != null)
            return enemyBoss.EnemyAbility.ImpactPoint;
    
        return enemy.transform;
    }

    public void MassDamage(int damage, Vector3 impactPoint, float impactRad)
    {
        HashSet<Transform> uniqueEntities = new HashSet<Transform>();
        Collider[] cols = Physics.OverlapSphere(impactPoint, impactRad, ~whatIsAlly);
        foreach (Collider hit in cols)
        {
            Transform root = hit.transform.root;
            if (root.TryGetComponentInChildren(out IDamageable damageable))
            {
                if (!uniqueEntities.Add(root)) continue;
                damageable.TakeDamage(damage);
            }
            ApplyPhysicalForce(impactPoint, impactRad, hit);
        }
    }

    protected void ApplyPhysicalForce(Vector3 impactPoint, float impactRad, Collider hit)
    {
        if (hit.TryGetComponent(out Rigidbody rbd))
            rbd.AddExplosionForce(impactPower, impactPoint, impactRad, upforceMultiplier,
                ForceMode.Impulse);
    }

    public void ApplyHammerAbilityDamage(int abilityDamage) =>
        MassDamage(abilityDamage, damagePoints[0].position, hammerRadius);

    protected override void SpawnFX(Transform point)
    {
        Transform newFX = FXSpawner.Instance.Spawn(meleeFX, point.position, Quaternion.identity);
        newFX.gameObject.SetActive(true);
    }
    
    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(enemy.transform.position, attackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(enemy.transform.position, minJumpDisRequired);

        // if (damagePoints.Length > 0) 
        // {
        //     foreach (var point in damagePoints)
        //     {
        //         Gizmos.color = Color.yellow;
        //         Gizmos.DrawWireSphere(point.position, attackRadius);
        //     }
        //     Gizmos.DrawWireSphere(damagePoints[0].position, hammerRadius);
        // }
    }
}