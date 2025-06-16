using UnityEngine;
using UnityEngine.UI;

public class QuestOpenButton : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO openQuestPanelEvent;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            openQuestPanelEvent.Raise();
        });
    }
}