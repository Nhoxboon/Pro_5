using System;
using UnityEngine;

public class EnemyAxe : Projectile
{
    [SerializeField] protected Transform axeModel;

    protected float flySpeed;
    protected Transform player;
    protected Vector3 direction;
    protected float rotationSpeed;
    protected float timer = 1f;
    
    protected string fxName = "AxeImpactFX";
    
    protected void Update()
    {
        axeModel.Rotate(Vector3.right * (rotationSpeed * Time.deltaTime));
        timer -= Time.deltaTime;

        if (timer > 0)
            direction = player.position + Vector3.up - transform.position;

        rb.linearVelocity = direction.normalized * flySpeed;
        transform.forward = rb.linearVelocity;
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadAxeModel();
    }

    protected void LoadAxeModel()
    {
        if (axeModel != null) return;
        axeModel = transform.Find("Model");
        Debug.Log(transform.name + " LoadAxeModel", gameObject);
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Bullet _) && !other.TryGetComponent(out PlayerCtrl _)) return;
        Transform newFX = FXSpawner.Instance.Spawn(fxName, transform.position, Quaternion.identity);
        newFX.gameObject.SetActive(true);
            
        ProjectileSpawner.Instance.Despawn(gameObject);
    }

    public void AxeSetup(float speed, Transform player, float time)
    {
        rotationSpeed = 1600f;
        flySpeed = speed;
        this.player = player;
        timer = time;
    }
    
    protected override void ResetProjectile()
    {
        base.ResetProjectile();
        transform.rotation = Quaternion.identity;
    }
}