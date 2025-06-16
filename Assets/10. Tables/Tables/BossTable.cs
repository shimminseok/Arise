using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossTable", menuName = "Tables/BossTable", order = 0)]
public class BossTable : BaseTable<int, BossSO>
{
    protected override string[] DataPath => new[] { "Assets/10. Tables/ScriptableObj/Boss" };

    public override void CreateTable()
    {
        Type = GetType();
        foreach (BossSO item in dataList)
        {
            DataDic[item.ID] = item;
        }
    }
}