using UnityEngine;

public class Stats : CoreComponent
{
    public float hp = 100f;
    [SerializeField] protected Stat health;
    public Stat Health => health;
    
    [SerializeField] protected Stat shieldDurability;
    public Stat ShieldDurability => shieldDurability;

    // [SerializeField] protected EnemyDataSO enemyStatsDataSO;
    // public EnemyDataSO EnemyStatsDataSO => enemyStatsDataSO;

    protected override void Awake()
    {
        base.Awake();

        health.SetMaxValue(hp);
        health.Init();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        // LoadEntityStatsDataSO();
    }

    // protected void LoadEntityStatsDataSO()
    // {
    //     // if (enemyStatsDataSO != null) return;
    //     // enemyStatsDataSO = Resources.Load<EnemyDataSO>("Enemy/" + core.Root.transform.name);
    //     // DebugTool.Log(transform.name + " LoadEntityStatsDataSO", gameObject);
    // }
}