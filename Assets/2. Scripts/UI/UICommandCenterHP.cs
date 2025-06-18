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

            centerStat.GetStat<ResourceStat>(StatType.CurHp).OnValueChanged += UpdateHPWarpper;
            UpdateHPWarpper(centerStat.GetValue(StatType.CurHp));
        }
    }

    private void UpdateHPWarpper(float cur)
    {
        UpdateHPUI(cur, centerStat.GetValue(StatType.MaxHp));
    }

    private void UpdateHPUI(float cur, float max)
    {
        hpSlider.value = cur / max;
        hpText.text = $"{(int)cur} / {(int)max}";
    }
}