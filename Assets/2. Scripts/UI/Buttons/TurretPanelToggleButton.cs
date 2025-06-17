using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurretPanelToggleButton : MonoBehaviour
{
    [SerializeField] private RectTransform turretPanel;
    [SerializeField] private Button toggleButton;
    [SerializeField] private TMP_Text buttonLabel;

    [SerializeField] private Vector2 shownPosition;
    [SerializeField] private Vector2 hiddenPosition;
    [SerializeField] private float slideSpeed = 5f;

    private bool isVisible = false;
    private Vector2 targetPosition;

    private void Start()
    {
        toggleButton.onClick.AddListener(TogglePanel);
        targetPosition = hiddenPosition;
        turretPanel.anchoredPosition = hiddenPosition;
        UpdateButtonText();
    }


    private void Update()
    {
        turretPanel.anchoredPosition = Vector2.Lerp(
            turretPanel.anchoredPosition,
            targetPosition,
            Time.unscaledDeltaTime * slideSpeed
        );
    }

    private void TogglePanel()
    {
        isVisible = !isVisible;
        targetPosition = isVisible ? shownPosition : hiddenPosition;
        UpdateButtonText();
        BuildingPlacer.Instance.ChangeBuildMode(isVisible);
    }

    private void UpdateButtonText()
    {
        if (buttonLabel != null)
        {
            buttonLabel.text = isVisible ? ">" : "<";
        }
    }
}