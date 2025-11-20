using System.Collections.Generic;
using UnityEngine;

public class EnemyCoverSystem : NhoxBehaviour
{
    protected CoverPoint lastCover;
    
    [SerializeField] protected bool canUseCover = true;
    public bool CanUseCover => canUseCover;
    [SerializeField] protected List<Cover> allCovers = new();

    protected override void Awake()
    {
        base.Awake();
        allCovers = CollectNearByCovers();
    }

    public Transform AttemptToFindCover()
    {
        List<CoverPoint> collectedCoverPoints = new List<CoverPoint>();
        foreach (Cover cover in allCovers)
        {
            collectedCoverPoints.AddRange(cover.CoverPoints);
        }

        CoverPoint closestCoverPoint = null;
        float shortestDistance = float.MaxValue;
        
        foreach (CoverPoint coverPoint in collectedCoverPoints)
        {
            float currentDistance = Vector3.Distance(transform.position, coverPoint.transform.position);
            if (!(currentDistance < shortestDistance)) continue;
            closestCoverPoint = coverPoint;
            shortestDistance = currentDistance;
        }

        if (closestCoverPoint is not null)
            lastCover = closestCoverPoint;
        return lastCover?.transform;
    }

    protected List<Cover> CollectNearByCovers()
    {
        float coverRadiusCheck = 20f;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, coverRadiusCheck);
        List<Cover> collectedCovers = new List<Cover>();
        foreach (Collider col in hitColliders)
        {
            Cover cover = col.GetComponent<Cover>();
            if (cover != null && !collectedCovers.Contains(cover)) 
                collectedCovers.Add(cover);
        }
        return collectedCovers;
    }
}
