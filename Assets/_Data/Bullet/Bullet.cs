using System;
using UnityEngine;

public class Bullet : NhoxBehaviour
{
    [SerializeField] protected Rigidbody rb;
    
    protected void OnEnable() => ResetBullet();

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadRigidbody();
    }

    protected void LoadRigidbody()
    {
        if (rb != null) return;
        rb = GetComponent<Rigidbody>();
        Debug.Log(transform.name + " LoadRigidbody", gameObject);
    }

    protected void OnCollisionEnter(Collision other)
    {
        rb.constraints = RigidbodyConstraints.FreezeAll; 
    }

    public void Fire(Vector3 direction, float speed)
    {
        rb.linearVelocity = direction * speed;
    }

    protected void ResetBullet()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.None;
    }
}
