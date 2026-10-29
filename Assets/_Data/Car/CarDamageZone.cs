using System;
using UnityEngine;

public class CarDamageZone : NhoxBehaviour
{
    [SerializeField] protected CarController carCtrl;
    [SerializeField] protected float minSpeedToDamage = 2.5f;

    [SerializeField] protected int carDamage = 200;
    [SerializeField] protected float impactForce = 150f;
    [SerializeField] protected float upwardMultiplier = 3f;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadCarCtrl();
    }

    protected void LoadCarCtrl()
    {
        if(carCtrl != null) return;
        carCtrl = GetComponentInParent<CarController>();
        Debug.Log(transform.name + " :LoadCarCtrl", gameObject);
    }

    protected void OnTriggerEnter(Collider other)
    {
        if(carCtrl.Rb.linearVelocity.magnitude > minSpeedToDamage)
            return;
        ApplyDamageToTarget(other);
        ApplyForceToTarget(other);
    }

    protected void ApplyDamageToTarget(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damageable))
            damageable.TakeDamage(carDamage);
    }

    protected void ApplyForceToTarget(Collider other)
    {
        if(other.TryGetComponent(out Rigidbody rb))
            ApplyForce(rb);
    }

    protected void ApplyForce(Rigidbody rb)
    {
        rb.isKinematic = false;
        rb.AddExplosionForce(impactForce, transform.position, 3, upwardMultiplier, ForceMode.Impulse);
    }
}
