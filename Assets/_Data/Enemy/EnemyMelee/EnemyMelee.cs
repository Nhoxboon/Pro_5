using UnityEngine;

public class EnemyMelee : Enemy
{
    public EMeleeIdleState IdleState { get; protected set; }
    public EMeleeMoveState MoveState { get; protected set; }
    public EMeleeRecoveryState RecoveryState { get; protected set; }
    public EMeleeChaseState ChaseState { get; protected set; }
    public EMeleeAttackState AttackState { get; protected set; }

    [SerializeField] protected Transform hiddenWeapon;
    [SerializeField] protected Transform pulledWeapon;

    protected override void Awake()
    {
        base.Awake();
        IdleState = new EMeleeIdleState(this, StateMachine, "idle");
        MoveState = new EMeleeMoveState(this, StateMachine, "move");
        RecoveryState = new EMeleeRecoveryState(this, StateMachine, "recovery");
        ChaseState = new EMeleeChaseState(this, StateMachine, "chase");
        AttackState = new EMeleeAttackState(this, StateMachine, "attack");
    }

    protected override void Start()
    {
        base.Start();
        StateMachine.Initialize(IdleState);
    }

    protected override void Update()
    {
        base.Update();
        StateMachine.currentState.LogicUpdate();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadHiddenWeapon();
        LoadPulledWeapon();
    }

    protected void LoadHiddenWeapon()
    {
        if (hiddenWeapon != null) return;
        var detects = GetComponentsInChildren<DetectTransform>();
        if (detects.Length > 0)
            hiddenWeapon = detects[0].transform;
        Debug.Log(transform.name + " LoadHiddenWeapon", gameObject);
    }

    protected void LoadPulledWeapon()
    {
        if (pulledWeapon != null) return;
        var detects = GetComponentsInChildren<DetectTransform>();
        if (detects.Length > 1)
            pulledWeapon = detects[1].transform;
        Debug.Log(transform.name + " LoadPulledWeapon", gameObject);
    }

    public void PullWeapon()
    {
        hiddenWeapon.gameObject.SetActive(false);
        pulledWeapon.gameObject.SetActive(true);
    }
}