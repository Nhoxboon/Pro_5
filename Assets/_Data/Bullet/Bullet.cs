using System;
using UnityEngine;

public class Bullet : NhoxBehaviour
{
    //NOTE: Default bullet speed from which our mass formula is derived
    private const float ReferenceBulletSpeed = 20f;
    
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected TrailRenderer tr;
    protected string fxName = "BulletImpactFX";
    
    protected void OnEnable() => ResetBullet();

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadRigidbody();
        LoadTrailRenderer();
    }

    protected void LoadRigidbody()
    {
        if (rb != null) return;
        rb = GetComponent<Rigidbody>();
        Debug.Log(transform.name + " LoadRigidbody", gameObject);
    }

    protected void LoadTrailRenderer()
    {
        if(tr != null ) return;
        tr = GetComponentInChildren<TrailRenderer>(true);
        Debug.Log(transform.name + " LoadTrailRenderer", gameObject);
    }

    protected void OnCollisionEnter(Collision other)
    {
        SpawnFX(other);
        BulletSpawner.Instance.Despawn(gameObject);
    }

    public void Fire(Vector3 direction, float speed)
    {
        rb.mass = ReferenceBulletSpeed / speed;
        rb.linearVelocity = direction * speed;
    }
    
    protected void SpawnFX(Collision other)
    {
        if (other.contacts.Length <= 0) return;
        ContactPoint contact = other.contacts[0];
        Transform newFX = FXSpawner.Instance.Spawn(fxName, contact.point, Quaternion.LookRotation(contact.normal));
        newFX.gameObject.SetActive(true);
    }

    protected void ResetBullet()
    {
        tr.Clear();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
