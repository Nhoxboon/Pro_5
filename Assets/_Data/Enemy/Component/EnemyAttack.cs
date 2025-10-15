using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : NhoxBehaviour
{
    [SerializeField] protected EnemyMelee enemy;
    
    public AttackData attackData;
    [SerializeField] protected List<AttackData> attackList;
    public List<AttackData> AttackList => attackList;
    
    protected override void Awake()
    {
        base.Awake();
        if (attackList is { Count: > 0 })
            attackData = attackList[0];
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadEnemyMelee();
    }

    protected void LoadEnemyMelee()
    {
        if(enemy != null) return;
        enemy = GetComponentInParent<EnemyMelee>();
        Debug.Log(transform.name + " LoadEnemyMelee", gameObject);
    }

    public bool PlayerInAttackRange() =>
        Vector3.Distance(transform.parent.position, PlayerCtrl.Instance.transform.position) < attackData.attackRange;

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.parent.position, attackData.attackRange);
    }
}