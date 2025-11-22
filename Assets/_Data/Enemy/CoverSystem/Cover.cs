
using System.Collections.Generic;
using UnityEngine;

public class Cover : NhoxBehaviour
{
    [Header("Cover Points")] 
    [SerializeField] protected GameObject coverPointPrefabs;
    [SerializeField] protected List<CoverPoint> coverPoints = new();
    
    [SerializeField] protected float xOffset = 1f;
    [SerializeField] protected float yOffset = 0.2f;
    [SerializeField] protected float zOffset = 1f;

    protected override void Start()
    {
        base.Start();
        GenerateCoverPoints();
    }

    protected void GenerateCoverPoints()
    {
        Vector3[] localCoverPoints = {
            new Vector3(0, yOffset, zOffset), // Front
            new Vector3(0, yOffset, -zOffset), // Back
            new Vector3(xOffset, yOffset, 0), // Right
            new Vector3(-xOffset, yOffset, 0), // Left
        };

        foreach (Vector3 localPoint in localCoverPoints)
        {
            Vector3 worldPoint = transform.TransformPoint(localPoint);
            CoverPoint coverPoint =
                Instantiate(coverPointPrefabs, worldPoint, Quaternion.identity, transform)
                    .GetComponent<CoverPoint>(); //Instantiate only 1 time when started, then no need to use object pooling
            
            coverPoints.Add(coverPoint);
        }
    }

    public List<CoverPoint> GetValidCoverPoints(Transform enemy)
    {
        List<CoverPoint> validCoverPoints = new List<CoverPoint>();
        foreach (CoverPoint coverPoint in coverPoints)
        {
            if (!IsValidCoverPoint(coverPoint, enemy)) continue;
            validCoverPoints.Add(coverPoint);
        }
        return validCoverPoints;
    }

    protected bool IsValidCoverPoint(CoverPoint coverPoint, Transform enemy)
    {
        if (coverPoint.occupied)
            return false;
        
        if(!IsFurtherestFromPlayer(coverPoint))
            return false;        
        
        if(IsCoverCloseToPlayer(coverPoint))
            return false;

        if (IsCoverBehindPlayer(coverPoint, enemy))
            return false;

        if (IsCoverCloseToLastCover(coverPoint, enemy))
            return false;

        return true;
    }

    protected bool IsCoverBehindPlayer(CoverPoint coverPoint, Transform enemy)
    {
        float distanceToPlayer = Vector3.Distance(coverPoint.transform.position, PlayerCtrl.Instance.transform.position);
        float distanceToEnemy = Vector3.Distance(coverPoint.transform.position, enemy.position);
        return distanceToPlayer < distanceToEnemy;
    }

    protected bool IsCoverCloseToPlayer(CoverPoint coverPoint) =>
        Vector3.Distance(coverPoint.transform.position, PlayerCtrl.Instance.transform.position) < 2f;

    protected bool IsCoverCloseToLastCover(CoverPoint coverPoint, Transform enemy)
    {
        CoverPoint lastCover = null;
        if (enemy.TryGetComponent(out EnemyRange enemyRange))
            lastCover = enemyRange.EnemyCover.CurrentCover;

        return lastCover is not null && Vector3.Distance(coverPoint.transform.position, lastCover.transform.position) < 3f;
    }

    protected bool IsFurtherestFromPlayer(CoverPoint coverPoint)
    {
        CoverPoint furtherestPoint = null;
        float furtherestDis = 0f;

        foreach (CoverPoint point in coverPoints)
        {
            float distance = Vector3.Distance(point.transform.position, PlayerCtrl.Instance.transform.position);
            if (!(distance > furtherestDis)) continue;
            furtherestDis = distance;
            furtherestPoint = point;
        }
        return furtherestPoint == coverPoint;
    }
}
