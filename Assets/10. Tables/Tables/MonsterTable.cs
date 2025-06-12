using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterTable", menuName = "Tables/MonsterTable", order = 0)]
public class MonsterTable : BaseTable<int, MonsterSO>
{
    protected override string[] DataPath => new[] { "Assets/10. Tables/ScriptableObj/Monster" };

    public override void CreateTable()
    {
        Type = GetType();
        foreach (MonsterSO item in dataList)
        {
            DataDic[item.ID] = item;
        }
    }
}