using UnityEngine;

public class PlayerHitbox : Hitbox
{
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        int newDamage = Mathf.RoundToInt(damage * damageMultiplier);
        PlayerCtrl.Instance.Core.DamageReceiver.TakeDamage(newDamage);
    }
}
