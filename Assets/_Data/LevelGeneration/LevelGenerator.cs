using System;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class LevelGenerator : NhoxSingleton<LevelGenerator>
{
    [SerializeField] protected NavMeshSurface navMesh;
    
    [SerializeField] protected Transform lastLevelPart;
    [SerializeField] protected List<Transform> levelParts;
    protected List<Transform> currentLevelParts;
    protected List<Transform> generatedLevelParts = new();
    
    [SerializeField] protected SnapPoint nextSnapPoint;
    protected SnapPoint defaultSnapPoint;
    
    [SerializeField] protected float generationCooldown;
    protected float cooldownTimer;
    protected bool generationOver = true;
    
    protected List<Enemy> enemies = new();
    public List<Enemy> Enemies => enemies;
    protected Transform enemyHolder;
    
    protected override void Start()
    {
        base.Start();
        enemyHolder = GameObject.Find("EnemyHolder").transform;
        defaultSnapPoint = nextSnapPoint;
    }

    protected void Update()
    {
        if (generationOver) return;
        UpdateGenerationTimer();
    }

    protected void UpdateGenerationTimer()
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer < 0f)
            TryGenerateOrFinish();
    }

    protected void TryGenerateOrFinish()
    {
        if (currentLevelParts.Count > 0)
        {
            cooldownTimer = generationCooldown;
            GenerateNextLevelPart();
        }
        else if (!generationOver)
        {
            FinishGeneration();
        }
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadNavMesh();
        LoadFirstSnapPoint();
    }

    protected void LoadNavMesh()
    {
        if(navMesh != null) return;
        navMesh = transform.GetComponentInChildren<NavMeshSurface>();
        Debug.Log(transform.name + " LoadNavMesh", gameObject);
    }
    
    protected void LoadFirstSnapPoint()
    {
        if(nextSnapPoint != null) return;
        nextSnapPoint = transform.GetComponentInChildren<SnapPoint>();
        Debug.Log(transform.name + " LoadFirstSnapPoint", gameObject);
    }

    [ContextMenu("Restart Generation")]
    public void InitializeGeneration()
    {
        nextSnapPoint = defaultSnapPoint;
        generationOver = false;
        currentLevelParts = new List<Transform>(levelParts);
        DestroyOldLevelParts();
    }

    protected void DestroyOldLevelParts()
    {
        foreach (var enemy in enemies)
            Destroy(enemy.gameObject);
        
        foreach (Transform t in generatedLevelParts) 
            Destroy(t.gameObject);
        generatedLevelParts.Clear();
        enemies.Clear();
    }

    protected void GenerateNextLevelPart()
    {
        Transform newPart = null;
        newPart = Instantiate(generationOver ? lastLevelPart : ChooseRandomPart());
        
        generatedLevelParts.Add(newPart);
        if (!newPart.TryGetComponent(out LevelPart levelPart)) return;
        levelPart.SnapAndAlignPartTo(nextSnapPoint);
        levelPart.transform.parent = this.transform;
        
        if(levelPart.IntersectionDetected())
        {
            InitializeGeneration();
            return;
        }
        nextSnapPoint = levelPart.GetExitPoint();
        enemies.AddRange(levelPart.Enemies);
    }

    protected Transform ChooseRandomPart()
    {
        int randomIndex = Random.Range(0, currentLevelParts.Count);
        Transform chosenPart = currentLevelParts[randomIndex];
        currentLevelParts.RemoveAt(randomIndex);
        return chosenPart;
    }
    
    protected void FinishGeneration()
    {
        generationOver = true;
        GenerateNextLevelPart();
        navMesh.BuildNavMesh();
        foreach(Enemy enemy in enemies)
        {
            enemy.transform.parent = enemyHolder;
            enemy.gameObject.SetActive(true);
        }

        MissionManager.Instance.StartMission();
    }

    public Enemy GetRandomEnemy()
    {
        int randomIndex = Random.Range(0, enemies.Count);
        return enemies[randomIndex];
    }
}