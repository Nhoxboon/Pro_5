using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISettings : NhoxBehaviour
{
    [SerializeField] protected SFXSlider sfxSlider;
    public SFXSlider SfxSlider => sfxSlider;

    [SerializeField] protected BGMSlider bgmSlider;
    public BGMSlider BGMSlider => bgmSlider;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadSFXSlider();
        LoadBGMSlider();
    }

    protected void LoadBGMSlider()
    {
        if(bgmSlider != null) return;
        bgmSlider = GetComponentInChildren<BGMSlider>();
        Debug.Log(transform.name + ": Load BGMSlider Slider", gameObject);
    }

    protected void LoadSFXSlider()
    {
        if(sfxSlider != null) return;
        sfxSlider = GetComponentInChildren<SFXSlider>();
        Debug.Log(transform.name + ": Load SFX Slider", gameObject);
    }
}