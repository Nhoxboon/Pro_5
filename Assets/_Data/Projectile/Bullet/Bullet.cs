using System;
using System.Collections;
using UnityEngine;

public class Bullet : Projectile
{
    //NOTE: Default bullet speed from which our mass formula is derived
    private const float ReferenceBulletSpeed = 20f;

    [SerializeField] protected SphereCollider col;
    [SerializeField] protected MeshRenderer mr;
    [SerializeField] protected TrailRenderer tr;

    protected bool bulletDisabled;

    protected string fxName = "BulletImpactFX";

    protected Vector3 startPosition;
    protected float flyDistance;
    protected float impactForce = 5f;

    protected virtual void Update()
    {
        FadeTrail();
        DisableBullet();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadSphereCollider();
        LoadMeshRenderer();
        LoadTrailRenderer();
    }

    protected void LoadSphereCollider()
    {
        if (col != null) return;
        col = GetComponent<SphereCollider>();
        Debug.Log(transform.name + " LoadSphereCollider", gameObject);
    }

    protected void LoadMeshRenderer()
    {
        if (mr != null) return;
        mr = GetComponent<MeshRenderer>();
        Debug.Log(transform.name + " LoadMeshRenderer", gameObject);
    }

    protected void LoadTrailRenderer()
    {
        if (tr != null) return;
        tr = GetComponentInChildren<TrailRenderer>(true);
        Debug.Log(transform.name + " LoadTrailRenderer", gameObject);
    }

    protected virtual void OnCollisionEnter(Collision other)
    {
        SpawnFX(other);
        if (other.gameObject.TryGetComponent(out IDamageable damageable))
            damageable.TakeDamage(damage);

        ApplyBulletImpactForEnemy(other);
        ProjectileSpawner.Instance.Despawn(gameObject);
    }

    protected void ApplyBulletImpactForEnemy(Collision other)
    {
        if (!other.gameObject.TryGetComponentInParent(out Enemy enemy)) return;
        Rigidbody hitRb = other.collider.attachedRigidbody;
        Vector3 force = rb.linearVelocity.normalized * impactForce;
        enemy.BulletImpact(force, other.contacts[0].point, hitRb);
    }

    public void BulletSetup(int damage, float flyDis = 100, float impactForce = 100)
    {
        this.damage = damage;
        this.impactForce = impactForce;

        startPosition = transform.position;
        flyDistance = flyDis + 0.5f; //Number based on laserTipLength in PlayerAimVisual
    }

    public void Fire(Vector3 direction, float speed)
    {
        rb.mass = ReferenceBulletSpeed / speed;
        rb.linearVelocity = direction * speed;
    }

    protected void DisableBullet()
    {
        if (!(Vector3.Distance(startPosition, transform.position) > flyDistance) || bulletDisabled) return;
        col.enabled = false;
        mr.enabled = false;
        bulletDisabled = true;

        if (tr.time < 0)
            ProjectileSpawner.Instance.Despawn(gameObject);
    }

    protected void FadeTrail()
    {
        if (Vector3.Distance(startPosition, transform.position) > flyDistance - 1.5f)
            tr.time -= 2 * Time.deltaTime;
    }

    protected void SpawnFX(Collision other)
    {
        if (other.contacts.Length <= 0) return;
        ContactPoint contact = other.contacts[0];
        Transform newFX = FXSpawner.Instance.Spawn(fxName, contact.point, Quaternion.LookRotation(contact.normal));
        newFX.gameObject.SetActive(true);
    }

    protected override void ResetProjectile()
    {
        base.ResetProjectile();
        bulletDisabled = false;
        col.enabled = true;
        mr.enabled = true;
        tr.Clear();
        tr.time = 0.25f;
    }
}