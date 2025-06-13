using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldRooting : MonoBehaviour
{
    [SerializeField] private int goldAmount;

    [Space(10f)]
    [SerializeField] private float popDist;
    [SerializeField] private float popTime;
    
    [Space(10f)]
    [SerializeField] private float yoyoDist;
    [SerializeField] private float yoyoTime;
    
    [Space(5f)]
    [SerializeField] private float chaseTime;
    
    [Space(10f)]
    [SerializeField] private AnimationCurve easeInCurve;
    [SerializeField] private AnimationCurve easeOutCurve;
    
    [Space(10f)]
    [Header("Events")]
    [SerializeField] private TransformEventSO rooting;
    [SerializeField] private IntegerEventChannelSO rootedGold;
    
    private ProgressTweener chaseTweener;
    
    private void Awake()
    {
        chaseTweener = new(this);
    }

    private void OnEnable()
    {
        rooting.RegisterListener(ChasedTarget);
        StartPopLoop();
    }
    
    private void StartPopLoop()
    {
        Vector3 popStartPos = transform.position;
        Vector3 popEndPos = popStartPos + Vector3.up * popDist;

        chaseTweener.Play(
            (ratio) => transform.position = Vector3.Lerp(popStartPos, popEndPos, ratio),
            popTime,
            () =>
            {
                chaseTweener.Play(
                    (ratio) => transform.position = Vector3.Lerp(popEndPos, popStartPos, ratio),
                    popTime,
                    StartPopLoop
                ).SetCurve(easeInCurve);
            }).SetCurve(easeOutCurve);
    }

    private void OnDisable()
    {
        rooting.UnregisterListener(ChasedTarget);
    }

    void ChasedTarget(Transform target)
    {
        Vector3 yoyoStartPos = transform.position;
        Vector3 yoyoDir = (yoyoStartPos - target.position).normalized;
        Vector3 yoyoEndPos = yoyoStartPos + yoyoDir * yoyoDist;
        

        chaseTweener.Play(
            (ratio) => transform.position = Vector3.Lerp(yoyoStartPos, yoyoEndPos, ratio),
            yoyoTime,
            () =>
            {
                chaseTweener.Play(
                    (ratio) => transform.position = Vector3.Lerp(yoyoEndPos, target.position, ratio),
                    chaseTime,
                    () =>
                    {
                        rootedGold.Raise(goldAmount);
                        gameObject.SetActive(false);
                    }).SetCurve(easeInCurve);
                
            }).SetCurve(easeOutCurve);
    }
}