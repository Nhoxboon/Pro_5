using UnityEngine;

public abstract class Hitbox : NhoxBehaviour, IDamageable
{
    [SerializeField] protected float damageMultiplier = 1f;

    public virtual void TakeDamage(int damage)
    {

    }
}
