using System;
using UnityEngine;

public class MissionObj_CarToDeliver : NhoxBehaviour
{
    public static event Action OnCarDelivery;

    public void CarDelivery() => OnCarDelivery?.Invoke();
}
