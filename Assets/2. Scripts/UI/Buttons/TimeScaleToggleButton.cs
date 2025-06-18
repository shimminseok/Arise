using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeScaleToggleButton : MonoBehaviour
{
    [SerializeField] private Button speedButton;
    [SerializeField] private TMP_Text speedText;
    [SerializeField] private BoolEventChannelSO isFastEvent;

    private bool isFast = false;

    private void Start()
    {
        speedButton.onClick.AddListener(ToggleTimeScale);
        UpdateText();
    }

    private void ToggleTimeScale()
    {
        isFast = !isFast;
        Time.timeScale = isFast ? 2f : 1f;
        isFastEvent.Raise(isFast);
        UpdateText();
    }

    private void UpdateText()
    {
        if (speedText != null)
            speedText.text = isFast ? "x2" : "x1";
    }
}