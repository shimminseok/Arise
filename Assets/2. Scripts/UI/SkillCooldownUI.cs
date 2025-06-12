using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillCooldownUI : MonoBehaviour
{
    [SerializeField] private Image cooldownOverlay;
    [SerializeField] private float cooldownDuration = 3f;

    private bool isCooldown = false;

    public void StartCooldown()
    {
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
            cooldownOverlay.fillAmount = 1f - (timer / cooldownDuration);
            yield return null;
        }

        cooldownOverlay.fillAmount = 0f;
        isCooldown = false;
    }
}