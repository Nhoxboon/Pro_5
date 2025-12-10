
using UnityEngine;

public class EnemyAim : NhoxBehaviour
{
    [SerializeField] protected float slowAim = 4f;
    [SerializeField] protected float fastAim = 20f;

    public Transform aim;
    [SerializeField] protected LayerMask whatToIgnore;

    protected override void Start()
    {
        base.Start();
        aim.parent = null;
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadAimPoint();
    }
    
    protected void LoadAimPoint()
    {
        if (aim != null) return;
        aim = transform.Find("AimPoint");
        Debug.Log(transform.name + " LoadAimPoint", gameObject);
    }

    public void UpdateAimPosition()
    {
        float aimSpeed = AimOnPlayer() ? fastAim : slowAim;
        aim.position = Vector3.MoveTowards(aim.position, PlayerCtrl.Instance.transform.position + Vector3.up, Time.deltaTime * aimSpeed);
    }
    
    public bool AimOnPlayer()
    {
        float distanceAimToPlayer = Vector3.Distance(aim.position, PlayerCtrl.Instance.transform.position);
        return distanceAimToPlayer < 2;
    }

    public bool IsSeeingPlayer()
    {
        Vector3 myPosition = transform.parent.position + Vector3.up;
        Vector3 directionToPlayer = (PlayerCtrl.Instance.transform.position + Vector3.up) - myPosition;
        if (Physics.Raycast(myPosition, directionToPlayer, out RaycastHit hit, Mathf.Infinity, ~whatToIgnore))
        {
            if (hit.transform.root == PlayerCtrl.Instance.transform)
            {
                UpdateAimPosition();
                return true;
            }
        }
        return false;
    }
}
