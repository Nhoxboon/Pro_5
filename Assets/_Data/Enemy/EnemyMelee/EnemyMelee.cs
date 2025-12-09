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

    [Header("Enemy Melee Settings")] 
    [SerializeField] protected EnemyBasicVisuals enemyVisuals;
    public EnemyBasicVisuals EnemyVisuals => enemyVisuals;
    [SerializeField] protected EMeleeAbility enemyAbility;
    public EMeleeAbility EnemyAbility => enemyAbility;
    
    public EnemyMeleeType meleeType;
    public EnemyMeleeWeaponType WeaponType { get; protected set; }

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
        LoadEnemyVisuals();
        LoadEnemyAbility();
        LoadShield();
    }
    
    protected void LoadEnemyVisuals()
    {
        if (enemyVisuals != null) return;
        enemyVisuals = GetComponentInChildren<EnemyBasicVisuals>();
        Debug.Log(transform.name + " LoadEnemyVisuals", gameObject);
    }

    protected void LoadEnemyAbility()
    {
        if(enemyAbility != null) return;
        enemyAbility = GetComponentInChildren<EMeleeAbility>();
        Debug.Log(transform.name + " LoadEnemyAbility", gameObject);
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
            WeaponType = EnemyMeleeWeaponType.Thrown;

        if (meleeType == EnemyMeleeType.Shield)
        {
            enemyAnimator.AnimationSetFloat("chaseIndex", 1);
            shieldTransform.gameObject.SetActive(true);
            WeaponType = EnemyMeleeWeaponType.OneHand;
        }

        if (meleeType == EnemyMeleeType.Dodge)
            WeaponType = EnemyMeleeWeaponType.Unarmed;
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

    protected override void EnemyDie()
    {
        base.EnemyDie();
        StateMachine.ChangeState(DeadState);
    }
}