
using System;
using UnityEngine;

public class FlameThrowDamageArea : NhoxBehaviour
{
    [SerializeField] protected EnemyBoss enemy;
    protected float damageCooldown;
    protected float lastTimeDamage;

    protected override void Awake()
    {
        base.Awake();
        damageCooldown = enemy.EnemyAbility.FlameDamageCooldown;
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadEnemyBoss();
    }

    protected void LoadEnemyBoss()
    {
        if(enemy != null) return;
        enemy = GetComponentInParent<EnemyBoss>();
        Debug.Log(transform.name + " LoadEnemyBoss", gameObject);
    }

    protected void OnTriggerStay(Collider other)
    {
        if(!enemy.EnemyAbility.FlameThrowActive) return;
        if(Time.time - lastTimeDamage < damageCooldown) return;

        if (!other.transform.root.TryGetComponentInChildren(out IDamageable damageable)) return;
        damageable.TakeDamage(1);
        lastTimeDamage = Time.time;
    }
}
