using System;
using UnityEngine;

public class MissionObj_HuntTarget : NhoxBehaviour
{
    public static event Action OnTargetKilled;

    public void TargetKilled() => OnTargetKilled?.Invoke();
}
