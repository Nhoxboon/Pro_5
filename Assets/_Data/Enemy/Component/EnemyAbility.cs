
using System;
using UnityEngine;

public class EnemyAbility : NhoxBehaviour
{
    [SerializeField] protected Enemy enemy;

    [Header("Ability")] 
    [SerializeField] protected int abilityDamage = 10;
    public int AbilityDamage => abilityDamage;
    [SerializeField] protected float minAbilityDistance = 6f;
    [SerializeField] protected float abilityCooldown = 5f;
    public float AbilityCooldown => abilityCooldown;
    protected float lastTimeUsedAbility;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadEnemy();
    }

    protected void LoadEnemy()
    {
        if (enemy != null) return;
        enemy = GetComponentInParent<Enemy>();
        Debug.Log(transform.name + " LoadEnemy", gameObject);
    }
    
    public virtual bool CanUseAbility()
    {
        bool playerWithinDistance = Vector3.Distance(enemy.transform.position, PlayerCtrl.Instance.transform.position) < minAbilityDistance;
        
        return Time.time > lastTimeUsedAbility + abilityCooldown && playerWithinDistance;
    }
    
    public virtual void SetAbilityOnCooldown() => lastTimeUsedAbility = Time.time;

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(enemy.transform.position, minAbilityDistance);
    }

    public virtual void ResetAbility()
    {
        lastTimeUsedAbility = -abilityCooldown;
    }
}
