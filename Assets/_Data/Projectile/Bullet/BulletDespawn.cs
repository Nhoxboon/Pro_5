using UnityEngine;

public class BulletDespawn : DespawnByTime
{
    public override void DespawnObject()
    {
        ProjectileSpawner.Instance.Despawn(transform.parent.gameObject);
    }
}