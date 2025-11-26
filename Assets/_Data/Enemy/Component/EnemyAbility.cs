
using UnityEngine;

public class EnemyAbility : NhoxBehaviour
{
    [SerializeField] protected Enemy enemy;
    
    [Header("Ability Cooldown")]
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
    
    public virtual bool CanUseAbility() => Time.time > lastTimeUsedAbility + abilityCooldown;
    
    public virtual void SetAbilityOnCooldown() => lastTimeUsedAbility = Time.time;
}
