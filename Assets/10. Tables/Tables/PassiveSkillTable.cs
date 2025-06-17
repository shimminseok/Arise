using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PassiveSkillTable", menuName = "Tables/PassiveSkillTable", order = 0)]
public class PassiveSkillTable : BaseTable<int, PassiveSkillSO>
{
    protected override string[] DataPath => new[] { "Assets/10. Tables/ScriptableObj/PassiveSkill" };

    public override void CreateTable()
    {
        Type = GetType();
        foreach (PassiveSkillSO item in dataList)
        {
            DataDic[item.ID] = item;
        }
    }
}