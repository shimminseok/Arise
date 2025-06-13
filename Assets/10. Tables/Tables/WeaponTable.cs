using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponTable", menuName = "Tables/WeaponTable", order = 0)]
public class WeaponTable : BaseTable<int, WeaponSO>
{
    protected override string[] DataPath => new[] { "Assets/10. Tables/ScriptableObj/Weapon" };

    public override void CreateTable()
    {
        Type = GetType();
        foreach (WeaponSO item in dataList)
        {
            DataDic[item.ID] = item;
        }
    }
}
