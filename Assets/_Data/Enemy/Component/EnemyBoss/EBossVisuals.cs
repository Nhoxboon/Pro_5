using System;
using UnityEngine;

public class EBossVisuals : NhoxBehaviour
{
    [SerializeField] protected EnemyBoss enemy;
    protected EBossAttack enemyAttack;
    [SerializeField] protected float landingOffset;
    [SerializeField] protected TrailRenderer[] weaponTrails;
    protected string landingZoneFX = "LandingZoneFX";
    
    [Header("Flame Thrower")]
    [SerializeField] protected GameObject[] batteries;
    [SerializeField] protected float initialBatteryScaleY = 0.2f;
    protected float dischargeSpeed;
    protected float rechargeSpeed;
    protected bool isRecharging;

    protected override void Awake()
    {
        base.Awake();
        enemyAttack = enemy.EnemyAttack as EBossAttack;
        EnableWeaponTrail(false);
        
        if(enemyAttack?.WeaponType == BossWeaponType.Fist)
            ResetBatteries();
    }

    protected void Update()
    {
        if(enemyAttack?.WeaponType == BossWeaponType.Fist)
            UpdateBatteriesScale();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadEnemyBoss();
        LoadTrailRenderers();
    }

    protected void LoadEnemyBoss()
    {
        if (enemy != null) return;
        enemy = GetComponentInParent<EnemyBoss>();
        Debug.Log(transform.name + " LoadEnemyBoss", gameObject);
    }

    protected void LoadTrailRenderers()
    {
        if (weaponTrails is { Length: > 0 }) return;
        weaponTrails = transform.parent.GetComponentsInChildren<TrailRenderer>(true);
        Debug.Log(transform.name + " LoadTrailRenderers", gameObject);
    }

    public void EnableWeaponTrail(bool enable)
    {
        foreach (TrailRenderer trail in weaponTrails)
            trail.gameObject.SetActive(enable);
    }

    public void PlaceLandingZoneFX(Vector3 target)
    {
        Vector3 dir = (target - enemy.transform.position).normalized;
        Vector3 offset = dir * landingOffset;
        
        Transform newFX = FXSpawner.Instance.Spawn(landingZoneFX, target + offset, Quaternion.identity);
        if (!newFX.GetChild(0).TryGetComponent(out ParticleSystem ps)) return;
        var mainModule = ps.main;
        mainModule.startLifetime = enemyAttack.TravelTimeToTarget * 2;
        newFX.gameObject.SetActive(true);
    }

    protected void UpdateBatteriesScale()
    {
        if (batteries.Length <= 0) return;
        foreach (GameObject battery in batteries)
        {
            if (!battery.activeSelf) continue;
            float scaleChange = (isRecharging ? rechargeSpeed : -dischargeSpeed) * Time.deltaTime;
            float newScaleY = Mathf.Clamp(battery.transform.localScale.y + scaleChange, 0, initialBatteryScaleY);

            battery.transform.localScale = new Vector3(0.15f, newScaleY, 0.15f);

            if (battery.transform.localScale.y <= 0)
                battery.SetActive(false);
        }
    }

    public void ResetBatteries()
    {
        isRecharging = true;
        rechargeSpeed = initialBatteryScaleY / enemy.EnemyAbility.AbilityCooldown;
        dischargeSpeed = initialBatteryScaleY / (enemy.EnemyAbility.FlameThrowDuration * 0.75f);

        foreach (GameObject battery in batteries)
            battery.SetActive(true);
    }

    public void DischargeBatteries() => isRecharging = false;
}