using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MissionSelectBtn : ScaleButton
{
    [SerializeField] protected UIMissionSelection missionUI;

    [SerializeField] protected Mission myMission;

    protected override void Start()
    {
        base.Start();
        buttonText.text = myMission.missionName;
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadUIMissionSel();
    }

    protected void LoadUIMissionSel()
    {
        if (missionUI != null) return;
        missionUI = transform.GetComponentInParent<UIMissionSelection>();
        Debug.Log(transform.name + " LoadUIMissionSel", gameObject);
    }

    protected override void OnClick()
    {
        MissionManager.Instance.SelectMission(myMission);
        UIManager.Instance.SwitchToInGameUI();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        missionUI.UpdateMissionDesc(myMission.missionDescription);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        missionUI.UpdateMissionDesc("Choose a mission");
    }
}