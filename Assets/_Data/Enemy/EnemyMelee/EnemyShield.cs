
using System;
using UnityEngine;

public class EnemyShield : NhoxBehaviour
{
    [SerializeField] protected Enemy enemy;
    protected float durability;

    protected override void Awake()
    {
        base.Awake();
        durability = enemy is EnemyMelee enemyMelee ? enemyMelee.ShieldDurability : 10f;
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

    protected void DisableShield()
    {
        enemy.EnemyAnimator.AnimationSetFloat("chaseIndex", 0);
        gameObject.SetActive(false);
    }
}
