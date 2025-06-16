using UnityEngine;

public class QuestUIController : MonoBehaviour
{
    [SerializeField] private GameObject questPanel;
    [SerializeField] private VoidEventChannelSO openQuestPanelEvent;
    [SerializeField] private VoidEventChannelSO closeQuestPanelEvent;

    private void Awake()
    {
        openQuestPanelEvent.RegisterListener(OpenPanel);
        closeQuestPanelEvent.RegisterListener(ClosePanel);
    }

    private void OnDestroy()
    {
        openQuestPanelEvent.UnregisterListener(OpenPanel);
        closeQuestPanelEvent.UnregisterListener(ClosePanel);
    }

    private void OpenPanel()
    {
        questPanel.SetActive(true);
    }

    private void ClosePanel()
    {
        questPanel.SetActive(false);
    }
}