using System.Collections.Generic;
using UnityEngine;

public class EnemyCoverSystem : NhoxBehaviour
{
    public CoverPerk coverPerk;
    public UnstoppablePerk unstoppablePerk;

    public CoverPoint CurrentCover { get; protected set; }
    public CoverPoint LastCover { get; protected set; }

    [SerializeField] protected float minCoverTime = 4f;
    public float MinCoverTime => minCoverTime;
    [SerializeField] protected float safeDistance = 2f;
    public float SafeDistance => safeDistance;

    public bool IsUnstoppable() => unstoppablePerk == UnstoppablePerk.Unstoppable;

    public bool CanGetCover()
    {
        if (coverPerk == CoverPerk.Unavailable)
            return false;

        Transform coverTransform = AttemptToFindCover();

        if (coverTransform is not null && coverTransform.TryGetComponent(out CoverPoint point))
            CurrentCover = point;

        if (LastCover != CurrentCover && CurrentCover is not null)
            return true;

        // Debug.LogWarning("No cover found!");
        return false;
    }

    protected Transform AttemptToFindCover()
    {
        List<CoverPoint> collectedCoverPoints = new List<CoverPoint>();
        foreach (Cover cover in CollectNearByCovers())
            collectedCoverPoints.AddRange(cover.GetValidCoverPoints(transform.parent));

        CoverPoint closestCoverPoint = null;
        float shortestDistance = float.MaxValue;

        foreach (CoverPoint coverPoint in collectedCoverPoints)
        {
            float currentDistance = Vector3.Distance(transform.parent.position, coverPoint.transform.position);
            if (!(currentDistance < shortestDistance)) continue;
            closestCoverPoint = coverPoint;
            shortestDistance = currentDistance;
        }

        if (closestCoverPoint is not null)
        {
            LastCover?.SetOccupied(false);
            LastCover = CurrentCover;
            CurrentCover = closestCoverPoint;
            CurrentCover.SetOccupied(true);

            return CurrentCover.transform;
        }

        return null;
    }

    protected List<Cover> CollectNearByCovers()
    {
        float coverRadiusCheck = 20f;
        Collider[] hitColliders = Physics.OverlapSphere(transform.parent.position, coverRadiusCheck);
        List<Cover> collectedCovers = new List<Cover>();
        foreach (Collider col in hitColliders)
        {
            if (col.TryGetComponent(out Cover cover) && !collectedCovers.Contains(cover))
                collectedCovers.Add(cover);
        }

        return collectedCovers;
    }
}