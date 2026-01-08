using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : NhoxBehaviour
{
    [SerializeField] protected Image healthBar;
    protected PlayerCtrl player;

    protected override void Awake()
    {
        base.Awake();
        player = FindFirstObjectByType<PlayerCtrl>();
    }

    protected void OnEnable() => player.Core.Stats.Health.OnValueDecreased += UpdateHealthUI;

    protected void OnDisable() => player.Core.Stats.Health.OnValueDecreased -= UpdateHealthUI;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadHealthBar();
    }

    protected void LoadHealthBar()
    {
        if (healthBar != null) return;
        healthBar = transform.Find("HealthBarFill").GetComponent<Image>();
        Debug.Log(transform.name + " LoadHealthBar", gameObject);
    }

    public void UpdateHealthUI()
    {
        var health = player.Core.Stats.Health;
        healthBar.fillAmount = health.CurrentValue / health.MaxValue;
    }
}
