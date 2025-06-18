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
    public bool IsFast = false;

    private void OnEnable()
    {
        _passiveSkills = PassiveSkillManager.Instance.GetThreeRandomChoices();
        
        SetPanelInfo(_passiveSkillNameA, _passiveSkillIconA, _passiveSkillDescriptionA, 0);
        SetPanelInfo(_passiveSkillNameB, _passiveSkillIconB, _passiveSkillDescriptionB, 1);
        SetPanelInfo(_passiveSkillNameC, _passiveSkillIconC, _passiveSkillDescriptionC, 2);

        Time.timeScale = 0f;
    }

    private void SetPanelInfo(TextMeshProUGUI name, Image icon, TextMeshProUGUI description, int index)
    {
        PassiveSkillSO skill = _passiveSkills[index];
        name.text = skill.SkillName;
        icon.sprite = skill.Icon;
        description.text = skill.Description;
    }

    private void ApplyPassiveSkill(int index)
    {
        PassiveSkillManager.Instance.ApplyPassive(_passiveSkills[index]);
        Debug.Log($"스킬 {index} {_passiveSkills[index].SkillName}");
        Time.timeScale = (!IsFast) ? 1f : 2f;
        gameObject.SetActive(false);
    }

    public void PassiveSkillA() => ApplyPassiveSkill(0);

    public void PassiveSkillB() => ApplyPassiveSkill(1);

    public void PassiveSkillC() => ApplyPassiveSkill(2);
}
