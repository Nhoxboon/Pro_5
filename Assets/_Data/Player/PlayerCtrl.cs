
using UnityEngine;

public class PlayerCtrl : NhoxBehaviour
{
    private static PlayerCtrl instance;
    public static PlayerCtrl Instance => instance;
    
    [SerializeField] protected PlayerInput playerInput;
    public PlayerInput PlayerInput => playerInput;
    
    [SerializeField] protected PlayerMovement playerMovement;
    public PlayerMovement PlayerMovement => playerMovement;
    
    [SerializeField] protected PlayerAnimator playerAnim;
    public PlayerAnimator PlayerAnim => playerAnim;

    protected override void Awake()
    {
        base.Awake();
        if (instance != null)
        {
            Debug.LogError("Only one instance of PlayerCtrl allow to exist");
            return;
        }
        instance = this;
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadPlayerInput();
        LoadPlayerMovement();
        LoadPlayerAnim();
    }
    
    protected void LoadPlayerInput()
    {
        if(playerInput != null) return;
        playerInput = GetComponent<PlayerInput>();
        Debug.Log(transform.name + " LoadPlayerInput", gameObject);
    }
    
    protected void LoadPlayerMovement()
    {
        if(playerMovement != null) return;
        playerMovement = GetComponentInChildren<PlayerMovement>();
        Debug.Log(transform.name + " LoadPlayerMovement", gameObject);
    }
    
    protected void LoadPlayerAnim()
    {
        if(playerAnim != null) return;
        playerAnim = GetComponentInChildren<PlayerAnimator>();
        Debug.Log(transform.name + " LoadPlayerAnim", gameObject);
    }
}
