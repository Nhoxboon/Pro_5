using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : NhoxBehaviour
{
    [SerializeField] protected Transform lastLevelPart;
    [SerializeField] protected List<Transform> levelParts;
    protected List<Transform> currentLevelParts;
    protected List<Transform> generatedLevelParts = new();
    
    [SerializeField] protected SnapPoint nextSnapPoint;
    protected SnapPoint defaultSnapPoint;
    
    [SerializeField] protected float generationCooldown;
    protected float cooldownTimer;
    protected bool generationOver;
    
    protected override void Start()
    {
        base.Start();
        defaultSnapPoint = nextSnapPoint;
        InitializeGeneration();
    }

    protected void Update()
    {
        if(generationOver) return;
        
        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer < 0)
        {
            if(currentLevelParts.Count > 0)
            {
                cooldownTimer = generationCooldown;
                GenerateNextLevelPart();
            }
            else if (!generationOver)
            {
                FinishGeneration();
            }
        }
    }

    [ContextMenu("Restart Generation")]
    protected void InitializeGeneration()
    {
        nextSnapPoint = defaultSnapPoint;
        generationOver = false;
        currentLevelParts = new List<Transform>(levelParts);
        DestroyOldLevelParts();
    }

    protected void DestroyOldLevelParts()
    {
        foreach (Transform t in generatedLevelParts) 
            Destroy(t.gameObject);
        generatedLevelParts.Clear();
    }

    protected void FinishGeneration()
    {
        generationOver = true;
        GenerateNextLevelPart();
    }

    [ContextMenu("Create Next Level Part")]
    protected void GenerateNextLevelPart()
    {
        Transform newPart = null;
        newPart = Instantiate(generationOver ? lastLevelPart : ChooseRandomPart());
        
        generatedLevelParts.Add(newPart);
        if (!newPart.TryGetComponent(out LevelPart levelPart)) return;
        levelPart.SnapAndAlignPartTo(nextSnapPoint);
        
        if(levelPart.IntersectionDetected())
        {
            InitializeGeneration();
            return;
        }
        nextSnapPoint = levelPart.GetExitPoint();
    }

    protected Transform ChooseRandomPart()
    {
        int randomIndex = Random.Range(0, currentLevelParts.Count);
        Transform chosenPart = currentLevelParts[randomIndex];
        currentLevelParts.RemoveAt(randomIndex);
        return chosenPart;
    }
}