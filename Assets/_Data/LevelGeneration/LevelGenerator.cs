using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : NhoxBehaviour
{
    [SerializeField] protected List<Transform> levelParts;
    [SerializeField] protected SnapPoint nextSnapPoint;

    [ContextMenu("Create Next Level Part")]
    protected void GenerateNextLevelPart()
    {
        Transform newPart = Instantiate(ChooseRandomPart());
        LevelPart levelPart = newPart.GetComponent<LevelPart>();

        levelPart.SnapAndAlignPartTo(nextSnapPoint);
        nextSnapPoint = levelPart.GetExitPoint();
    }

    protected Transform ChooseRandomPart()
    {
        int randomIndex = Random.Range(0, levelParts.Count);
        Transform chosenPart = levelParts[randomIndex];
        levelParts.RemoveAt(randomIndex);
        return chosenPart;
    }
}