using UnityEngine;
using UnityEngine.UI;

public class TurretDescriptionCloseButton : MonoBehaviour
{
    [SerializeField] private TurretDescriptionPanel turretPanel;
    [SerializeField] private VoidEventChannelSO clickEvent;
    [SerializeField] private Button thisButton;

    private void Start()
    {
        thisButton.onClick.AddListener(() =>
        {
            clickEvent?.Raise();
            turretPanel.ShowOnly(null);
        });
    }
}