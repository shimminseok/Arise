using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillCooldownIndicator : MonoBehaviour
{
    [SerializeField] private Image cooldownOverlay;
    private float cooldownDuration;
    private bool isCooldown = false;

    public void StartCooldown(float duration)
    {
        cooldownDuration = duration;
        if (!isCooldown)
            StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine()
    {
        isCooldown = true;
        cooldownOverlay.fillAmount = 1f;

        float timer = 0f;
        while (timer < cooldownDuration)
        {
            timer += Time.deltaTime;
            float fill = 1f - (timer / cooldownDuration);
            cooldownOverlay.fillAmount = fill;
            yield return null;
        }

        cooldownOverlay.fillAmount = 0f;
        isCooldown = false;
    }

}