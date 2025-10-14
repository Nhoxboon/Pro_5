using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : NhoxBehaviour
{
    public AttackData attackData;
    [SerializeField] protected List<AttackData> attackList;
    public List<AttackData> AttackList => attackList;

    public bool PlayerInAttackRange() =>
        Vector3.Distance(transform.parent.position, PlayerCtrl.Instance.transform.position) < attackData.attackRange;

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.parent.position, attackData.attackRange);
    }
}