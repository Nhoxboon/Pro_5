using System;
using UnityEngine;

public class EBossAttack : EnemyAttack
{
    [SerializeField] protected float attackRange = 1.5f;
    [SerializeField] protected BossWeaponType weaponType;
    public BossWeaponType WeaponType => weaponType;

    [Header("Jump Attack")] 
    [SerializeField] protected float minJumpDisRequired = 6f;
    [SerializeField] protected float jumpAtkCooldown = 10f;
    [SerializeField] protected float travelTimeToTarget = 1f;
    public float TravelTimeToTarget => travelTimeToTarget;
    [Space]
    [SerializeField] protected Transform impactPoint;
    [SerializeField] protected float impactRadius = 2.5f;
    [SerializeField] protected float impactPower = 30f;
    protected float upforceMultiplier = 10f;
    [SerializeField] protected LayerMask whatToIgnore;
    protected float lastTimeJumped;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadLayerMask();
    }

    protected void LoadLayerMask()
    {
        if (whatToIgnore != 0) return;
        whatToIgnore = LayerMask.GetMask("Enemy");
        Debug.Log(transform.name + " LoadLayerMask", gameObject);
    }

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

        //TODO: Will change late because now player use hitbox in ragdoll not hitbox
        if (Physics.Raycast(myPos, directionToPlayer, out RaycastHit hit, 100, ~whatToIgnore))
        {
            return hit.transform.parent == PlayerCtrl.Instance.transform ||
                   hit.transform == PlayerCtrl.Instance.transform;
        }
        return false;
    }

    public void JumpImpact()
    {
        Transform impact = impactPoint;
        if(impact == null)
            impact = enemy.transform;
        
        Collider[] cols = Physics.OverlapSphere(impact.position, impactRadius);
        foreach (Collider hit in cols)
        {
            if (hit.TryGetComponent(out Rigidbody rbd))
                rbd.AddExplosionForce(impactPower, impact.position, impactRadius, upforceMultiplier,
                    ForceMode.Impulse);
        }
    }
    
    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(enemy.transform.position, attackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(enemy.transform.position, minJumpDisRequired);
    }
}