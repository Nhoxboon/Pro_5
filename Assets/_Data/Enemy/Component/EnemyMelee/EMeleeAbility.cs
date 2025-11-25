
using UnityEngine;

public class EMeleeAbility : EnemyAbility
{
    protected EnemyMelee enemyMelee;
    
    [Header("Axe Throw Ability")] 
    protected string axe = "Axe";
    [SerializeField] protected Transform axeSpawnPoint;

    [SerializeField] protected float axeFlySpeed = 10f;
    [SerializeField] protected float axeAimTimer = 0.3f;
    [SerializeField] protected float axeThrowCooldown = 3f;
    protected float lastTimeAxeThrown;
    
    protected override void Awake()
    {
        base.Awake();
        enemyMelee = enemy as EnemyMelee;
    }
    
    protected override void Start()
    {
        base.Start();
        lastTimeAxeThrown -= axeThrowCooldown;
    }
    
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadAxeSpawnPoint();
    }

    protected void LoadAxeSpawnPoint()
    {
        if (axeSpawnPoint != null) return;
        axeSpawnPoint = transform.Find("AxeSpawnPoint");
        Debug.Log(transform.name + " LoadAxeSpawnPoint", gameObject);
    }
    
    public bool CanThrowAxe()
    {
        if (enemyMelee.meleeType != EnemyMeleeType.AxeThrow) return false;

        if (!(Time.time > lastTimeAxeThrown + axeThrowCooldown)) return false;
        lastTimeAxeThrown = Time.time;
        return true;
    }

    public void ThrowAxe()
    {
        Transform newAxe = ProjectileSpawner.Instance.Spawn(axe, axeSpawnPoint.position, Quaternion.identity);
        newAxe.gameObject.SetActive(true);
        if (newAxe.TryGetComponent(out EnemyAxe enemyAxe))
            enemyAxe.AxeSetup(axeFlySpeed, PlayerCtrl.Instance.transform, axeAimTimer);
    }
}
