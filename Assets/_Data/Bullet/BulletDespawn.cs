using UnityEngine;

public class BulletDespawn : DespawnByTime
{
    public override void DespawnObject()
    {
        BulletSpawner.Instance.Despawn(transform.parent.gameObject);
    }
}