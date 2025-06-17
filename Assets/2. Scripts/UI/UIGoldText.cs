using TMPro;
using UnityEngine;
using System.Collections;

public class UIGoldText : MonoBehaviour
{
    [SerializeField] private TMP_Text goldText;

    private void OnEnable()
    {
        StartCoroutine(WaitForGoldManagerAndSubscribe());
    }

    private void OnDisable()
    {
        if (GoldManager.Instance != null)
        {
            GoldManager.Instance.OnGoldChanged -= UpdateGoldText;
        }
    }

    private IEnumerator WaitForGoldManagerAndSubscribe()
    {
        // GoldManager가 생성될 때까지 대기
        while (GoldManager.Instance == null)
        {
            yield return null;
        }

        // 초기 골드 텍스트 설정
        goldText.text = GoldManager.Instance.CurrentGold.ToString();

        // 이벤트 등록
        GoldManager.Instance.OnGoldChanged += UpdateGoldText;
    }

    private void UpdateGoldText(int newGold)
    {
        goldText.text = newGold.ToString();
    }
}