using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class SkillCooldownIndicator : MonoBehaviour
{
    [SerializeField] private Image cooldownOverlay;
    [SerializeField] private TMP_Text cooldownText;
    
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
        cooldownOverlay.enabled = true;
        cooldownText.gameObject.SetActive(true);

        float timer = 0f;
        while (timer < cooldownDuration)
        {
            timer += Time.deltaTime;
            float remaining = Mathf.Max(0f, cooldownDuration - timer);
            float fill = 1f - (timer / cooldownDuration);
            cooldownOverlay.fillAmount = fill;

            cooldownText.text = $"{remaining:F1}";

            yield return null;
        }

        cooldownOverlay.fillAmount = 0f;
        cooldownOverlay.enabled = false;
        cooldownText.gameObject.SetActive(false);
        isCooldown = false;
    }

}