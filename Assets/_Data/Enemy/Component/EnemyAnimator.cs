using UnityEngine;

public class EnemyAnimator : NhoxBehaviour
{
    [SerializeField] protected Animator anim;
    public Animator Animator => anim;

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

    public void SetAnimator(bool value) => anim.enabled = value;

    public void AnimationState(string state, bool isPlay = true)
    {
        anim.SetBool(state, isPlay);
    }

    public void AnimationTrigger(string state)
    {
        anim.SetTrigger(state);
    }

    public void AnimationSetFloat(string state, float value)
    {
        anim.SetFloat(state, value);
    }
    
    public void SwitchAnimationLayer(int layerIndex)
    {
        for (int i = 1; i < anim.layerCount; i++)
            anim.SetLayerWeight(i, 0);
        
        anim.SetLayerWeight(layerIndex, 1);
    }

    public float GetAnimationClipDuration(string clipName)
    {
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        foreach (var clip in clips)
        {
            if(clip.name == clipName)
                return clip.length;
        }
        return 0;
    }
}