
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGrenade : Projectile
{
    [SerializeField] protected float impactRadius = 2f;
    [SerializeField] protected float upwardsMultiplier = 1;

    protected string explodeFX = "ExplosionFX";
    protected float impactPower;
    protected float timer;

    protected LayerMask allyLayerMask;
    protected bool canExplode = true;

    protected void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0 && canExplode)
            Explode();
    }

    public void SetupGrenade(LayerMask allyLayer, Vector3 target, float timeToTarget, float countdown, float power)
    {
        allyLayerMask = allyLayer;
        rb.linearVelocity = CalculateLaunchVelocity(target, timeToTarget);
        timer = countdown + timeToTarget;
        impactPower = power;
    }

    protected bool IsTargetValid(Collider col)
    {
        if(GameManager.Instance.friendlyFire) 
            return true;
        return (allyLayerMask.value & (1 << col.gameObject.layer)) <= 0;
    }
    
    protected Vector3 CalculateLaunchVelocity(Vector3 target, float timeToTarget)
    {
        Vector3 dir = target - transform.position;
        Vector3 dirXZ = new Vector3(dir.x, 0, dir.z);
        
        Vector3 velocityXZ = dirXZ / timeToTarget;
        float velocityY = (dir.y - (Physics.gravity.y * Mathf.Pow(timeToTarget, 2)) / 2) / timeToTarget;
        
        Vector3 launchVelocity = velocityXZ + Vector3.up * velocityY;
        return launchVelocity;
    }

    protected void Explode()
    {
        canExplode = false;
        SpawnFX();
        ProjectileSpawner.Instance.Despawn(gameObject);

        HashSet<Transform> uniqueEntities = new HashSet<Transform>();
        
        Collider[] cols = Physics.OverlapSphere(transform.position, impactRadius);
        foreach (Collider hit in cols)
        {
            if (!IsTargetValid(hit)) continue;
            
            Transform root = hit.transform.root;
            if (!uniqueEntities.Add(root)) continue;
            
            ApplyDamage(root);
            ApplyPhysicalForce(hit);
        }
    }

    protected void ApplyPhysicalForce(Collider hit)
    {
        if (hit.TryGetComponent(out Rigidbody rbd))
            rbd.AddExplosionForce(impactPower, transform.position, impactRadius, upwardsMultiplier,
                ForceMode.Impulse);
    }

    protected void ApplyDamage(Transform target)
    {
        if (target.TryGetComponent(out PlayerCtrl player))
            player.Core.DamageReceiver.TakeDamage(1);
        else if(target.TryGetComponent(out Enemy enemy))
            enemy.Core.DamageReceiver.TakeDamage(1);
    }

    protected void SpawnFX()
    {
        Transform newFX = FXSpawner.Instance.Spawn(explodeFX, transform.position, Quaternion.identity);
        newFX.gameObject.SetActive(true);
    }

    protected override void ResetProjectile()
    {
        base.ResetProjectile();
        canExplode = true;
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, impactRadius);
    }
}
