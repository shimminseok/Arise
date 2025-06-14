using UnityEngine;
using UnityEngine.UI;

public class QuestCloseButton : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO closeQuestPanelEvent;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            closeQuestPanelEvent.Raise();
        });
    }
}