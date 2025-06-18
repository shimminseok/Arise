using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSkillUIPanelHandler : MonoBehaviour
{
    [SerializeField] private GameObject passiveSkillPanelUI;
    [SerializeField] private VoidEventChannelSO onPassiveSkillPanelEvent;

    private void OnEnable()
    {
        onPassiveSkillPanelEvent.RegisterListener(SetActivePassiveSkillPanel);
    }

    private void OnDisable()
    {
        onPassiveSkillPanelEvent.UnregisterListener(SetActivePassiveSkillPanel);
    }

    private void SetActivePassiveSkillPanel()
    {
        Debug.Log("패시브 패널 이벤트 확인!");
        passiveSkillPanelUI.SetActive(true);
    }
}
