using System;
using System.Collections;
using UnityEngine;

public class TimeManager : NhoxSingleton<TimeManager>
{
    [SerializeField] protected float resumeRate = 3f;
    [SerializeField] protected float pauseRate = 7f;

    protected float timeAdjustRate;
    protected float targetTimeScale = 1;

    protected void Update()
    {
        if (Mathf.Abs(Time.timeScale - targetTimeScale) > 0.05f)
        {
            float adjustRate = Time.unscaledDeltaTime * timeAdjustRate;
            Time.timeScale = Mathf.Lerp(Time.timeScale, targetTimeScale, adjustRate);
        }
        else
            Time.timeScale = targetTimeScale;
    }

    public void PauseTime()
    {
        timeAdjustRate = pauseRate;
        targetTimeScale = 0;
    }

    public void ResumeTime()
    {
        timeAdjustRate = resumeRate;
        targetTimeScale = 1;
    }

    public void SlowMotionIn(float seconds) => StartCoroutine(SlowTimeCo(seconds));

    protected IEnumerator SlowTimeCo(float seconds)
    {
        targetTimeScale = 0.5f;
        Time.timeScale = targetTimeScale;
        yield return new WaitForSecondsRealtime(seconds);
        ResumeTime();
    }
}
