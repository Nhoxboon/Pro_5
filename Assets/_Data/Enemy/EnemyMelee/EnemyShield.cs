
using System;
using UnityEngine;

public class EnemyShield : NhoxBehaviour
{
    [SerializeField] protected Enemy enemy;
    [SerializeField] protected float durability = 20f;

    protected override void Awake()
    {
        base.Awake();
        enemy.Core.Stats.ShieldDurability.SetMaxValue(durability);
        enemy.Core.Stats.ShieldDurability.Init();
        
        enemy.Core.Stats.ShieldDurability.OnCurrentValueZero += DisableShield;
    }

    protected void OnDestroy()
    {
        enemy.Core.Stats.ShieldDurability.OnCurrentValueZero -= DisableShield;
    }

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

    public void GetDamage(float damage) => enemy.Core.DamageReceiver.ReduceShieldDurability(damage);

    protected void DisableShield()
    {
        enemy.EnemyAnimator.AnimationSetFloat("chaseIndex", 0);
        gameObject.SetActive(false);
    }
}
