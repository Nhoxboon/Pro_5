using TMPro;
using UnityEngine;

public class UIMission : NhoxBehaviour
{
    [SerializeField] protected TextMeshProUGUI missionText;
    [SerializeField] protected TextMeshProUGUI missionDetails;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadMissionTxt();
        LoadMissionDetailsTxt();
    }

    protected void LoadMissionTxt()
    {
        if (missionText != null) return;
        missionText = transform.Find("MissionTxt").GetComponent<TextMeshProUGUI>();
        Debug.Log(transform.name + " LoadMissionTxt", gameObject);
    }

    protected void LoadMissionDetailsTxt()
    {
        if (missionDetails != null) return;
        missionDetails = transform.Find("MissionDetailsTxt").GetComponent<TextMeshProUGUI>();
        Debug.Log(transform.name + " LoadMissionDetailsTxt", gameObject);
    }

    public void UpdateMissionInfo(string missionName, string details = null)
    {
        missionText.text = missionName;
        missionDetails.text = details;
    }
}
