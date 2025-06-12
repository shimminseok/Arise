using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerTable", menuName = "Tables/TowerTable", order = 0)]
public class TowerTable : BaseTable<int, TowerSO>
{
    protected override string[] DataPath => new[] { "Assets/10. Tables/ScriptableObj/Tower" };

    public override void CreateTable()
    {
        Type = GetType();
        foreach (TowerSO item in dataList)
        {
            DataDic[item.ID] = item;
        }
    }
}