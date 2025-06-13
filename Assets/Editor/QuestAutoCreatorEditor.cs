using UnityEngine;
using UnityEditor;
using System.IO;

public class QuestAutoCreatorEditor : EditorWindow
{
    private string questId = "quest_kill_10";
    private string title = "10마리 처치하기";
    private string description = "적을 10마리 처치하세요.";
    private QuestType questType = QuestType.KillEnemies;
    private int targetValue = 10;
    private int rewardGold = 500;

    [MenuItem("Tools/Quest Creator")]
    public static void ShowWindow()
    {
        GetWindow<QuestAutoCreatorEditor>("퀘스트 자동 생성기");
    }

    private void OnGUI()
    {
        GUILayout.Label("퀘스트 정보 입력", EditorStyles.boldLabel);

        questId = EditorGUILayout.TextField("Quest ID", questId);
        title = EditorGUILayout.TextField("Title", title);
        description = EditorGUILayout.TextField("Description", description);
        questType = (QuestType)EditorGUILayout.EnumPopup("Condition Type", questType);
        targetValue = EditorGUILayout.IntField("Target Value", targetValue);
        rewardGold = EditorGUILayout.IntField("Reward Gold", rewardGold);

        EditorGUILayout.Space();

        if (GUILayout.Button("퀘스트 생성"))
        {
            CreateQuest();
        }
    }

    private void CreateQuest()
    {
        // 생성할 ScriptableObject 인스턴스 생성
        QuestData quest = ScriptableObject.CreateInstance<QuestData>();
        quest.QuestId = questId;
        quest.Title = title;
        quest.Description = description;
        quest.Condition = new QuestCondition { Type = questType, TargetValue = targetValue };
        quest.RewardGold = rewardGold;

        // 저장 경로 확인 및 생성
        string folderPath = "Assets/10. Tables/ScriptableObj/Quests";
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string assetPath = $"{folderPath}/{questId}.asset";
        AssetDatabase.CreateAsset(quest, assetPath);
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = quest;

        Debug.Log($"✅ 퀘스트 생성 완료: {assetPath}");
    }
}