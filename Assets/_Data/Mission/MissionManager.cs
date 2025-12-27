using System;
using UnityEngine;

public class MissionManager : NhoxSingleton<MissionManager>
{
    public Mission currentMission;

    protected override void Start()
    {
        base.Start();
        Invoke(nameof(StartMission), 2f);
    }

    protected void Update()
    {
        currentMission?.UpdateMission();
    }

    protected void StartMission() => currentMission.StartMission();

    public bool MissionCompleted() => currentMission.MissionCompleted();
}
