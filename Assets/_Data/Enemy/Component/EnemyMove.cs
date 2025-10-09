
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : NhoxBehaviour
{
    [SerializeField] protected float moveSpeed = 5f;
    public float MoveSpeed => moveSpeed;
    [SerializeField] protected Transform[] patrolPoints;
    protected int currentPatrolIndex;
    [SerializeField] protected NavMeshAgent agent;
    public NavMeshAgent Agent => agent;

    protected override void Start()
    {
        base.Start();
        InitializePatrolPoints();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadNavMeshAgent();
    }

    protected void LoadNavMeshAgent()
    {
        if (agent != null) return;
        agent = GetComponentInParent<NavMeshAgent>();
        Debug.Log(transform.name + " LoadNavMeshAgent", gameObject);
    }

    public Vector3 GetPatrolDestination()
    {
        Vector3 destination = patrolPoints[currentPatrolIndex].position;
        currentPatrolIndex++;
        if (currentPatrolIndex >= patrolPoints.Length) 
            currentPatrolIndex = 0;
        return destination;
    }
    
    protected void InitializePatrolPoints()
    {
        foreach (Transform point in patrolPoints)
        {
            point.parent = null;
        }
    }
}
