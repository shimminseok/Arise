using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIGoldText : MonoBehaviour
{
    [SerializeField] private TMP_Text goldText;

    private void Awake()
    {
        SceneLoader.Instance.AddChangeSceneEvent(WaitForGoldManagerAndSubscribe);
    }

    private void WaitForGoldManagerAndSubscribe()
    {
        goldText.text = GoldManager.Instance.CurrentGold.ToString();
        GoldManager.Instance.OnGoldChanged += UpdateGoldText;
    }

    private void UpdateGoldText(int newGold)
    {
        goldText.text = newGold.ToString();
    }
}