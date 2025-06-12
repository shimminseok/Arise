using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UITurretPanelToggle : MonoBehaviour
{
    [SerializeField] private RectTransform turretPanel;
    [SerializeField] private Button toggleButton;
    [SerializeField] private TMP_Text buttonLabel;

    [SerializeField] private Vector2 shownPosition;
    [SerializeField] private Vector2 hiddenPosition;
    [SerializeField] private float slideSpeed = 5f;

    private bool isVisible = true;
    private Vector2 targetPosition;

    private void Start()
    {
        toggleButton.onClick.AddListener(TogglePanel);
        targetPosition = shownPosition;
        UpdateButtonText();
    }

    private void Update()
    {
        turretPanel.anchoredPosition = Vector2.Lerp(
            turretPanel.anchoredPosition,
            targetPosition,
            Time.deltaTime * slideSpeed
        );
    }

    private void TogglePanel()
    {
        isVisible = !isVisible;
        targetPosition = isVisible ? shownPosition : hiddenPosition;
        UpdateButtonText();
    }

    private void UpdateButtonText()
    {
        if (buttonLabel != null)
        {
            buttonLabel.text = isVisible ? ">" : "<";
        }
    }
}