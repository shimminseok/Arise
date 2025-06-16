using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillTable", menuName = "Tables/SkillTable", order = 0)]
public class SkillTable : BaseTable<int, SkillSO>
{
    protected override string[] DataPath => new[] { "Assets/10. Tables/ScriptableObj/Skill" };

    public override void CreateTable()
    {
        Type = GetType();
        foreach (SkillSO item in dataList)
        {
            DataDic[item.ID] = item;
        }
    }
}