using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTestButton : MonoBehaviour
{
    [SerializeField] private GameObject _passivePanel;
    public void OnClickSkillButton(int skillID)
    {
        SkillManager.Instance.ExecuteSkill(skillID);
    }


    public void SkillA() => OnClickSkillButton(0);
    public void SkillB() => OnClickSkillButton(1);
    public void SkillC() => OnClickSkillButton(2);


    public void PassivePanelOnButton() => _passivePanel.SetActive(!_passivePanel.activeSelf);
}
