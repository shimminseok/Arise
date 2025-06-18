using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PassiveSkillUI : MonoBehaviour
{
    [Header("Passive Skill A")]
    [SerializeField] private TextMeshProUGUI _passiveSkillNameA;
    [SerializeField] private Image _passiveSkillIconA;
    [SerializeField] private TextMeshProUGUI _passiveSkillDescriptionA;

    [Header("Passive Skill B")]
    [SerializeField] private TextMeshProUGUI _passiveSkillNameB;
    [SerializeField] private Image _passiveSkillIconB;
    [SerializeField] private TextMeshProUGUI _passiveSkillDescriptionB;

    [Header("Passive Skill C")]
    [SerializeField] private TextMeshProUGUI _passiveSkillNameC;
    [SerializeField] private Image _passiveSkillIconC;
    [SerializeField] private TextMeshProUGUI _passiveSkillDescriptionC;
    
    private List<PassiveSkillSO> _passiveSkills;

    private void OnEnable()
    {
        _passiveSkills = PassiveSkillManager.Instance.GetThreeRandomChoices();
        
        SetPanelInfo(_passiveSkillNameA, _passiveSkillIconA, _passiveSkillDescriptionA, 0);
        SetPanelInfo(_passiveSkillNameB, _passiveSkillIconB, _passiveSkillDescriptionB, 1);
        SetPanelInfo(_passiveSkillNameC, _passiveSkillIconC, _passiveSkillDescriptionC, 2);
    }

    private void SetPanelInfo(TextMeshProUGUI name, Image icon, TextMeshProUGUI description, int index)
    {
        PassiveSkillSO skill = _passiveSkills[index];
        name.text = skill.SkillName;
        icon.sprite = skill.Icon;
        description.text = skill.Description;
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
