using UnityEngine;

[CreateAssetMenu(fileName = "MissionKeyFind", menuName = "Missions/Mission Find Key")]
public class MissionKeyFind : Mission
{
    [SerializeField] protected GameObject key;
    protected bool keyFound = false;

    public override void StartMission()
    {
        MissionObj_Key.OnKeyPickedUp += PickUpKey;

        UIManager.Instance.UIInGame.UIMission.UpdateMissionInfo(missionName, missionDescription);

        Enemy enemy = LevelGenerator.Instance.GetRandomEnemy();
        enemy.GetComponentInChildren<EnemyDrop>()?.GiveKey(key);
        enemy.MakeEnemyStronger();
    }

    public override bool MissionCompleted()
    {
        return keyFound;
    }

    protected void PickUpKey()
    {
        keyFound = true;
        MissionObj_Key.OnKeyPickedUp -= PickUpKey;
        UIManager.Instance.UIInGame.UIMission.UpdateMissionInfo(missionName, "Key found! Mission completed.");
    }
}
