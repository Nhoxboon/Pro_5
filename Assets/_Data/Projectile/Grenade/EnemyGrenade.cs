
using System;
using UnityEngine;

public class EnemyGrenade : Projectile
{
    [SerializeField] protected float impactRadius = 2f;
    [SerializeField] protected float upwardsMultiplier = 1;

    protected string explodeFX = "ExplosionFX";
    protected float impactPower;
    protected float timer;

    protected void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
            Explode();
    }

    public void SetupGrenade(Vector3 target, float timeToTarget, float countdown, float power)
    {
        rb.linearVelocity = CalculateLaunchVelocity(target, timeToTarget);
        timer = countdown + timeToTarget;
        impactPower = power;
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
        Transform newFX = FXSpawner.Instance.Spawn(explodeFX, transform.position, Quaternion.identity);
        newFX.gameObject.SetActive(true);
        ProjectileSpawner.Instance.Despawn(gameObject);
        
        Collider[] cols = Physics.OverlapSphere(transform.position, impactRadius);
        foreach (Collider hit in cols)
        {
            if (hit.TryGetComponent(out Rigidbody rbd))
                rbd.AddExplosionForce(impactPower, transform.position, impactRadius, upwardsMultiplier,
                    ForceMode.Impulse);
        }
    }
    
    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, impactRadius);
    }
}
