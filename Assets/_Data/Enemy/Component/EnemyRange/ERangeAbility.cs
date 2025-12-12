
using UnityEngine;

public class ERangeAbility : EnemyAbility
{
    protected EnemyRange enemyRange;
    
    [Header("Grenade Perk")]
    public GrenadePerk grenadePerk;
    [SerializeField] protected Transform grenadeSpawnPoint;
    protected float impactPower = 10f;
    public float timeToTarget = 1.2f;
    protected float explosionTimer = 0.75f;
    protected string grenadePrefab = "EnemyGrenade";
    
    [SerializeField] protected LayerMask whatIsAlly;
    
    protected override void Awake()
    {
        base.Awake();
        enemyRange = enemy as EnemyRange;
        lastTimeUsedAbility = -10;
    }
    
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadAllyLayer();
        LoadGrenadeSpawnPoint();
    }
    
    protected void LoadAllyLayer()
    {
        if(whatIsAlly != 0) return;
        whatIsAlly = LayerMask.GetMask("Enemy");
        Debug.Log(transform.name + " LoadAllyLayer", gameObject);
    }
    
    protected void LoadGrenadeSpawnPoint()
    {
        if (grenadeSpawnPoint != null) return;
        grenadeSpawnPoint = transform.parent.GetComponentInChildren<EnemyRangeWeaponModel>(true).transform.parent.parent
            .Find("GrenadeStartPoint");
        Debug.Log(transform.name + " LoadGrenadeSpawnPoint", gameObject);
    }
    
    public override bool CanUseAbility()
    {
        if(grenadePerk == GrenadePerk.Unavailable) return false;

        if (Vector3.Distance(enemyRange.transform.position, PlayerCtrl.Instance.transform.position) <
            enemyRange.EnemyCover.SafeDistance)
            return false;
        
        return Time.time > abilityCooldown + lastTimeUsedAbility;
    }

    public void ThrowGrenade()
    {
        lastTimeUsedAbility = Time.time;
        Transform newPrefab = ProjectileSpawner.Instance.Spawn(grenadePrefab, grenadeSpawnPoint.position,
            Quaternion.identity);
        newPrefab?.gameObject.SetActive(true);
        
        if (newPrefab is null || !newPrefab.TryGetComponent(out EnemyGrenade grenade)) 
            return;
        if (EnemyDeadAction(grenade)) 
            return; 
        grenade.SetupGrenade(abilityDamage, whatIsAlly, PlayerCtrl.Instance.transform.position, timeToTarget, explosionTimer, impactPower);
    }
    
    protected bool EnemyDeadAction(EnemyGrenade grenade)
    {
        if (enemyRange.StateMachine.currentState != enemyRange.DeadState) return false;
        grenade.SetupGrenade(abilityDamage, whatIsAlly, transform.parent.position, 1, explosionTimer, impactPower);
        return true;
    }
}
