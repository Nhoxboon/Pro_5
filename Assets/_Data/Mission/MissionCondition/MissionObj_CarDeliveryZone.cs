using System;
using UnityEngine;

public class MissionObj_CarDeliveryZone : NhoxBehaviour
{


    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out CarController car))
        {
            car.GetComponent<MissionObj_CarToDeliver>()?.CarDelivery();
        }
    }
}
