using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : NhoxBehaviour
{
    [SerializeField] protected float walkSpeed = 1.5f;
    public float WalkSpeed => walkSpeed;
    [SerializeField] protected float runSpeed = 4f;
    public float RunSpeed => runSpeed;

    [SerializeField] protected float advanceStoppingDistance = 5f;
    public float AdvanceStoppingDistance => advanceStoppingDistance;
    [SerializeField] protected float advanceDuration = 2.5f;
    public float AdvanceDuration => advanceDuration;

    [SerializeField] protected float turnSpeed = 5f;

    [SerializeField] protected Transform[] patrolPoints;
    protected Vector3[] patrolPointsPos;
    protected int currentPatrolIndex;
    [SerializeField] protected NavMeshAgent agent;
    public NavMeshAgent Agent => agent;

    protected bool manualMovement;
    public bool ManualMovement => manualMovement;
    protected bool manualRotation;
    public bool ManualRotation => manualRotation;

    protected override void Start()
    {
        base.Start();
        InitializePatrolPoints();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadPatrolsPoint();
        LoadNavMeshAgent();
    }

    protected void LoadPatrolsPoint()
    {
        if (patrolPoints is { Length: > 0 }) return;
        var patrolPointComponents = GetComponentsInChildren<EnemyPatrolPoint>();
        patrolPoints = new Transform[patrolPointComponents.Length];
        for (int i = 0; i < patrolPointComponents.Length; i++)
            patrolPoints[i] = patrolPointComponents[i].transform;
        Debug.Log(gameObject.name + " LoadPatrolsPoint", gameObject);
    }

    protected void LoadNavMeshAgent()
    {
        if (agent != null) return;
        agent = GetComponentInParent<NavMeshAgent>();
        Debug.Log(transform.name + " LoadNavMeshAgent", gameObject);
    }

    public Vector3 GetPatrolDestination()
    {
        Vector3 destination = patrolPointsPos[currentPatrolIndex];
        currentPatrolIndex++;
        if (currentPatrolIndex >= patrolPoints.Length)
            currentPatrolIndex = 0;
        return destination;
    }

    protected void InitializePatrolPoints()
    {
        patrolPointsPos = new Vector3[patrolPoints.Length];
        for (int i = 0; i < patrolPoints.Length; i++)
        {
            patrolPointsPos[i] = patrolPoints[i].position;
            patrolPoints[i].gameObject.SetActive(false);
        }
    }

    public void FaceTarget(Vector3 target)
    {
        Quaternion targetRotation = Quaternion.LookRotation(target - transform.parent.position);
        Vector3 currentEulerAngles = transform.parent.rotation.eulerAngles;
        float yRotation =
            Mathf.LerpAngle(currentEulerAngles.y, targetRotation.eulerAngles.y, turnSpeed * Time.deltaTime);

        transform.parent.rotation = Quaternion.Euler(currentEulerAngles.x, yRotation, currentEulerAngles.z);
    }

    public void SetMoveSpeed(float speed) => walkSpeed = speed;
    public void SetTurnSpeed(float speed) => turnSpeed = speed;
    public void SetAdvanceDuration(float duration) => advanceDuration = duration;

    public void ActivateManualMovement(bool manualMove) => manualMovement = manualMove;
    public void ActivateManualRotation(bool manualRotate) => manualRotation = manualRotate;
}