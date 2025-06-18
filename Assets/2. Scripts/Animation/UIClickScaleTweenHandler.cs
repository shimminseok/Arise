using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIClickScaleTweenHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private RectTransform rectTransform;
    
    [SerializeField] private float duration;
    [SerializeField] private Vector2 targetScale;
    [SerializeField] private AnimationCurve easeOutCurve;
    
    
    private ProgressTweener clickTweener;

    private void Awake()
    {
        clickTweener = new(this);

        rectTransform = GetComponent<RectTransform>();
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        clickTweener.Play((ratio) => rectTransform.localScale = Vector3.Lerp(Vector3.one, targetScale, ratio) , duration).SetCurve(easeOutCurve);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        clickTweener.Play((ratio) => rectTransform.localScale = Vector3.Lerp(targetScale, Vector3.one, ratio) , 0.05f).SetCurve(easeOutCurve);
    }
}