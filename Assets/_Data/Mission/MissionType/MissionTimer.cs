using UnityEngine;

[CreateAssetMenu(fileName = "MissionTimer", menuName = "Missions/Mission Timer")]
public class MissionTimer : Mission
{
    public float time;
    protected float currentTime;

    public override void StartMission()
    {
        currentTime = time;
    }

    public override bool MissionCompleted()
    {
        return currentTime > 0;

    }

    // ReSharper disable Unity.PerformanceAnalysis
    public override void UpdateMission()
    {
        base.UpdateMission();
        currentTime -= Time.deltaTime;
        if (currentTime < 0)
            UIManager.Instance.UIInGame.UIMission.UpdateMissionInfo("Mission Failed: Time's up!");

        string timeText = System.TimeSpan.FromSeconds(currentTime).ToString("mm':'ss");

        string missionTxt = "Get to evacuation point before plane take off.";
        string missionDetails = "Time left: " + timeText;
        UIManager.Instance.UIInGame.UIMission.UpdateMissionInfo(missionTxt, missionDetails);
    }
}
