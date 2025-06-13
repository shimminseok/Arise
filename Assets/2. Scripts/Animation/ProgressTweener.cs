using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ProgressTweener
{
    private float progressRatio;
    private MonoBehaviour runner;
    private Coroutine tweenCoroutine;
    private AnimationCurve runningCurve;

    public ProgressTweener(MonoBehaviour runner)
    {
        this.runner = runner;
    }
    
    public ProgressTweener SetCurve(AnimationCurve curve)
    {
        runningCurve = curve;
        return this;
    }

    public ProgressTweener Play(UnityAction<float> onUpdateToProgressRatio, float duration, UnityAction onComplete = null)
    {
        if (tweenCoroutine != null)
        {
            runner.StopCoroutine(tweenCoroutine);
        }

        tweenCoroutine = runner.StartCoroutine(TweenCoroutine(onUpdateToProgressRatio, duration, onComplete));
        return this;
    }

    private IEnumerator TweenCoroutine(UnityAction<float> onUpdateToProgressRatio , float duration, UnityAction onComplete)
    {
        float time = progressRatio * duration;
        while (time < duration)
        {
            yield return null;
            time += Time.deltaTime;
            progressRatio = runningCurve != null? runningCurve.Evaluate(time / duration) : time / duration;
            onUpdateToProgressRatio?.Invoke(progressRatio);
        }
        
        progressRatio = 1;
        onUpdateToProgressRatio?.Invoke(progressRatio); 
        runningCurve = null;
        progressRatio = 0;
        onComplete?.Invoke(); 
    }
}