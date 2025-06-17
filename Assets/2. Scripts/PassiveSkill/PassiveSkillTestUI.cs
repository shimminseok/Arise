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

    public void PassiveSkillA()
    {
        PassiveSkillManager.Instance.ApplyPassive(_passiveSkills[0]);
        Debug.Log($"스킬 A {_passiveSkills[0].SkillName}");
    }

    public void PassiveSkillB()
    {
        PassiveSkillManager.Instance.ApplyPassive(_passiveSkills[1]);
        Debug.Log($"스킬 B {_passiveSkills[1].SkillName}");
    }

    public void PassiveSkillC()
    {
        PassiveSkillManager.Instance.ApplyPassive(_passiveSkills[2]);
        Debug.Log($"스킬 C {_passiveSkills[2].SkillName}");
    }
}
