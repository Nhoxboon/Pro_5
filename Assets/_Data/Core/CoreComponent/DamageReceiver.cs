using System;

public class DamageReceiver : CoreComponent, IDamageable
{
    public Action GetDamage;
    
    public virtual void TakeDamage(int damage)
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
}