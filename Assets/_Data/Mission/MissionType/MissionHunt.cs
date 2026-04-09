using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "MissionHunt", menuName = "Missions/Mission Hunt")]
public class MissionHunt : Mission
{
    public int amountToKill = 12;
    public EnemyType enemyType;

    protected int killsToGo;

    public override void StartMission()
    {
        killsToGo = amountToKill;
        UpdateMissionUI();
        MissionObj_HuntTarget.OnTargetKilled += EliminateTarget;
        List<Enemy> validEnemies = GetValidEnemies();

        for (int i = 0; i < amountToKill; i++)
        {
            if(validEnemies.Count <= 0) return;
            int randomIndex = Random.Range(0, validEnemies.Count);
            validEnemies[randomIndex].AddComponent<MissionObj_HuntTarget>();
            validEnemies.RemoveAt(randomIndex);
        }
    }

    public override bool MissionCompleted()
    {
        return killsToGo <= 0;
    }

    protected void EliminateTarget()
    {
        killsToGo--;
        UpdateMissionUI();

        if (killsToGo <= 0)
        {
            UIManager.Instance.UIInGame.UIMission.UpdateMissionInfo("Get to the evacuation point.");
            MissionObj_HuntTarget.OnTargetKilled -= EliminateTarget;
        }
    }

    protected List<Enemy> GetValidEnemies()
    {
        List<Enemy> validEnemies = new List<Enemy>();

        if(enemyType == EnemyType.Random)
            validEnemies = LevelGenerator.Instance.Enemies;
        else
            validEnemies.AddRange(LevelGenerator.Instance.Enemies.Where(enemy => enemy.enemyType == enemyType));

        return validEnemies;
    }

    protected void UpdateMissionUI()
    {
        string missionTxt = $"Eliminate {amountToKill} enemies with signal disruptor.";
        string missionDetails = $"Target left: {killsToGo}";

        UIManager.Instance.UIInGame.UIMission.UpdateMissionInfo(missionTxt, missionDetails);
    }
}
