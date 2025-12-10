using System;
using UnityEngine;

public class EnemyWeaponModel : NhoxBehaviour
{
    public EnemyMeleeWeaponType weaponType;
    public AnimatorOverrideController overrideController;
    public EMeleeWeaponDataSO weaponData;

    [SerializeField] protected TrailRenderer[] tr;
    
    [Header("Damage Attribute")]
    [SerializeField] protected Transform[] damagePoints;
    public Transform[] DamagePoints => damagePoints;
    [SerializeField] protected float attackRadius = 0.3f;
    public float AttackRadius => attackRadius;

    protected override void Awake()
    {
        base.Awake();
        EnableTrail(false);
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadTrailRenderer();
    }

    protected void LoadTrailRenderer()
    {
        if (tr is { Length: > 0 }) return;
        tr = GetComponentsInChildren<TrailRenderer>();
        Debug.Log(transform.name + " LoadTrailRenderer", gameObject);
    }

    public void EnableTrail(bool enable)
    {
        foreach (TrailRenderer trail in tr)
            trail.gameObject.SetActive(enable);
    }

    [ContextMenu("Assign Damage Points")]
    protected void GetDamagePoints()
    {
        damagePoints = new Transform[tr.Length];
        for(int i = 0; i < tr.Length; i++)
        {
            damagePoints[i] = tr[i].transform;
        }
    }

    protected void OnDrawGizmos()
    {
        if (damagePoints.Length == 0) return;
        foreach (Transform point in damagePoints)
            Gizmos.DrawWireSphere(point.position, attackRadius);
    }
}