using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITransparentOnHover : NhoxBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected float hoverAlphaMultiplier = 0.15f;

    protected Dictionary<Image, Color> originalImageColors = new();
    protected Dictionary<TextMeshProUGUI, Color> originalTextMeshColors = new();

    protected bool hasUIWeaponSlots;

    protected override void Start()
    {
        base.Start();
        hasUIWeaponSlots = GetComponentInChildren<UIWeaponSlot>();

        foreach (Image img in GetComponentsInChildren<Image>(true))
        {
            originalImageColors[img] = img.color;
        }

        foreach (TextMeshProUGUI t in GetComponentsInChildren<TextMeshProUGUI>(true))
        {
            originalTextMeshColors[t] = t.color;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        foreach (var img in originalImageColors.Keys)
        {
            var color = img.color;
            color.a *= hoverAlphaMultiplier;
            img.color = color;
        }

        foreach (var t in originalTextMeshColors.Keys)
        {
            var color = t.color;
            color.a *= hoverAlphaMultiplier;
            t.color = color;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (var img in originalImageColors.Keys)
            img.color = originalImageColors[img];

        foreach (var t in originalTextMeshColors.Keys)
            t.color = originalTextMeshColors[t];

        PlayerCtrl.Instance?.PlayerInventory.UpdateWeaponUI();
    }
}
