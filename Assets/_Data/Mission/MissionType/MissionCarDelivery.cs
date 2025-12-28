using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "MissionCarDelivery", menuName = "Missions/Mission Car Delivery")]
public class MissionCarDelivery : Mission
{
    protected bool carDelivered;

    public override void StartMission()
    {
        FindFirstObjectByType<MissionObj_CarDeliveryZone>(FindObjectsInactive.Include).gameObject.SetActive(true);
        carDelivered = false;
        MissionObj_CarToDeliver.OnCarDelivery += CarDeliveryCompleted;
        Car[] cars = FindObjectsByType<Car>(FindObjectsSortMode.None);
        foreach (Car car in cars)
            car.AddComponent<MissionObj_CarToDeliver>();
    }

    public override bool MissionCompleted()
    {
        return carDelivered;
    }

    protected void CarDeliveryCompleted()
    {
        carDelivered = true;
        MissionObj_CarToDeliver.OnCarDelivery -= CarDeliveryCompleted;
    }
}
