using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSkillTestUI : MonoBehaviour
{
    private List<PassiveSkillSO> _passiveSkills;

    private void OnEnable()
    {
        _passiveSkills = PassiveSkillManager.Instance.GetThreeRandomChoices();
    }

    public void PassiveSkillA() => PassiveSkillManager.Instance.ApplyPassive(_passiveSkills[0]);
    public void PassiveSkillB() => PassiveSkillManager.Instance.ApplyPassive(_passiveSkills[1]);
    public void PassiveSkillC() => PassiveSkillManager.Instance.ApplyPassive(_passiveSkills[2]);
}
