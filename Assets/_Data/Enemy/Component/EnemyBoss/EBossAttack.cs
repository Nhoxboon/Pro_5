using System;
using UnityEngine;

public class EBossAttack : EnemyAttack
{
    [SerializeField] protected float attackRange = 1.5f;

    [Header("Jump Attack")] [SerializeField]
    protected float minJumpDisRequired = 6f;

    [SerializeField] protected float jumpAtkCooldown = 10f;
    [SerializeField] protected float travelTimeToTarget = 1f;
    public float TravelTimeToTarget => travelTimeToTarget;
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

    public bool CanJumpAttack()
    {
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, PlayerCtrl.Instance.transform.position);
        if (distanceToPlayer < minJumpDisRequired) return false;

        if (Time.time > lastTimeJumped + jumpAtkCooldown + enemy.EnemyAnimator.GetAnimationClipDuration("Jump Attack")
            && IsPlayerInClearSight())
        {
            lastTimeJumped = Time.time;
            return true;
        }

        return false;
    }

    public bool IsPlayerInClearSight()
    {
        Vector3 myPos = enemy.transform.position + new Vector3(0, 1.5f, 0);
        Vector3 playerPos = PlayerCtrl.Instance.transform.position + Vector3.up;
        Vector3 directionToPlayer = (playerPos - myPos).normalized;

        if (Physics.Raycast(myPos, directionToPlayer, out RaycastHit hit, 100, ~whatToIgnore))
        {
            return hit.transform.parent == PlayerCtrl.Instance.transform ||
                   hit.transform == PlayerCtrl.Instance.transform;
        }

        return false;
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(enemy.transform.position, attackRange);

        if (PlayerCtrl.Instance != null)
        {
            Vector3 myPos = enemy.transform.position + new Vector3(0, 1.5f, 0);
            Vector3 playerPos = PlayerCtrl.Instance.transform.position + Vector3.up;
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(myPos, playerPos);
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(enemy.transform.position, minJumpDisRequired);
    }
}