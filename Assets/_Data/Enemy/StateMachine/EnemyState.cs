using UnityEngine;
using UnityEngine.AI;


public class EnemyState
{
    protected Enemy enemyBase;
    protected EnemyStateMachine stateMachine;
    protected string animBoolName;

    protected float startTimer;
    protected bool triggerCalled;

    public EnemyState(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName)
    {
        this.enemyBase = enemyBase;
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        enemyBase.EnemyAnimator.AnimationState(animBoolName);

        triggerCalled = false;
    }

    public virtual void Exit()
    {
        enemyBase.EnemyAnimator.AnimationState(animBoolName, false);
    }

    public virtual void LogicUpdate()
    {
        startTimer -= Time.deltaTime;
    }

    protected Vector3 GetNextPathPoint()
    {
        NavMeshAgent agent = enemyBase.EnemyMove.Agent;
        NavMeshPath path = agent.path;
        if (path.corners.Length < 2)
            return agent.destination;
        for (int i = 0; i < path.corners.Length; i++)
        {
            if (Vector3.Distance(agent.transform.position, path.corners[i]) < 1)
                return path.corners[i + 1];
        }

        return agent.destination;
    }

    public void AnimationTrigger() => triggerCalled = true;

    public virtual void AbilityTrigger()
    {
    }
}