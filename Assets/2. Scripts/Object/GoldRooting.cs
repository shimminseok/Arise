using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldRooting : MonoBehaviour, IPoolObject
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
    [Header("ObjectPool")]
    [SerializeField] private string poolID;

    [SerializeField] private int poolSize;
    private ProgressTweener chaseTweener;

    public GameObject GameObject => gameObject;
    public string     PoolID     => poolID;
    public int        PoolSize   => poolSize;

    private Transform target;
    
    private void Awake()
    {
        chaseTweener = new(this);
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

    public void Initialized(int amount)
    {
        goldAmount = amount;
        OnSpawnFromPool();
        StartPopLoop();
    }
    public void OnSpawnFromPool()
    {
        StartCoroutine(DropGold());
    }

    public void OnReturnToPool()
    {
    }

    private IEnumerator DropGold()
    {
        yield return new WaitForSeconds(3f);
        ChasedTarget(SkillManager.Instance.Owner.transform);
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
                        GoldManager.Instance.AddGold(goldAmount);
                        ObjectPoolManager.Instance.ReturnObject(gameObject);
                    }).SetCurve(easeInCurve);
                
            }).SetCurve(easeOutCurve);
    }


}