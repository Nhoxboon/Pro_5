using System;
using UnityEngine;

public class MissionManager : NhoxSingleton<MissionManager>
{
    public Mission currentMission;

    protected void Update()
    {
        currentMission?.UpdateMission();
    }

    public void StartMission() => currentMission?.StartMission();

    public bool MissionCompleted() => currentMission != null && currentMission.MissionCompleted();

    public void SelectMission(Mission newMission)
    {
        currentMission = newMission;
    }
}