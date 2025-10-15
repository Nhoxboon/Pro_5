using System;

public class DamageReceiver : CoreComponent
{
    public Action GetDamage;
    
    public virtual void TakeDamage(float damage)
    {
        // if (core.Stats.ShieldDurability.CurrentValue > 0)
        //     core.Stats.ShieldDurability.Decrease(damage);
        // else
        core.Stats.Health.Decrease(damage);
        GetDamage?.Invoke();
    }
    
    public void ReduceShieldDurability(float value)
    {
        core.Stats.ShieldDurability.Decrease(value);
    }
}