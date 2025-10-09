using UnityEngine;

public class EnemyState
{
    protected Enemy enemyBase;
    protected EnemyStateMachine stateMachine;
    protected string animBoolName;
    
    protected float startTimer;

    public EnemyState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName)
    {
        this.enemyBase = enemyBase;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }
    
    public virtual void Enter()
    {
    }
    
    public virtual void Exit()
    {
    }
    
    public virtual void LogicUpdate()
    {
        startTimer -= Time.deltaTime;
    }
}