
using UnityEngine;

public class Projectile : NhoxBehaviour
{
    protected int damage;
    [SerializeField] protected Rigidbody rb;
    
    protected virtual void OnEnable() => ResetProjectile();
    
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadRigidbody();
    }

    protected virtual void LoadRigidbody()
    {
        if (rb != null) return;
        rb = GetComponent<Rigidbody>();
        Debug.Log(transform.name + " LoadRigidbody", gameObject);
    }
    
    protected virtual void ResetProjectile()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
