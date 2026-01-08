using System;
using UnityEngine;
using UnityEngine.UI;

public class UIInGame : NhoxBehaviour
{
    [SerializeField] protected UIHealthBar healthBar;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadHealthBar();
    }

    protected void LoadHealthBar()
    {
        if (healthBar != null) return;
        healthBar = transform.GetComponentInChildren<UIHealthBar>();
        Debug.Log(transform.name + " LoadHealthBar", gameObject);
    }
}
