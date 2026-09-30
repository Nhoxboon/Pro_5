using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public abstract class BaseSlider : NhoxBehaviour
{
    [SerializeField] protected Slider sfxSlider;
    [SerializeField] protected TextMeshProUGUI sfxSliderTxt;

    protected virtual void OnEnable() => sfxSlider.onValueChanged.AddListener(SliderValue);

    protected virtual void OnDisable() => sfxSlider.onValueChanged.RemoveListener(SliderValue);

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadSlider();
        LoadSliderTxt();
    }

    protected void LoadSlider()
    {
        if(sfxSlider != null) return;
        sfxSlider = GetComponentInChildren<Slider>();
        Debug.Log(transform.name + ": LoadSlider", gameObject);
    }

    protected void LoadSliderTxt()
    {
        if(sfxSliderTxt  != null) return;
        sfxSliderTxt = GetComponentInChildren<TextMeshProUGUI>();
        Debug.Log(transform.name + ": LoadSliderTxt", gameObject);
    }

    public virtual void SliderValue(float value)
    {
        sfxSliderTxt.text = Mathf.RoundToInt(value * 100f) + "%";
        // sfxSlider.value = value;
    }
}