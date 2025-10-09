using System;
using UnityEngine;

public class Enemy : NhoxBehaviour
{
    [Header("Idle Data")]
    public float idleTime = 2f;

    [Header("Move Data")] 
    [SerializeField] protected EnemyMove enemyMove;
    public EnemyMove EnemyMove => enemyMove;
    
    public EnemyStateMachine StateMachine { get; protected set; }
    
    protected override void Awake()
    {
        base.Awake();
        StateMachine = new EnemyStateMachine();
    }

    protected virtual void Update()
    {
        
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadEnemyMove();
    }

    protected void LoadEnemyMove()
    {
        if(enemyMove != null) return;
        enemyMove = GetComponentInChildren<EnemyMove>();
        Debug.Log(transform.name + " LoadEnemyMove", gameObject);
    }
}