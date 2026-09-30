using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FriendlyFireToggle : NhoxBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Toggle friendlyFireToggle;
    [SerializeField] protected TextMeshProUGUI toolTipTxt;

    protected void OnEnable()
    {
        friendlyFireToggle.onValueChanged.AddListener(delegate { OnFriendlyFireToggle(); });
    }

    protected void OnDisable()
    {
        friendlyFireToggle.onValueChanged.RemoveListener(delegate { OnFriendlyFireToggle(); });
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadFriendlyFireToggle();
    }

    protected void LoadFriendlyFireToggle()
    {
        if(friendlyFireToggle != null) return;
        friendlyFireToggle = GetComponentInChildren<Toggle>();
        Debug.Log(transform.name + ": Load Friendly Fire Toggle", gameObject);
    }

    public void OnFriendlyFireToggle()
    {
        bool friendlyFire = GameManager.Instance.friendlyFire;
        GameManager.Instance.friendlyFire = !friendlyFire;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        toolTipTxt?.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        toolTipTxt?.gameObject.SetActive(false);
    }
}
