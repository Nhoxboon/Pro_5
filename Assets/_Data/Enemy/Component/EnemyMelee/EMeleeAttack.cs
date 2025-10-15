using UnityEngine;

public class EMeleeAttack : EnemyAttack
{
    [Header("Axe Throw Ability")]
    protected string axe = "Axe";
    [SerializeField] protected Transform axeSpawnPoint;
    
    [SerializeField] protected float axeFlySpeed = 5f;
    [SerializeField] protected float axeAimTimer;
    [SerializeField] protected float axeThrowCooldown;
    protected float lastTimeAxeThrown;
    
    public float AxeThrowCooldown => axeThrowCooldown;

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
        if(enemy.meleeType != EnemyMeleeType.AxeThrow) return false;
        
        if (!(Time.time > lastTimeAxeThrown + axeThrowCooldown)) return false;
        lastTimeAxeThrown = Time.time;
        return true;
    }

    public void ThrowAxe()
    {
        Transform newAxe = ProjectileSpawner.Instance.Spawn(axe, axeSpawnPoint.position, Quaternion.identity);
        newAxe.gameObject.SetActive(true);
        if(newAxe.TryGetComponent(out EnemyAxe enemyAxe))
            enemyAxe.AxeSetup(axeFlySpeed, PlayerCtrl.Instance.transform, axeAimTimer);
    }
}