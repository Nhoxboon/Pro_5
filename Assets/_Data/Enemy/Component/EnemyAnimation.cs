
using UnityEngine;

public class EnemyAnimation : NhoxBehaviour
{
    [SerializeField] protected Animator anim;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadAnimator();
    }
    
    protected void LoadAnimator()
    {
        if (anim != null) return;
        anim = transform.parent.GetComponentInChildren<Animator>();
        Debug.Log(transform.name + " LoadAnimator", gameObject);
    }

    public void AnimationState(string state, bool isPlay = true)
    {
        anim.SetBool(state, isPlay);
    }
}
