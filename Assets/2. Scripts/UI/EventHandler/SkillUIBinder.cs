using UnityEngine;

public class SkillUIBinder : MonoBehaviour
{
    [System.Serializable]
    public class SkillSlot
    {
        public int skillID;
        public SkillCooldownIndicator cooldownIndicator;
    }

    [SerializeField] private SkillSlot[] skillSlots;

    private void OnEnable()
    {
        SkillManager.OnSkillManagerReady += RegisterToSkillManager;

        // 혹시 SkillManager가 이미 생성되어 있으면 바로 등록
        if (SkillManager.Instance != null)
        {
            RegisterToSkillManager(SkillManager.Instance);
        }
    }

    private void OnDisable()
    {
        SkillManager.OnSkillManagerReady -= RegisterToSkillManager;
    }

    private void RegisterToSkillManager(SkillManager manager)
    {

        foreach (var slot in skillSlots)
        {
            if (slot.cooldownIndicator != null)
            {
                manager.RegisterCooldownIndicator(slot.skillID, slot.cooldownIndicator);
            }
        }
    }
}