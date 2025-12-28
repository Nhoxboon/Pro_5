using UnityEngine;

public class EnemyHitbox : Hitbox
{
    [SerializeField] protected Enemy enemy;

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

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        int newDamage = Mathf.RoundToInt(damage * damageMultiplier);
        enemy.Core.DamageReceiver.TakeDamage(newDamage);
    }
}
