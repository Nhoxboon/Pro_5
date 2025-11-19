using UnityEngine;

public class EnemyBullet : Bullet
{
    protected override void OnCollisionEnter(Collision other)
    {
        SpawnFX(other);
        ProjectileSpawner.Instance.Despawn(gameObject);

        if (other.gameObject.TryGetComponentInParent(out PlayerCtrl player))
            Debug.Log("Player Hit");
    }
}