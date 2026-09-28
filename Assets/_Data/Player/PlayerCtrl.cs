using System;
using UnityEngine;

public class PlayerCtrl : NhoxSingleton<PlayerCtrl>
{
    [SerializeField] protected Core core;
    public Core Core => core;

    [SerializeField] protected PlayerInput playerInput;
    public PlayerInput PlayerInput => playerInput;

    [SerializeField] protected PlayerMovement playerMovement;
    public PlayerMovement PlayerMovement => playerMovement;

    [SerializeField] protected PlayerAimCtrl playerAim;
    public PlayerAimCtrl PlayerAim => playerAim;

    [SerializeField] protected PlayerAttack playerAttack;
    public PlayerAttack PlayerAttack => playerAttack;

    [SerializeField] protected PlayerAnimator playerAnim;
    public PlayerAnimator PlayerAnim => playerAnim;

    [SerializeField] protected PlayerWeaponVisual playerWeaponVisual;
    public PlayerWeaponVisual PlayerWeaponVisual => playerWeaponVisual;

    [SerializeField] protected PlayerInventory playerInventory;
    public PlayerInventory PlayerInventory => playerInventory;

    [SerializeField] protected PlayerInteraction playerInteraction;
    public PlayerInteraction PlayerInteraction => playerInteraction;
    
    public bool IsDead => core.Stats.Health.CurrentValue == 0;

    protected override void Awake()
    {
        base.Awake();
        core.Stats.Health.OnCurrentValueZero += PlayerDie;
    }

    protected void OnDestroy() => core.Stats.Health.OnCurrentValueZero -= PlayerDie;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadCore();
        LoadPlayerInput();
        LoadPlayerMovement();
        LoadPlayerAimCtrl();
        LoadPlayerAtk();
        LoadPlayerAnim();
        LoadPlayerWeaponVisual();
        LoadPlayerInventory();
        LoadPlayerInteraction();
    }
    
    protected void LoadCore()
    {
        if (core != null) return;
        core = GetComponentInChildren<Core>();
        Debug.Log(transform.name + " LoadCore", gameObject);
    }

    protected void LoadPlayerInput()
    {
        if (playerInput != null) return;
        playerInput = GetComponent<PlayerInput>();
        Debug.Log(transform.name + " LoadPlayerInput", gameObject);
    }

    protected void LoadPlayerMovement()
    {
        if (playerMovement != null) return;
        playerMovement = GetComponentInChildren<PlayerMovement>();
        Debug.Log(transform.name + " LoadPlayerMovement", gameObject);
    }

    protected void LoadPlayerAimCtrl()
    {
        if (playerAim != null) return;
        playerAim = GetComponentInChildren<PlayerAimCtrl>();
        Debug.Log(transform.name + " LoadPlayerAimCtrl", gameObject);
    }

    protected void LoadPlayerAtk()
    {
        if (playerAttack != null) return;
        playerAttack = GetComponentInChildren<PlayerAttack>();
        Debug.Log(transform.name + " LoadPlayerAtk", gameObject);
    }

    protected void LoadPlayerAnim()
    {
        if (playerAnim != null) return;
        playerAnim = GetComponentInChildren<PlayerAnimator>();
        Debug.Log(transform.name + " LoadPlayerAnim", gameObject);
    }

    protected void LoadPlayerWeaponVisual()
    {
        if (playerWeaponVisual != null) return;
        playerWeaponVisual = GetComponentInChildren<PlayerWeaponVisual>();
        Debug.Log(transform.name + " LoadPlayerWeaponVisual", gameObject);
    }

    protected void LoadPlayerInventory()
    {
        if (playerInventory != null) return;
        playerInventory = GetComponentInChildren<PlayerInventory>();
        Debug.Log(transform.name + " LoadPlayerInventory", gameObject);
    }

    protected void LoadPlayerInteraction()
    {
        if (playerInteraction != null) return;
        playerInteraction = GetComponentInChildren<PlayerInteraction>();
        Debug.Log(transform.name + " LoadPlayerInteraction", gameObject);
    }
    
    protected void PlayerDie()
    {
        playerAnim.Animator.enabled = false;
        core.Ragdoll.RagdollActive(true);
        GameManager.Instance.GameOver();
    }
}