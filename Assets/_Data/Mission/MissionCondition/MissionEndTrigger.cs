using System;
using UnityEngine;

public class MissionEndTrigger : NhoxBehaviour
{
    protected void OnTriggerEnter(Collider other)
    {
        if (other.transform != PlayerCtrl.Instance.transform) return;

        if (MissionManager.Instance.MissionCompleted())
            GameManager.Instance.GameCompleted();
    }
}