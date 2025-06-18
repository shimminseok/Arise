using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICommandCenterHP : MonoBehaviour
{
    [SerializeField] private Slider hpSlider;
    [SerializeField] private TMP_Text hpText;

    private StatManager centerStat;

    private void Start()
    {
        if (CommandCenter.Instance != null)
        {
            centerStat = CommandCenter.Instance.StatManager;

            centerStat.OnStatChanged += UpdateHPUI;

            UpdateHPUI();
        }
    }

    private void OnDestroy()
    {
        if (centerStat != null)
            centerStat.OnStatChanged -= UpdateHPUI;
    }

    private void UpdateHPUI()
    {
        float curHp = centerStat.GetValue(StatType.CurHp);
        float maxHp = centerStat.GetValue(StatType.MaxHp);

        hpSlider.value = curHp / maxHp;
        hpText.text = $"{(int)curHp} / {(int)maxHp}";
    }
}