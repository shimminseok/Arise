using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurretSelectSlot : MonoBehaviour
{
    [Header("터렛 데이터")]
    [SerializeField] private TowerSO towerData;

    [Header("UI")]
    [SerializeField] private TMP_Text goldText; // TurretGold_Text
    [SerializeField] private GameObject turretDescriptionPanel;
    [SerializeField] private TurretDescriptionPanel turretPanel;
    [SerializeField] private Button installButton;

    private Button slotButton;

    private void Awake()
    {
        slotButton = GetComponent<Button>();
    }

    private void Start()
    {
        if (towerData != null && goldText != null)
        {
            goldText.text = towerData.BuildCost.ToString();
        }

        slotButton.onClick.AddListener(() =>
        {
            turretPanel.ShowOnly(turretDescriptionPanel);
        });

        installButton.onClick.AddListener(() =>
        {
            BuildingPlacer.Instance.TryBuildingTower(towerData);
            turretPanel.ShowOnly(null);
        });
    }
}