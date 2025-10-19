using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : NhoxBehaviour
{
    [SerializeField] protected EnemyMelee enemy;
    
    public AttackData_EnemyMelee attackData;
    [SerializeField] protected List<AttackData_EnemyMelee> attackList;
    public List<AttackData_EnemyMelee> AttackList => attackList;

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

    public void UpdateAttackData()
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

    public bool PlayerInAttackRange() =>
        Vector3.Distance(transform.parent.position, PlayerCtrl.Instance.transform.position) < attackData.attackRange;

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.parent.position, attackData.attackRange);
    }
}