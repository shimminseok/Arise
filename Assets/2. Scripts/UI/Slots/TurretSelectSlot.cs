using UnityEngine;
using UnityEngine.UI;

public class TurretSelectSlot : MonoBehaviour
{
    [SerializeField] private GameObject turretDescriptionPanel;
    [SerializeField] private TurretDescriptionPanel turretPanel;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            turretPanel.ShowOnly(turretDescriptionPanel);
        });
    }
}