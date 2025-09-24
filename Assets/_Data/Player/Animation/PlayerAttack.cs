using UnityEngine;

public class PlayerAttack : NhoxBehaviour
{
    protected string bulletName = "Bullet";

    [SerializeField] protected float bulletSpeed = 10f;
    [SerializeField] protected Transform gunPoint;

    public void ShootBullet()
    {
        Transform prefab =
            BulletSpawner.Instance.Spawn(bulletName, gunPoint.position, Quaternion.LookRotation(gunPoint.forward));
        prefab.gameObject.SetActive(true);
        
        if(prefab.TryGetComponent(out Bullet bullet))
            bullet.Fire(gunPoint.forward, bulletSpeed);
    }
}
