using UnityEngine;
using System;
using System.Collections.Generic;

public class QuestTableTypeDebugger : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("🧪 [QuestTableTypeDebugger] QuestTable 타입 검사 시작");

        // 1. typeof(QuestTable) 직접 출력
        Type requestedType = typeof(QuestTable);
        Debug.Log($"📣 GetTable 요청 타입: {requestedType.FullName}");

        // 2. tableDic에 실제 등록된 타입 출력
        var tableManager = TableManager.Instance;
        if (tableManager == null)
        {
            Debug.LogError("❌ TableManager.Instance 가 null입니다. 씬에 배치되어 있는지 확인하세요.");
            return;
        }

        // 내부 Dictionary는 private이므로 리플렉션으로 접근
        var field = typeof(TableManager).GetField("tableDic", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (field == null)
        {
            Debug.LogError("❌ tableDic 필드를 찾을 수 없습니다.");
            return;
        }

        var tableDic = field.GetValue(tableManager) as IDictionary<Type, ITable>;
        if (tableDic == null)
        {
            Debug.LogError("❌ tableDic이 비어있거나 올바르지 않습니다.");
            return;
        }

        Debug.Log("📦 현재 등록된 tableDic 키 목록:");
        foreach (var key in tableDic.Keys)
        {
            Debug.Log($"- {key.FullName} (== 요청한 타입인가? {key == requestedType})");
        }

        if (tableDic.ContainsKey(requestedType))
        {
            Debug.Log("✅ QuestTable 타입이 정확히 tableDic에 등록되어 있습니다!");
        }
        else
        {
            Debug.LogError("❌ QuestTable 타입이 tableDic에 등록되어 있지 않습니다.");
        }
    }
}