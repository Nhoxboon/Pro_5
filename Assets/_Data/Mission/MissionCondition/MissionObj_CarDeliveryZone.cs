using System;
using UnityEngine;

public class MissionObj_CarDeliveryZone : NhoxBehaviour
{


    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Car>(out Car car))
        {
            car.GetComponent<MissionObj_CarToDeliver>()?.CarDelivery();
        }
    }
}
