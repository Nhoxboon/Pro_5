using UnityEngine;

public class EnemyAttack : NhoxBehaviour
{
    [Header("Base")]
    [SerializeField] protected Enemy enemy;
    
    [SerializeField] protected LayerMask whatIsAlly;
    [SerializeField] protected LayerMask whatIsPlayer;
    protected bool isAttackReady;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadEnemy();
        LoadAllyLayer();
        LoadPlayerLayer();
    }

    protected void LoadAllyLayer()
    {
        if(whatIsAlly != 0) return;
        whatIsAlly = LayerMask.GetMask("Enemy");
        Debug.Log(transform.name + " LoadAllyLayer", gameObject);
    }
    
    protected void LoadPlayerLayer()
    {
        if (whatIsPlayer != 0) return;
        whatIsPlayer = LayerMask.GetMask("Player");
        Debug.Log(transform.name + " LoadAllyLayer", gameObject);
    }

    protected void LoadEnemy()
    {
        if (enemy != null) return;
        enemy = GetComponentInParent<Enemy>();
        Debug.Log(transform.name + " LoadEnemy", gameObject);
    }

    public virtual void UpdateAttackData()
    {
        //For override
    }

    public virtual bool PlayerInAttackRange() => false;

    protected virtual void MeleeAttackCheck(Transform[] damagePoints, float attackRadius, int damage)
    {
        if(!isAttackReady) return;
        foreach (Transform atkPoint in damagePoints)
        {
            Collider[] detectedHits = Physics.OverlapSphere(atkPoint.position, attackRadius, whatIsPlayer);

            foreach (var col in detectedHits)
            {
                if (!col.transform.root.TryGetComponent(out PlayerCtrl player)) continue;
                player.Core.DamageReceiver.TakeDamage(damage);
                isAttackReady = false;
                SpawnFX(atkPoint);
                return;
            }
        }
    }
    
    public void EnableMeleeAttack(bool enable) => isAttackReady = enable;
    
    protected virtual void SpawnFX(Transform point)
    {
        //For override
    }

    public virtual void ResetAttackCooldown()
    {
        //For override
    }
}