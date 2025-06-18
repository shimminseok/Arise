using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSkillUIPanelHandler : MonoBehaviour
{
    [SerializeField] private GameObject passiveSkillPanelUI;
    [SerializeField] private VoidEventChannelSO onPassiveSkillPanelEvent;
    [SerializeField] private BoolEventChannelSO isFastEvent;

    private bool isFast = false;


    private void OnEnable()
    {
        onPassiveSkillPanelEvent.RegisterListener(SetActivePassiveSkillPanel);
        isFastEvent.RegisterListener(IsFast);
    }

    private void OnDisable()
    {
        onPassiveSkillPanelEvent.UnregisterListener(SetActivePassiveSkillPanel);
        isFastEvent.UnregisterListener(IsFast);
    }

    private void SetActivePassiveSkillPanel()
    {
        Debug.Log("패시브 패널 이벤트 확인!");
        passiveSkillPanelUI.SetActive(true);
        passiveSkillPanelUI.GetComponent<PassiveSkillUI>().IsFast = isFast;
    }

    private void IsFast(bool value) => isFast = value;
}
