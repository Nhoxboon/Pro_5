using UnityEngine;

public class EnemyRange : Enemy
{
    #region State

    public ERangeIdleState IdleState { get; protected set; }
    public ERangeMoveState MoveState { get; protected set; }
    public ERangeBattleState BattleState { get; protected set; }

    #endregion

    public EnemyRangeWeaponType weaponType;

    protected override void Awake()
    {
        base.Awake();
        IdleState = new ERangeIdleState(this, StateMachine, "idle");
        MoveState = new ERangeMoveState(this, StateMachine, "move");
        BattleState = new ERangeBattleState(this, StateMachine, "battle");
    }

    protected override void Start()
    {
        base.Start();
        StateMachine.Initialize(IdleState);
        enemyVisuals.SetupLook();
    }

    protected override void Update()
    {
        base.Update();
        StateMachine.currentState.LogicUpdate();
    }

    public override void EnterBattleMode()
    {
        if (InBattleMode) return;
        base.EnterBattleMode();
        StateMachine.ChangeState(BattleState);
    }
}