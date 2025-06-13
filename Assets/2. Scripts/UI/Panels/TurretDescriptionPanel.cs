using System.Collections.Generic;
using UnityEngine;

public class TurretDescriptionPanel : MonoBehaviour
{
    [Header("설명 패널들")]
    [SerializeField] private List<GameObject> turretDescriptionPanels;

    // 설명창 열기
    public void ShowOnly(GameObject panelToShow)
    {
        foreach (var panel in turretDescriptionPanels)
        {
            panel.SetActive(false);
        }

        if (panelToShow != null)
            panelToShow.SetActive(true);
    }

    private void Start()
    {
        ShowOnly(null);
    }
}