using UnityEngine;

public class SettingsPanelController : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO openSettingsEvent;
    [SerializeField] private VoidEventChannelSO closeSettingsEvent;

    [SerializeField] private GameObject settingsCanvas;

    private void OnEnable()
    {
        openSettingsEvent.RegisterListener(OpenSettings);
        closeSettingsEvent.RegisterListener(CloseSettings);
    }

    private void OnDisable()
    {
        openSettingsEvent.UnregisterListener(OpenSettings);
        closeSettingsEvent.UnregisterListener(CloseSettings);
    }

    private void OpenSettings()
    {
        settingsCanvas.SetActive(true);
    }

    private void CloseSettings()
    {
        settingsCanvas.SetActive(false);
    }
}