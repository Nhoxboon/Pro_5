using UnityEngine;
using UnityEngine.UI;

public abstract class BaseBtn : NhoxBehaviour
{
    [Header("Base Button")] 
    [SerializeField] protected Button button;

    protected virtual void OnEnable() => button.onClick.AddListener(OnClick);

    protected virtual void OnDisable() => button.onClick.RemoveListener(OnClick);

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadButton();
    }

    protected void LoadButton()
    {
        if (button != null) return;
        button = GetComponent<Button>();
        Debug.Log(transform.name + " :LoadButton", gameObject);
    }

    protected abstract void OnClick();
}