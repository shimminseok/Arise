using UnityEngine;

[CreateAssetMenu(fileName = "QuestTable", menuName = "Tables/QuestTable", order = 0)]
public class QuestTable : BaseTable<string, QuestData>
{
    protected override string[] DataPath => new[] { "Assets/10. Tables/ScriptableObj/Quests" };

    public override void CreateTable()
    {
        Type = GetType();
        foreach (QuestData item in dataList)
        {
            DataDic[item.QuestId] = item;
        }
    }
}