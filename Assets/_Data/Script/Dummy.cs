
using System;
using UnityEngine;

public class Dummy : NhoxBehaviour,IDamageable
{
    public int currentHealth;
    public int maxHealth = 100;
    [Space]
    public MeshRenderer mr;
    public Material defaultMat;
    public Material dieMat;
    [Space]
    public float refreshCooldown = 2f;
    protected float lastTimeDamaged;

    protected override void Awake()
    {
        base.Awake();
        Refresh();
    }

    protected void Update()
    {
        if (Time.time > lastTimeDamaged + refreshCooldown || Input.GetKeyDown(KeyCode.B)) 
            Refresh();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadMR();
    }
    
    protected void LoadMR()
    {
        if (mr != null) return;
        mr = GetComponent<MeshRenderer>();
        Debug.Log(transform.name + " LoadMR", gameObject);
    }

    protected void Refresh()
    {
        currentHealth = maxHealth;
        mr.sharedMaterial = defaultMat;
    }

    protected void Die() => mr.sharedMaterial = dieMat;

    public void TakeDamage(int damage)
    {
        lastTimeDamaged = Time.time;
        currentHealth -= damage;
        if(currentHealth <= 0)
            Die();
    }
}
