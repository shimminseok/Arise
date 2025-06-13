using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSkillSO", menuName = "ScriptableObject/Skill", order = 0)]
public class SkillSO : ScriptableObject
{
    public int ID;

    public Sprite Icon;

    public string Description;
    // 스킬 범위 타입
    // 스킬의 넓이 가로 x 세로

    // 원형 혹은 부채꼴
    public float SkillRange;
    public List<StatusEffectData> StatusEffects = new List<StatusEffectData>();
}