using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Core : NhoxBehaviour
{
    [SerializeField] protected List<CoreComponent> components = new();

    #region Core Components

    [SerializeField] protected GameObject root;
    public GameObject Root => root;

    [SerializeField] protected Stats stats;
    public Stats Stats => stats;

    [SerializeField] protected DamageReceiver damageReceiver;
    public DamageReceiver DamageReceiver => damageReceiver;
    #endregion

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadRoot();
        LoadStats();
        LoadDamageReceiver();
    }

    protected void LoadRoot()
    {
        if (root != null) return;
        root = transform.parent.gameObject;
        Debug.Log(transform.name + " LoadRoot", gameObject);
    }

    protected void LoadStats()
    {
        if (stats != null) return;
        stats = GetComponentInChildren<Stats>();
        Debug.Log(transform.name + " LoadStats", gameObject);
    }

    protected void LoadDamageReceiver()
    {
        if (damageReceiver != null) return;
        damageReceiver = GetComponentInChildren<DamageReceiver>();
        Debug.Log(transform.name + " LoadDamageReceiver", gameObject);
    }

    public void LogicUpdate()
    {
        foreach (var component in components) component.LogicUpdate();
    }

    public void AddComponent(CoreComponent component)
    {
        if (!components.Contains(component)) components.Add(component);
    }
}