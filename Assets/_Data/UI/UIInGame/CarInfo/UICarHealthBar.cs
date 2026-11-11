using UnityEngine;
using UnityEngine.UI;

public class UICarHealthBar : NhoxBehaviour
{
    [SerializeField] protected Image healthBar;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadHealthBar();
    }

    protected void LoadHealthBar()
    {
        if(healthBar != null) return;
        healthBar = GetComponentInChildren<Image>();
        Debug.Log(transform.name + " LoadHealthBar", gameObject);
    }

    public void UpdateCarHealthUI(int currentHealth, int maxHealth)
    {
        healthBar.fillAmount = (float)currentHealth / maxHealth;
    }
}
