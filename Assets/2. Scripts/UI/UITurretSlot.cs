using UnityEngine;
using UnityEngine.UI;

public class UITurretSlot : MonoBehaviour
{
    [SerializeField] private GameObject turretDescriptionPanel;
    [SerializeField] private UITurretPanel turretPanel;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            turretPanel.ShowOnly(turretDescriptionPanel);
        });
    }
}