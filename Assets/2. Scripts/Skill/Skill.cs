using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    protected SkillSO SkillData;
    protected float _duration;
    

    public void Initialize(SkillSO data)
    {
        SkillData = data;
        _duration = SkillData.Duration;
    }

    public void Excute(Transform origin)
    {
        OnStart();
        Apply(origin);
        SkillManager.Instance.StartCooldown(SkillData.ID, SkillData.Cooldown); 
        StartCoroutine(DurationRoutine());
    }

    private IEnumerator DurationRoutine()
    {
        yield return new WaitForSeconds(SkillData.Duration);
        OnEnd();
    }

    protected virtual void OnStart()
    {
        Debug.Log("스킬 시작");
    }

    protected virtual void OnEnd()
    {
        Debug.Log("스킬 종료");
        Destroy(gameObject);
    }
    protected abstract void Apply(Transform origin);

    public void ApplyEffects(IDamageable target)
    {
        if (target is MonoBehaviour monoTarget &&
            monoTarget.TryGetComponent<StatusEffectManager>(out var effectManager))
        {
            foreach (var effectData in SkillData.StatusEffects)
            {
                var effect = BuffFactory.CreateBuff(SkillData.ID, effectData);
                if (effect == null) continue;
                effectManager.ApplyEffect(effect);
                Debug.Log("효과 적용!");
            }
        }
    }
}
