using TMPro;
using UnityEngine;

public class UIPlayerStatPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text attackText;
    [SerializeField] private TMP_Text fireRateText;
    [SerializeField] private TMP_Text moveSpeedText;

    private StatManager playerStatManager;
    private StatManager weaponStatManager;

    public void SetStatManagers(StatManager player, StatManager weapon)
    {
        playerStatManager = player;
        weaponStatManager = weapon;

        if (playerStatManager != null)
            playerStatManager.OnStatChanged += UpdateStatTexts;
        if (weaponStatManager != null)
            weaponStatManager.OnStatChanged += UpdateStatTexts;

        UpdateStatTexts();
    }

    private void OnDestroy()
    {
        if (playerStatManager != null)
            playerStatManager.OnStatChanged -= UpdateStatTexts;
        if (weaponStatManager != null)
            weaponStatManager.OnStatChanged -= UpdateStatTexts;
    }

    public void UpdateStatTexts()
    {
        float attackValue    = GetSafeStatValue(weaponStatManager, StatType.AttackPow);
        float fireRateValue  = GetSafeStatValue(weaponStatManager, StatType.AttackSpd);
        float moveSpeedValue = GetSafeStatValue(playerStatManager, StatType.MoveSpeed);

        attackText.text    = attackValue.ToString();
        fireRateText.text  = fireRateValue.ToString("F1");
        moveSpeedText.text = moveSpeedValue.ToString();
    }

    private float GetSafeStatValue(StatManager manager, StatType statType)
    {
        if (manager != null && manager.Stats.TryGetValue(statType, out var stat))
            return stat.GetCurrent();
        return 0f;
    }
}