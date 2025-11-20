using UnityEngine;

public class EnemyWeaponModel : NhoxBehaviour
{
    public EnemyMeleeWeaponType weaponType;
    public AnimatorOverrideController overrideController;
    public EMeleeWeaponDataSO weaponData;

    [SerializeField] protected TrailRenderer[] tr;

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
}