using UnityEngine;
using UnityEngine.UI;

public class TurretDescriptionCloseButton : MonoBehaviour
{
    [SerializeField] private GameObject parentPanel;
    [SerializeField] private VoidEventChannelSO clickEvent;
    [SerializeField] private Button thisButton;

    private void Start()
    {
        thisButton.onClick.AddListener(() =>
        {
            clickEvent?.Raise();
            parentPanel?.SetActive(false);
        });
    }
}