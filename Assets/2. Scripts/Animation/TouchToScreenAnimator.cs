using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchToScreenAnimator : MonoBehaviour
{
    [SerializeField] private float scaleSpeed = 1.0f;
    [SerializeField] private float maxScale = 1.2f;
    [SerializeField] private float minScale = 0.8f;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        StartCoroutine(AnimateScale());
    }

    private IEnumerator AnimateScale()
    {
        Vector3 startScale = Vector3.one * minScale;
        Vector3 endScale = Vector3.one * maxScale;

        while (true)
        {
            yield return StartCoroutine(ScaleOverTime(startScale, endScale));
            yield return StartCoroutine(ScaleOverTime(endScale, startScale));
        }
    }

    private IEnumerator ScaleOverTime(Vector3 from, Vector3 to)
    {
        float elapsed = 0f;
        while (elapsed < 1f)
        {
            rectTransform.localScale = Vector3.Lerp(from, to, elapsed);
            elapsed += Time.unscaledDeltaTime * scaleSpeed;
            yield return null;
        }

        rectTransform.localScale = to;
    }
}