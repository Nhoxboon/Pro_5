using TMPro;
using UnityEngine;

public class UIMissionSelection : NhoxBehaviour
{
    [SerializeField] protected TextMeshProUGUI missionDesc;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadMissionDesc();
    }

    protected void LoadMissionDesc()
    {
        if (missionDesc != null) return;
        missionDesc = transform.Find("MissionDesc").GetComponentInChildren<TextMeshProUGUI>(true);
        Debug.Log(transform.name + " LoadMissionDesc", gameObject);
    }

    public void UpdateMissionDesc(string newDesc) => missionDesc.text = newDesc;
}