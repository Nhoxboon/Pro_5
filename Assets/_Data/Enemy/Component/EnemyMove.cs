
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : NhoxBehaviour
{
    [SerializeField] protected float moveSpeed = 1.5f;
    public float MoveSpeed => moveSpeed;
    [SerializeField] protected float chaseSpeed = 3.5f;
    public float ChaseSpeed => chaseSpeed;
    [SerializeField] protected float turnSpeed = 5f;
    
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
            point.parent = null;
    }
    
    public Quaternion FaceTarget(Vector3 target)
    {
        Quaternion targetRotation = Quaternion.LookRotation(target - transform.parent.position);
        Vector3 currentEulerAngles = transform.parent.rotation.eulerAngles;
        float yRotation =
            Mathf.LerpAngle(currentEulerAngles.y, targetRotation.eulerAngles.y, turnSpeed * Time.deltaTime);
        
        return Quaternion.Euler(currentEulerAngles.x, yRotation, currentEulerAngles.z);
    }
}
