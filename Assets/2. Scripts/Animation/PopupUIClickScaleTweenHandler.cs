using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PopupUIClickScaleTweenHandler : MonoBehaviour
{
    [SerializeField] private float duration = 0.3f;
    [SerializeField] private AnimationCurve bounceCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private Vector3 startScale = Vector3.zero;
    [SerializeField] private Vector3 endScale = Vector3.one * 1.1f;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        rectTransform.localScale = startScale;
        StartCoroutine(PlayBounce());
    }

    private IEnumerator PlayBounce()
    {
        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;
            float curveValue = bounceCurve.Evaluate(t);
            rectTransform.localScale = Vector3.LerpUnclamped(startScale, endScale, curveValue);
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        float shrinkTime = 0.1f;
        time = 0f;
        Vector3 fromScale = rectTransform.localScale;
        while (time < shrinkTime)
        {
            float t = time / shrinkTime;
            rectTransform.localScale = Vector3.LerpUnclamped(fromScale, Vector3.one, t);
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        rectTransform.localScale = Vector3.one;
    }
}