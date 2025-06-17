using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private GameObject overPanel;
    [SerializeField] private VoidEventChannelSO onGameOverEvent;

    private void OnEnable()
    {
        if (onGameOverEvent != null)
            onGameOverEvent.RegisterListener(ShowOverPanel);
    }

    private void OnDisable()
    {
        if (onGameOverEvent != null)
            onGameOverEvent.UnregisterListener(ShowOverPanel);
    }

    private void ShowOverPanel()
    {
        if (overPanel != null)
            overPanel.SetActive(true);
    }
}