using UnityEngine;

public class EnemyMelee : Enemy
{
    #region State

    public EMeleeIdleState IdleState { get; protected set; }
    public EMeleeMoveState MoveState { get; protected set; }
    public EMeleeRecoveryState RecoveryState { get; protected set; }
    public EMeleeChaseState ChaseState { get; protected set; }
    public EMeleeAttackState AttackState { get; protected set; }
    public EMeleeAbilityState AbilityState { get; protected set; }
    public EMeleeDeadState DeadState { get; protected set; }

    #endregion

    [Header("Enemy Settings")] public EnemyMeleeType meleeType;
    public EnemyMeleeWeaponType weaponType;

    [SerializeField] protected Transform shieldTransform;
    public float dodgeCooldown = 2f;
    protected float lastDodgeTime = -10f;

    protected override void Awake()
    {
        base.Awake();
        IdleState = new EMeleeIdleState(this, StateMachine, "idle");
        MoveState = new EMeleeMoveState(this, StateMachine, "move");
        RecoveryState = new EMeleeRecoveryState(this, StateMachine, "recovery");
        ChaseState = new EMeleeChaseState(this, StateMachine, "chase");
        AttackState = new EMeleeAttackState(this, StateMachine, "attack");
        AbilityState = new EMeleeAbilityState(this, StateMachine, "axeThrow");
        DeadState = new EMeleeDeadState(this, StateMachine, "idle"); //Idle anim is just a placeholder, we use rag doll

        core.Stats.Health.OnCurrentValueZero += EnemyDead;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        core.Stats.Health.OnCurrentValueZero -= EnemyDead;
    }

    protected override void Start()
    {
        base.Start();
        StateMachine.Initialize(IdleState);

        InitializePerk();
        enemyVisuals.SetupLook();
        enemyAttack.UpdateAttackData();
    }

    protected override void Update()
    {
        base.Update();
        StateMachine.currentState.LogicUpdate();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadShield();
    }

    protected void LoadShield()
    {
        if (shieldTransform != null) return;
        shieldTransform = GetComponentInChildren<EnemyShield>(true).transform;
        Debug.Log(transform.name + " LoadShield", gameObject);
    }

    public override void EnterBattleMode()
    {
        if (InBattleMode) return;
        base.EnterBattleMode();
        StateMachine.ChangeState(RecoveryState);
    }

    public override void AbilityTrigger()
    {
        base.AbilityTrigger();
        enemyMove.SetMoveSpeed(enemyMove.WalkSpeed * 0.6f);
        enemyVisuals.EnableWeaponModel(false);
    }

    protected override void InitializePerk()
    {
        if (meleeType == EnemyMeleeType.AxeThrow)
            weaponType = EnemyMeleeWeaponType.Thrown;

        if (meleeType != EnemyMeleeType.Shield) return;
        enemyAnimator.AnimationSetFloat("chaseIndex", 1);
        shieldTransform.gameObject.SetActive(true);
        weaponType = EnemyMeleeWeaponType.OneHand;

        if (meleeType == EnemyMeleeType.Dodge)
            weaponType = EnemyMeleeWeaponType.Unarmed;
    }


    public void ActivateDodge()
    {
        if (meleeType != EnemyMeleeType.Dodge) return;
        if (StateMachine.currentState != ChaseState) return;
        if (Vector3.Distance(transform.position, PlayerCtrl.Instance.transform.position) < 2f) return;

        if (!(Time.time >
              dodgeCooldown + lastDodgeTime + enemyAnimator.GetAnimationClipDuration("enemy1_DodgeRoll"))) return;
        lastDodgeTime = Time.time;
        enemyAnimator.AnimationTrigger("dodge");
    }

    protected override void EnemyDead()
    {
        base.EnemyDead();
        StateMachine.ChangeState(DeadState);
    }
}