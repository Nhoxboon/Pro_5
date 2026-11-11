using System;
using UnityEngine;

public class CarHealthController : NhoxBehaviour, IDamageable
{
    [SerializeField] protected CarController car;
    [SerializeField] protected int maxHealth = 100;
    [SerializeField] protected int currentHealth;

    protected bool isBroken;

    protected override void Start()
    {
        base.Start();
        currentHealth = maxHealth;
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadCarCtrl();
    }

    protected void LoadCarCtrl()
    {
        if(car != null) return;
        car = GetComponentInParent<CarController>();
        Debug.Log(transform.name + " :LoadCarCtrl", gameObject);
    }

    protected void ReduceHealth(int amount)
    {
        if(isBroken) return;
        currentHealth -= amount;
        if (currentHealth <= 0)
            BrakeTheCar();
    }

    protected void BrakeTheCar()
    {
        currentHealth = 0;
        isBroken = true;
        car.BrakeTheCar();
        //Enable vfx like smoke,...
    }

    public void TakeDamage(int damage)
    {
        ReduceHealth(damage);
        car.NotifyHealthChanged(currentHealth, maxHealth);
    }

    public void BroadcastCurrentHealth()
    {
        car.NotifyHealthChanged(currentHealth, maxHealth);
    }
}
