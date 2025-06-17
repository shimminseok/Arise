using UnityEngine;

public class GameClearManager : MonoBehaviour
{
    [SerializeField] private GameObject clearPanel;
    [SerializeField] private VoidEventChannelSO onGameClearEvent;

    private void OnEnable()
    {
        if (onGameClearEvent != null)
            onGameClearEvent.RegisterListener(ShowClearPanel);
    }

    private void OnDisable()
    {
        if (onGameClearEvent != null)
            onGameClearEvent.UnregisterListener(ShowClearPanel);
    }

    private void ShowClearPanel()
    {
        if (clearPanel != null)
            clearPanel.SetActive(true);

        FindObjectOfType<TimeLimitQuestHelper>()?.TryClear();
    }
}