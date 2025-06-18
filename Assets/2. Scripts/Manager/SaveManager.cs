using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;

[Serializable]
public class GameSaveData
{
    public List<QuestProgress> Quests = new();
    public List<string> UnlockedSkills = new();
    public List<BuildingSaveData> Buildings = new();

    public int PlayerLevel;
    public float PlayerHP;
    public Vector3 PlayerPosition;

    public int Gold;
    public int MaxClearedStage;
}


public class SaveManager : SceneOnlySingleton<SaveManager>
{
    private string SavePath => Path.Combine(Application.persistentDataPath, "save.json");

    public void SaveGame(GameSaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
        Debug.Log("게임 저장 완료: " + SavePath);
    }

    public GameSaveData LoadGame()
    {
        if (!File.Exists(SavePath))
        {
            Debug.LogWarning("저장 파일이 존재하지 않습니다.");
            return null;
        }

        string json = File.ReadAllText(SavePath);
        GameSaveData data = JsonUtility.FromJson<GameSaveData>(json);
        Debug.Log("게임 불러오기 완료");
        return data;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("저장 요청됨");
            var data = CollectSaveDataFromGame();
            SaveGame(data);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("불러오기 요청됨");
            var data = LoadGame();
            if (data != null) ApplySaveDataToGame(data);
        }
    }

    private GameSaveData CollectSaveDataFromGame()
    {
        GameSaveData data = new();

        if (QuestManager.Instance != null)
        {
            foreach (var (quest, progress) in QuestManager.Instance.GetAllProgress())
                data.Quests.Add(progress);
        }

        data.UnlockedSkills = SkillManager.Instance?.GetUnlockedSkillIds() ?? new();

        var player = GameObject.FindWithTag("Player");
        if (player != null && player.TryGetComponent<PlayerController>(out var pc))
        {
            data.PlayerPosition = player.transform.position;
        }

        data.Gold = GoldManager.Instance?.Gold ?? 0;
        data.MaxClearedStage = StageManager.Instance?.MaxStage ?? 0;

        data.Buildings = BuildingManager.Instance?.GetAllBuildingData() ?? new();

        return data;
    }

    private void ApplySaveDataToGame(GameSaveData data)
    {
        QuestManager.Instance?.ApplyLoadedProgress(data.Quests);
        SkillManager.Instance?.ApplyUnlockedSkillIds(data.UnlockedSkills);

        var player = GameObject.FindWithTag("Player");
        if (player != null && player.TryGetComponent<PlayerController>(out var pc))
        {
            player.transform.position = data.PlayerPosition;
        }

        if (GoldManager.Instance != null)
            GoldManager.Instance.Gold = data.Gold;

        if (StageManager.Instance != null)
            StageManager.Instance.MaxStage = data.MaxClearedStage;

        BuildingManager.Instance?.RebuildFromData(data.Buildings);

        Debug.Log("게임 불러오기 완료 및 적용됨");
    }
}