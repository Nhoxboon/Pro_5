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

        transform.forward = rb.linearVelocity;
    }

    protected void FixedUpdate()
    {
        rb.linearVelocity = direction.normalized * flySpeed;
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

    protected void OnCollisionEnter(Collision other)
    {
        SpawnFX();
        ProjectileSpawner.Instance.Despawn(gameObject);
        if (!other.transform.root.TryGetComponentInChildren(out IDamageable damageable)) return;
        damageable.TakeDamage(1);
    }

    protected void SpawnFX()
    {
        Transform newFX = FXSpawner.Instance.Spawn(fxName, transform.position, Quaternion.identity);
        newFX.gameObject.SetActive(true);
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