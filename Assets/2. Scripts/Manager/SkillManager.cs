using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : SceneOnlySingleton<SkillManager>
{
    public static event Action<SkillManager> OnSkillManagerReady;

    private readonly Dictionary<int, Skill> _skillInstances = new();
    private readonly Dictionary<int, float> _skillCooldowns = new();
    private readonly Dictionary<int, SkillCooldownIndicator> _cooldownIndicators = new();
    
    [SerializeField] private GameObject _owner;

    private HashSet<string> _unlockedSkillIds = new();
    public GameObject Owner => _owner;
    protected override void Awake()
    {
        base.Awake();
        OnSkillManagerReady?.Invoke(this);
    }

    public void Initialize(GameObject owner)
    {
        _owner = owner;
    }

    public void RegisterCooldownIndicator(int skillID, SkillCooldownIndicator indicator)
    {
        _cooldownIndicators[skillID] = indicator;
    }

    public void ExecuteSkill(int skillID)
    {
        SkillTable skillTable = TableManager.Instance.GetTable<SkillTable>();
        if (skillTable == null)
        {
            Debug.LogError("SkillTable이 로드되지 않았습니다.");
            return;
        }

        if (!skillTable.DataDic.TryGetValue(skillID, out SkillSO skillData))
        {
            Debug.LogWarning($"Skill ID {skillID}가 테이블에 없습니다.");
            return;
        }

        if (IsOnCooldown(skillID))
        {
            Debug.LogWarning($"Skill ID {skillID}는 쿨다운 중입니다. 남은 시간: {GetRemainingCooldown(skillID):F2}s");
            return;
        }

        Skill skillInstance = skillData.CreateSkillInstance(_owner);
        _skillInstances[skillID] = skillInstance;
        skillInstance.Excute(_owner.transform);

        StartCooldown(skillID, skillData.Cooldown);
        QuestManager.Instance.UpdateProgress(QuestType.UseSkill, 1);
    }

    public void StartCooldown(int skillID, float cooldown)
    {
        _skillCooldowns[skillID] = Time.time + cooldown;
        if (_cooldownIndicators.TryGetValue(skillID, out var indicator))
        {
            indicator.StartCooldown(cooldown);
        }
    }

    public bool IsOnCooldown(int skillID)
    {
        if (!_skillCooldowns.TryGetValue(skillID, out float endTime)) return false;
        return Time.time < endTime;
    }

    public float GetRemainingCooldown(int skillID)
    {
        if (!_skillCooldowns.TryGetValue(skillID, out float endTime)) return 0f;
        return Mathf.Max(0f, endTime - Time.time);
    }

    public List<string> GetUnlockedSkillIds()
    {
        return new List<string>(_unlockedSkillIds);
    }

    public void ApplyUnlockedSkillIds(List<string> ids)
    {
        _unlockedSkillIds = new HashSet<string>(ids);
    }


}
