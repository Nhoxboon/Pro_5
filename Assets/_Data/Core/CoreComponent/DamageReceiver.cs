using System;
using UnityEngine;

public class DamageReceiver : CoreComponent
{
    public Action GetDamage;
    [SerializeField] protected Hitbox[] hitboxes;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadHitboxes();
    }

    protected void LoadHitboxes()
    {
        if (hitboxes is { Length: > 0 }) return;
        hitboxes = core.Root.GetComponentsInChildren<Hitbox>();
        Debug.Log(transform.name + " LoadHitboxes", gameObject);
    }

    public void TakeDamage(int damage)
    {
        if (core.Stats.ShieldDurability.CurrentValue > 0)
            ReduceShieldDurability(damage);
        else
            core.Stats.Health.Decrease(damage);
        GetDamage?.Invoke();
    }
    
    protected void ReduceShieldDurability(float value)
    {
        core.Stats.ShieldDurability.Decrease(value);
    }

    //Note: Rigidbody is Kinematic when hit by car. Maybe should reset it if consider use again enemy from pool
    protected void ResetHitboxes()
    {

    }
}