using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "MissionLastDefense", menuName = "Missions/Mission Defense")]
public class MissionLastDefense : Mission
{
    protected bool defenseBegun = false;
    protected string defenseTimerText;

    [Header("Cooldown and Duration")]
    public float defenseDuration = 120f;
    protected float defenseTimer;
    public float waveCooldown = 15f;
    protected float waveTimer;

    [Header("Respawn Details")]
    public int amountRespawnPoints = 2;
    public List<Transform> respawnPoints;
    protected Vector3 defensePoint;

    public int enemyPerWave = 5;
    public GameObject[] possibleEnemies;

    protected void OnEnable()
    {
        defenseBegun = false;
    }

    public override void StartMission()
    {
        defensePoint = FindFirstObjectByType<MissionEndTrigger>().transform.position;
        respawnPoints = new List<Transform>(ClosestRespawnPoints(amountRespawnPoints));
    }

    public override bool MissionCompleted()
    {
        if(!defenseBegun)
        {
            StartDefenseEvent();
            return false;
        }
        return defenseTimer <= 0f;
    }

    public override void UpdateMission()
    {
        base.UpdateMission();
        if (!defenseBegun) return;
        defenseTimer -= Time.deltaTime;
        waveTimer -= Time.deltaTime;

        if (!(waveTimer <= 0f)) return;
        CreateNewEnemies(enemyPerWave);
        waveTimer = waveCooldown;

        defenseTimerText = TimeSpan.FromSeconds(defenseTimer).ToString("mm':'ss");
    }

    protected void StartDefenseEvent()
    {
        waveTimer = 0.5f;
        defenseTimer = defenseDuration;
        defenseBegun = true;
    }

    protected void CreateNewEnemies(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Transform randomRespawnPoint = respawnPoints[Random.Range(0, respawnPoints.Count)];
            int randomEnemyIndex = Random.Range(0, possibleEnemies.Length);
            GameObject randomEnemy = possibleEnemies[randomEnemyIndex];

            if (!randomEnemy.TryGetComponent(out Enemy _)) continue;
            Transform newEnemy = EnemySpawner.Instance.Spawn(randomEnemy.name, randomRespawnPoint.position, Quaternion.identity);
            newEnemy?.gameObject.SetActive(true);
            if (newEnemy is not null && newEnemy.TryGetComponent(out Enemy spawnedEnemy))
                spawnedEnemy.aggroRange = 1000;
        }
    }

    protected List<Transform> ClosestRespawnPoints(int amount)
    {
        List<Transform> closestPoints = new List<Transform>();
        List<MissionObj_EnemyRespawnPoint> allRespawnPoints =
            new List<MissionObj_EnemyRespawnPoint>(
                FindObjectsByType<MissionObj_EnemyRespawnPoint>(FindObjectsSortMode.None));

        while (closestPoints.Count < amount && allRespawnPoints.Count > 0)
        {
            float shortestDistance = float.MaxValue;
            MissionObj_EnemyRespawnPoint closestPoint = null;
            foreach (var point in allRespawnPoints)
            {
                float distance = Vector3.Distance(defensePoint, point.transform.position);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    closestPoint = point;
                }
            }

            if (closestPoint == null) continue;
            closestPoints.Add(closestPoint.transform);
            allRespawnPoints.Remove(closestPoint);
        }

        return  closestPoints;
    }
}
