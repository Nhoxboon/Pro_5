using System;
using UnityEngine;

public class MissionObj_Key : NhoxBehaviour
{
    public static event Action OnKeyPickedUp;

    protected void OnTriggerEnter(Collider other)
    {
        if(other.transform != PlayerCtrl.Instance.transform) return;

        OnKeyPickedUp?.Invoke();
        Destroy(gameObject);
    }
}
